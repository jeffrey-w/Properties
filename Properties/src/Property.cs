using Extra.Guard;

namespace Properties;

/// <summary>
/// The <c>Property</c> class represents an association between some
/// <see cref="Properties.Value">data</see> and a descriptive identifier.
/// </summary>
public sealed class Property
{
    private Property(string name, Value value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>The identifier for this <c>Property</c>.</summary>
    public string Name { get; }

    /// <summary>The data associated with this <c>Property</c>.</summary>
    public Value Value { get; }

    /// <summary>
    /// The raw data that backs the <see cref="Value" /> associated with this
    /// <c>Property</c>.
    /// </summary>
    public object? Payload => Value.Payload;

    /// <summary>
    /// Creates a new <c>Property</c> with the specified <paramref name="name" /> and
    /// <paramref name="value" />.
    /// </summary>
    /// <param name="name">The identifier for the new <c>Property</c>.</param>
    /// <param name="value">
    /// The <see cref="Properties.Value">data</see> associated with the new
    /// <c>Property</c>.
    /// </param>
    /// <returns>A new <c>Property</c>.</returns>
    /// <exception cref="ArgumentException">
    /// If the specified <paramref name="name" /> is <c>null</c>, empty, or only
    /// whitespace.
    /// </exception>
    public static Property Make(string name, Value value)
    {
        return new Property(Against.NullOrWhitespace(name, nameof(name)), value);
    }

    /// <summary>Provides the attributes of this <c>Property</c>.</summary>
    /// <param name="name">The identifier of this <c>Property</c>.</param>
    /// <param name="value">The data associated with this <c>Property</c>.</param>
    public void Deconstruct(out string name, out Value value)
    {
        name = Name;
        value = Value;
    }
}
