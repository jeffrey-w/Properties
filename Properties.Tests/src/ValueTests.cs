namespace Properties.Tests;

public class ValueTests
{
    private static readonly Value Boolean = true;
    private static readonly Value Integer = 1;
    private static readonly Value Number = 1f;
    private static readonly Value String = "Test";

    [Test]
    public void BooleanOrDefaultIsFalseWhenValueIsNull()
    {
        Assert.That(Value.Empty.BooleanOrDefault(), Is.False);
    }

    [Test]
    public void BooleanOrDefaultThrowsWhenValueIsNotBool()
    {
        Assert.Throws<InvalidOperationException>(() => Integer.BooleanOrDefault());
    }

    [Test]
    public void IntegerOrDefaultIsZeroWhenValueIsNull()
    {
        Assert.That(Value.Empty.IntegerOrDefault(), Is.Zero);
    }

    [Test]
    public void IntegerOrDefaultThrowsValueWhenValueIsNotInt()
    {
        Assert.Throws<InvalidOperationException>(() => Number.IntegerOrDefault());
    }

    [Test]
    public void NumberOrDefaultIsZeroWhenValueIsNull()
    {
        // Assert.Equal(0, Flammability.Properties.Value.Empty.NumberOrDefault());
        Assert.That(Value.Empty.NumberOrDefault(), Is.Zero);
    }

    [Test]
    public void NumberOrDefaultThrowsWhenValueIsNotDouble()
    {
        Assert.Throws<InvalidOperationException>(() => String.NumberOrDefault());
    }

    [Test]
    public void StringOrDefaultIsEmptyWhenValueIsNull()
    {
        Assert.That(Value.Empty.StringOrDefault(), Is.Empty);
    }

    [Test]
    public void StringOrDefaultThrowsWhenValueIsNotCharacterSequence()
    {
        Assert.Throws<InvalidOperationException>(() => Boolean.StringOrDefault());
    }
}
