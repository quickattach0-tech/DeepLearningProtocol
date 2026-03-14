using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Threading.Tasks;
using DeepLearningProtocol.Web.Hubs;
using Tesseract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace DeepLearningProtocol.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IHubContext<ChatHub> _hubContext;

    public IndexModel(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostUploadImage(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ViewData["ImageResult"] = "No file uploaded.";
            return Page();
        }

        // Save uploaded file temporarily
        var tempPath = Path.GetTempFileName();
        using (var stream = new FileStream(tempPath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        try
        {
            // Perform OCR on the uploaded image
            string extractedText = ExtractTextFromImage(tempPath);
            
            // Send the extracted text as a message to the conference
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "InstructionReader", 
                $"📄 Extracted from {Path.GetFileName(imageFile.FileName)}: {extractedText}");
            
            // Also log the processing details
            await _hubContext.Clients.All.SendAsync("ReceiveImageLog", 
                $"[{DateTime.Now:HH:mm:ss}] Processed {Path.GetFileName(imageFile.FileName)}\n" +
                $"File size: {imageFile.Length} bytes\n" +
                $"Upload time: {DateTime.Now}\n" +
                $"Extracted text length: {extractedText.Length} characters");

            ViewData["ImageResult"] = $"Text extracted and added to chat: {extractedText}";
        }
        catch (Exception ex)
        {
            var errorMsg = $"Error processing image: {ex.Message}";
            ViewData["ImageResult"] = errorMsg;

            // Broadcast error to conference
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "InstructionReader", 
                $"❌ Error reading instruction: {errorMsg}");
        }
        finally
        {
            // Clean up
            if (System.IO.File.Exists(tempPath))
                System.IO.File.Delete(tempPath);
        }

        return Page();
    }

    private string ExtractTextFromImage(string imagePath)
    {
        try
        {
            using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        string extractedText = page.GetText().Trim();
                        return extractedText.Length > 0 ? extractedText : "No text detected in image.";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return $"OCR failed: {ex.Message}";
        }
    }
}
