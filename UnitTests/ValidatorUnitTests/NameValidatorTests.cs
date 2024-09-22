namespace UnitTests;

public class NameValidatorTests
{
    private static ValidNameAttribute nameValidator = new("first name");

    [Theory]
    [InlineData("Jiří")]
    [InlineData("Strauß")]
    [InlineData("Søren")]
    [InlineData("Çağlar")]
    [InlineData("Göran")]
    public void CorrectNamesWithLanguageSpecificLetters(string name)
    {
        Assert.True(nameValidator.IsValid(name));
    }

    [Theory]
    [InlineData("Dr. Etker")]
    [InlineData("Karel IV.")]
    [InlineData("Jane-Anne")]
    [InlineData("O'Brian")]
    public void CorrectNamesWithPunctuation(string name)
    {
        Assert.True(nameValidator.IsValid(name));
    }

    [Theory]
    [InlineData("Karel 4")]
    [InlineData("Mandalore195")]
    [InlineData("42")]
    public void IncorrectContainsDigit(string name)
    {
        Assert.False(nameValidator.IsValid(name));
    }

    [Theory]
    [InlineData("prof. RNDr. Aleš Pultr, DrSc.")]
    [InlineData("karel@mail")]
    [InlineData("nekdo,dulezity")]
    [InlineData("fan.pan")]
    [InlineData("(jelito)")]
    public void IncorrectContainsPunctuation(string name)
    {
        Assert.False(nameValidator.IsValid(name));
    }

    [Theory]
    [InlineData("anicka+matyasek")]
    [InlineData("prom = deset")]
    public void IncorrectContainsSymbols(string name)
    {
        Assert.False(nameValidator.IsValid(name));
    }
}