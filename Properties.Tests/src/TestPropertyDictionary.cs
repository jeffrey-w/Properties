using System.Collections.Immutable;

namespace Properties.Tests;

public sealed class TestPropertyDictionary : BasePropertyDictionary<TestPropertyDictionary>
{
    public TestPropertyDictionary() : this(ImmutableDictionary<string, Property>.Empty)
    {
    }

    private TestPropertyDictionary(ImmutableDictionary<string, Property> properties) : base(properties)
    {
    }

    protected override TestPropertyDictionary WithProperties(ImmutableDictionary<string, Property> properties)
    {
        return new TestPropertyDictionary(properties);
    }
}
