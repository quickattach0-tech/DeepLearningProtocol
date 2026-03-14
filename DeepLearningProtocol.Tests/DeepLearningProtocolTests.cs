using Xunit;
using DeepLearningProtocol; // Your main namespace

namespace DeepLearningProtocol.Tests
{
    public class DeepLearningProtocolTests
    {
        private readonly DeepLearningProtocol _protocol = new();
        private readonly CoreTranslation _ct = new();

        [Fact]
        public void GetCurrentState_ReturnsInitialState()
        {
            // Act
            var state = _protocol.GetCurrentState();

            // Assert
            Assert.Equal("Initial", state);
        }

        [Fact]
        public void UpdateState_ChangesCurrentState()
        {
            // Act
            _protocol.UpdateState("Test State");

            // Assert
            Assert.Equal("Test State", _protocol.GetCurrentState());
        }

        [Fact]
        public void SetAim_UpdatesAimAndState()
        {
            // Act
            var result = _protocol.SetAim("Test Goal");

            // Assert
            Assert.Equal("Aim set to: Test Goal", result);
            Assert.Equal("Aiming: Test Goal", _protocol.GetCurrentState());
        }

        [Fact]
        public void PursueAim_ReturnsCoreResultWithAim()
        {
            // Arrange
            _protocol.SetAim("Test Goal");
            var currentState = "Input State";

            // Act
            var result = _protocol.PursueAim(currentState);

            // Assert
            Assert.Contains("[Aim Pursuit]", result);
            Assert.Contains("[Abstract Core] Deep abstract processing: Input State", result);
            Assert.Contains("towards Test Goal", result);
        }

        [Theory]
        [InlineData("Input", 0, "[Depth 0] Input")]
        [InlineData("Input", 1, "[Depth 1] [Abstract Core] Deep abstract processing: Input")]
        [InlineData("Input", 2, "[Depth 2] [Abstract Core] Deep abstract processing: [Abstract Core] Deep abstract processing: Input")]
        public void ProcessAtDepth_AppliesCorrectDepth(string input, int depth, string expectedContains)
        {
            // Act
            var result = _protocol.ProcessAtDepth(input, depth);

            // Assert
            Assert.StartsWith($"[Depth {depth}]", result);
            Assert.Contains(expectedContains, result);
            Assert.Equal($"Depth {depth} processed", _protocol.GetCurrentState());
        }

        [Fact]
        public void ExecuteProtocol_FullFlow_ReturnsExpectedOutput()
        {
            // Act
            var result = _protocol.ExecuteProtocol(
                initialInput: "Raw Data",
                goal: "Test Goal",
                depth: 2
            );

            // Assert
            Assert.Contains("[Aim Pursuit]", result);
            Assert.Contains("[Depth 2]", result);
            Assert.Contains("[Abstract Core] Deep abstract processing: [Abstract Core] Deep abstract processing: Raw Data", result);
            Assert.Contains("towards Test Goal", result);
            Assert.Equal("Depth 2 processed", _protocol.GetCurrentState());
        }

        // Core Translation Tests
        [Fact]
        public void CoreTranslation_AssessQuality_HighQualityContent_ReturnsHighScore()
        {
            // Act
            var score = _ct.AssessQuality("This is a well-written, properly formatted sentence with good grammar.");

            // Assert
            Assert.True(score >= 70);
        }

        [Fact]
        public void CoreTranslation_AssessQuality_LowQualityContent_ReturnsLowScore()
        {
            // Act
            var score = _ct.AssessQuality("bad");

            // Assert
            Assert.True(score < 30);
        }

        [Fact]
        public void CoreTranslation_Translate_EnglishToSpanish_ReturnsTranslatedText()
        {
            // Act
            var result = _ct.Translate("hello world", CoreTranslation.Language.Spanish);

            // Assert
            Assert.Equal("hola mundo", result);
        }

