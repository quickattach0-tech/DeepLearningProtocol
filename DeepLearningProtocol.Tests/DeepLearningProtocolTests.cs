using Xunit;
using DeepLearningProtocol; // Your main namespace

namespace DeepLearningProtocol.Tests
{
    public class DeepLearningProtocolTests
    {
        private readonly DeepLearningProtocol _protocol = new();

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
    }
}
