namespace Properties.Tests;

public class PropertyDictionaryTests
{
    private TestPropertyDictionary _dictionary = new();

    [SetUp]
    public void SetUp()
    {
        _dictionary = new TestPropertyDictionary();
    }

    [Test]
    public void HasPropertyProvidesExpectedValue()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_dictionary.HasProperty("Test"), Is.False);
            Assert.That(
                _dictionary
                   .WithProperty("Test", true)
                   .HasProperty("Test"),
                Is.True);
        });
    }

    [Test]
    public void WithPropertyProvidesNewObjectWhenPropertyIsNotAlreadyExhibited()
    {
        Assert.That(_dictionary, Is.Not.SameAs(_dictionary.WithProperty("Test", true)));
    }

    [Test]
    public void WithPropertyProvidesSameObjectWhenPropertyIsAlreadyExhibited()
    {
        var withProperty = _dictionary.WithProperty("Test", true);
        Assert.That(withProperty, Is.SameAs(withProperty.WithProperty("Test", true)));
    }

    [Test]
    public void WithPropertyProvidesNewObjectWhenPropertyIsAExhibited()
    {
        var withProperty = _dictionary.WithProperty("Test", true);
        Assert.That(withProperty, Is.Not.SameAs(withProperty.WithoutProperty("Test")));
    }

    [Test]
    public void WithoutPropertyProvidesSameObjectWhenPropertyIsNotExhibited()
    {
        Assert.That(_dictionary, Is.SameAs(_dictionary.WithoutProperty("Test")));
    }
}