        [Fact]
        public void CoreTranslation_Translate_EnglishToArabic_ReturnsTranslatedText()
        {
            // Act
            var result = _ct.Translate("hello", CoreTranslation.Language.Arabic);

            // Assert
            Assert.Equal("مرحبا", result);
        }

        [Fact]
        public void CoreTranslation_Translate_EnglishToFrench_ReturnsTranslatedText()
        {
            // Act
            var result = _ct.Translate("goodbye", CoreTranslation.Language.French);

            // Assert
            Assert.Equal("au revoir", result);
        }

        [Fact]
        public void CoreTranslation_Translate_EnglishToGerman_ReturnsTranslatedText()
        {
            // Act
            var result = _ct.Translate("deep learning protocol", CoreTranslation.Language.German);

            // Assert
            Assert.Equal("Deep Learning Protokoll", result);
        }

        [Fact]
        public void CoreTranslation_Translate_EnglishToItalian_ReturnsTranslatedText()
        {
            // Act
            var result = _ct.Translate("quality translation", CoreTranslation.Language.Italian);

            // Assert
            Assert.Equal("traduzione di qualità", result);
        }

        [Fact]
        public void CoreTranslation_RecordUptimeEvent_IncreasesUptimeCount()
        {
            // Act
            var before = _ct.GetUptimeCalendar();
            _ct.RecordUptimeEvent();
            var after = _ct.GetUptimeCalendar();

            // Assert
            Assert.True(after.Count >= before.Count);
        }

        [Fact]
        public void CoreTranslation_TestTranslation_SingleIteration_ReturnsValidResult()
        {
            // Act
            var result = _ct.TestTranslation("hello world", CoreTranslation.Language.Spanish, 1);

            // Assert
            Assert.Equal("hello world", result.CoreText);
            Assert.Equal(CoreTranslation.Language.Spanish, result.TargetLanguage);
            Assert.Equal(1, result.Iterations);
            Assert.Single(result.Results);
            Assert.Single(result.QualityScores);
            Assert.Single(result.Timestamps);
            Assert.True(result.ConsistencyScore >= 0 && result.ConsistencyScore <= 100);
        }

        [Fact]
        public void CoreTranslation_TestTranslation_MultipleIterations_CalculatesConsistency()
        {
            // Act
            var result = _ct.TestTranslation("deep learning protocol", CoreTranslation.Language.French, 3);

            // Assert
            Assert.Equal(3, result.Iterations);
            Assert.Equal(3, result.Results.Count);
            Assert.Equal(3, result.QualityScores.Count);
            Assert.Equal(3, result.Timestamps.Count);
            Assert.True(result.ConsistencyScore >= 0 && result.ConsistencyScore <= 100);
            Assert.True(result.AverageQuality >= 0 && result.AverageQuality <= 100);
        }

        [Fact]
        public void CoreTranslation_TestTranslation_ConsistentTranslations_ReturnsHighConsistency()
        {
            // Act
            var result = _ct.TestTranslation("goodbye", CoreTranslation.Language.Spanish, 5);

            // Assert
            Assert.True(result.ConsistencyScore >= 80); // Should be highly consistent for simple translations
            Assert.True(result.IsConsistent); // All results should be the same
        }

        // Image Processing Tests
        [Fact]
        public void CoreTranslation_ProcessImage_NonExistentFile_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _ct.ProcessImage("nonexistent.png"));
        }

        [Fact]
        public void CoreTranslation_ProcessImage_ValidImage_ReturnsResult()
        {
            // Arrange - Use the instruction.png from the Instructions folder
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Instructions", "Instruction.png");

            // Act
            var result = _ct.ProcessImage(imagePath);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(imagePath, result.ImagePath);
            Assert.True(result.Width > 0);
            Assert.True(result.Height > 0);
            Assert.True(result.PixelCount > 0);
            Assert.NotNull(result.ColorAnalysis);
            Assert.NotNull(result.Features);
            Assert.True(result.Features.Length > 0);
        }
    }
}
