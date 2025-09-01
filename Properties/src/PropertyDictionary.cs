using System.Collections.Immutable;
using Properties.Patches;

namespace Properties;

/// <summary>
/// The <c>IPropertyDictionary</c> interface provides properties and operations on
/// an entity that exhibits arbitrary <see cref="Property">attributes</see>.
/// </summary>
/// <typeparam name="T">The natural type of this <c>IPropertyDictionary</c>.</typeparam>
public interface IPropertyDictionary<T> where T : IPropertyDictionary<T>
{
    /// <summary>
    /// Provides the <see cref="Value" /> associated with the specified
    /// <see cref="Property" /> <paramref name="name" />, or the
    /// <see cref="Value.Empty">empty value</see> if this <c>IPropertyDictionary</c>
    /// contains no such association.
    /// </summary>
    /// <param name="name">The identifier for the queried <see cref="Property" />.</param>
    /// <returns>
    /// The <see cref="Value" /> associated with the <see cref="Property" /> by the
    /// specified <paramref name="name" />.
    /// </returns>
    Value this[string name] { get; }

    /// <summary>
    /// The <see cref="Property">attributes</see> associated with this
    /// <c>IPropertyDictionary</c>.
    /// </summary>
    IEnumerable<Property> Properties { get; }

    /// <summary>
    /// Determines whether this <c>IPropertyDictionary</c> is associated with a
    /// <see cref="Property" /> by the specified <paramref name="name" />.
    /// </summary>
    /// <param name="name">The identifier for the queried <see cref="Property" />.</param>
    /// <returns>
    /// <c>true</c> if this <c>IPropertyDictionary</c> exhibits a
    /// <see cref="Property" /> with the specified <paramref name="name" />.
    /// </returns>
    bool HasProperty(string name);

    /// <summary>
    /// Provides a new <c>IPropertyDictionary</c> with the same attributes as this one,
    /// in addition to the <see cref="Property" /> by the specified
    /// <paramref name="name" /> associated with the specified
    /// <paramref name="value" />.
    /// </summary>
    /// <param name="name">The identifier for the <see cref="Property" /> to include.</param>
    /// <param name="value">
    /// The data associated with the <see cref="Property" /> to include.
    /// </param>
    /// <returns></returns>
    T WithProperty(string name, Value value);

    /// <summary>
    /// Provides a new <c>IPropertyDictionary</c> with the same attributes as this one,
    /// except without the <see cref="Property" /> with the specified
    /// <paramref name="name" />.
    /// </summary>
    /// <param name="name">The identifier for the <see cref="Property" /> to exclude.</param>
    /// <returns>A new <c>IPropertyDictionary</c>.</returns>
    T WithoutProperty(string name);

    /// <summary>
    /// Provides the <see cref="Patch" /> that defines the changes to the
    /// <paramref name="other" /> <c>IPropertyDictionary</c> necessary to obtain this
    /// one.
    /// </summary>
    /// <param name="other">The <c>IPropertyDictionary</c> to compare this one to.</param>
    /// <returns>A new <see cref="Patch" />.</returns>
    Patch Diff(T other);
}

/// <summary>
/// The <c>BasePropertyDictionary</c> class provides a minimal implementation of
/// the <see cref="IPropertyDictionary{T}" /> interface.
/// </summary>
/// <typeparam name="T">The natural type of this <c>BasePropertyDictionary</c>.</typeparam>
public abstract class BasePropertyDictionary<T> : IPropertyDictionary<T> where T : IPropertyDictionary<T>
{
    /// <inheritdoc />
    public Value this[string name] => _properties.TryGetValue(name, out var property) ? property.Value : Value.Empty;
    
    private T This => (T)(IPropertyDictionary<T>)this;
    
    private readonly ImmutableDictionary<string, Property> _properties;

    /// <summary>
    /// Creates a new <c>BasePropertyDictionary</c> with the
    /// <see cref="Property">properties</see> by the specified
    /// <paramref name="properties" />.
    /// </summary>
    /// <param name="properties">
    /// A collection of <see cref="Property">properties</see>, indexed by their name.
    /// </param>
    protected BasePropertyDictionary(ImmutableDictionary<string, Property> properties)
    {
        _properties = properties;
    }

    /// <inheritdoc />
    public IEnumerable<Property> Properties =>
        _properties
           .OrderBy(pair => pair.Key)
           .Select(pair => pair.Value);

    /// <inheritdoc />
    public bool HasProperty(string name)
    {
        return _properties.ContainsKey(name);
    }

    /// <inheritdoc />
    public T WithProperty(string name, Value value)
    {
        if (_properties.TryGetValue(name, out var other))
        {
            if (other.Value.Equals(value))
            {
                return This;
            }
        }
        return WithProperties(_properties.SetItem(name, Property.Make(name, value)));
    }

    /// <inheritdoc />
    public T WithoutProperty(string name)
    {
        return HasProperty(name) ? WithProperties(_properties.Remove(name)) : This;
    }

    /// <inheritdoc />
    public Patch Diff(T other)
    {
        return Patches.Diff.ComputeFor(Properties, other.Properties);
    }

    /// <summary>
    /// Provides a new <c>BasePropertyDictionary</c>, with the appropriate compile-time
    /// type, that is associated with the <see cref="Property">properties</see> by the
    /// specified <paramref name="properties" />.
    /// </summary>
    /// <param name="properties">
    /// A collection of <see cref="Property">properties</see>, indexed by their unique
    /// identifiers.
    /// </param>
    /// <returns>A new <c>BasePropertyDictionary</c>.</returns>
    protected abstract T WithProperties(ImmutableDictionary<string, Property> properties);

    /// <summary>
    /// Provides a new <c>BasePropertyDictionary</c>, with the appropriate compile-time
    /// type, that is derived by calling the specified <paramref name="provider" /> on
    /// the <see cref="Property">properties</see> associated with this
    /// <c>BasePropertyDictionary</c>.
    /// </summary>
    /// <param name="provider">
    /// A function from a collection of <see cref="Property">properties</see>, indexed
    /// by their unique identifiers, to a concrete <c>BasePropertyDictionary</c> type.
    /// </param>
    /// <returns>A new <c>BasePropertyDictionary</c></returns>
    protected T ApplyToProperties(Func<ImmutableDictionary<string, Property>, T> provider)
    {
        return provider(_properties);
    }
}
