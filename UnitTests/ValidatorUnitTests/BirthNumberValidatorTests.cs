namespace UnitTests;

public class BirthNumberValidatorTests
{

    private static ValidBirthNumberAttribute birthNumberValidator = new();

    [Theory]
    [InlineData("1151236438")]
    [InlineData("6310183550")]
    [InlineData("7155177018")]
    [InlineData("2011130319")]
    public void CorrectLengthAndDateAndDivisible(string birthNumber)
    {
        Assert.True(birthNumberValidator.IsValid(birthNumber));
    }

    [Theory]
    [InlineData("11512303")]
    [InlineData("63101835500")]
    public void IncorrectLengthButDivisibleAndCorrectDate(string birthNumber)
    {
        Assert.False(birthNumberValidator.IsValid(birthNumber));
    }

    [Theory]
    [InlineData("1151236439")]
    [InlineData("6310183551")]
    public void CorrectLengthAndDateButNotDivisible(string birthNumber)
    {
        Assert.False(birthNumberValidator.IsValid(birthNumber));
    }
}