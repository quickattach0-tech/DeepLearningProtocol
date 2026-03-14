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
        public void CoreTranslation_GetUptimePercentage_ReturnsValidPercentage()
        {
            // Act
            var percentage = _ct.GetUptimePercentage();

            // Assert
            Assert.True(percentage >= 0 && percentage <= 100);
        }
    }
}
