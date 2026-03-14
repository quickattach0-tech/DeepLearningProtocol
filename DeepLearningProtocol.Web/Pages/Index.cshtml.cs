using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;

namespace DeepLearningProtocol.Web.Pages;

public class IndexModel : PageModel
{
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
            // Basic image analysis (simplified version)
            var result = AnalyzeImage(tempPath);
            ViewData["ImageResult"] = result;
        }
        catch (Exception ex)
        {
            ViewData["ImageResult"] = $"Error processing image: {ex.Message}";
        }
        finally
        {
            // Clean up
            if (System.IO.File.Exists(tempPath))
                System.IO.File.Delete(tempPath);
        }

        return Page();
    }

    private string AnalyzeImage(string imagePath)
    {
        // Simplified analysis - in real app, use CoreTranslation.ProcessImage
        var fileInfo = new FileInfo(imagePath);
        return $"Image processed successfully!\n" +
               $"File size: {fileInfo.Length} bytes\n" +
               $"File name: {Path.GetFileName(imagePath)}\n" +
               $"Upload time: {DateTime.Now}\n\n" +
               $"Note: Full OCR and translation processing available in console app.";
    }
}
