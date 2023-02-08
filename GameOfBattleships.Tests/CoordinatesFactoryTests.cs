using NUnit.Framework;
using FluentAssertions;

namespace GameOfBattleships.Tests;

public class CoordinatesFactoryTests
{
    [TestCase("A1", 'A', 1)]
    [TestCase("C12", 'C', 12)]
    [TestCase("Z98", 'Z', 98)]
    public void TryCreateFromUserInput_ShouldParseValidCoordinates(string userInput, char parsedX, int parsedY)
    {
        var successful = CoordinatesFactory.TryCreateFromUserInput(userInput, out var coordinates);
        
        successful.Should().BeTrue();
        coordinates!.X.Should().Be(parsedX);
        coordinates.Y.Should().Be(parsedY);
    }

    [Test]
    public void TryCreateFromUserInput_ShouldAcceptLowercaseLetters()
    {
        var successful = CoordinatesFactory.TryCreateFromUserInput("a1", out var coordinates);

        successful.Should().BeTrue();
        coordinates!.X.Should().Be('A');
        coordinates.Y.Should().Be(1);
    }

    [Test]
    public void TryCreateFromUserInput_ShouldTrimInput()
    {
        var successful = CoordinatesFactory.TryCreateFromUserInput("  A1  ", out var coordinates);

        successful.Should().BeTrue();
        coordinates!.X.Should().Be('A');
        coordinates.Y.Should().Be(1);
    }

    [TestCase("A 1")]
    [TestCase("A")]
    [TestCase("1")]
    [TestCase("AB12")]
    [TestCase("A123")]
    public void TryCreateFromUserInput_ShouldNotParseInvalidCoordinates(string userInput)
    {
        var successful = CoordinatesFactory.TryCreateFromUserInput(userInput, out var coordinates);
        
        successful.Should().BeFalse();
        coordinates.Should().BeNull();
    }
}