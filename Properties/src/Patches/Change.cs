namespace Properties.Patches;

internal interface IChange
{
    bool IsAddition { get; }
    Property Property { get; }

    T Apply<T>(T dictionary) where T : IPropertyDictionary<T>;
}

internal static class Change
{
    public static IChange Addition(Property property)
    {
        return new AdditionChange(property);
    }

    public static IChange Deletion(Property property)
    {
        return new DeletionChange(property);
    }

    private sealed class AdditionChange(Property property) : IChange
    {
        public bool IsAddition => true;
        public Property Property => property;

        public T Apply<T>(T dictionary) where T : IPropertyDictionary<T>
        {
            return dictionary.WithProperty(Property.Name, Property.Value);
        }
    }

    private sealed class DeletionChange(Property property) : IChange
    {
        public bool IsAddition => false;
        public Property Property => property;

        public T Apply<T>(T dictionary) where T : IPropertyDictionary<T>
        {
            return dictionary.WithoutProperty(Property.Name);
        }
    }
}
