using Extra.Guard;

namespace Properties.Attributes;

/// <summary>
/// The <c>PropertyUsageAttribute</c> annotates classes with information regarding
/// the <see cref="Property">properties</see> queried by the methods declared on
/// them.
/// </summary>
/// <param name="type">
/// The <see cref="Type" /> of the <see cref="IPropertyDictionary{T}" /> queried by
/// the class annotated by the new <c>PropertyUsageAttribute</c>.
/// </param>
/// <param name="names">
/// The unique identifiers for the <see cref="Property">properties</see> queried by
/// the class annotated by the new <c>PropertyUsageAttribute</c>.
/// </param>
/// <exception cref="ArgumentException">
/// If the specified <paramref name="type" /> does not implement the
/// <see cref="IPropertyDictionary{T}" /> interface.
/// </exception>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class PropertyUsageAttribute(Type type, params string[] names) : Attribute
{
    /// <summary>
    /// The <see cref="Type" /> of the <see cref="IPropertyDictionary{T}" /> queried by
    /// the class annotated by this <c>PropertyUsageDictionary</c>.
    /// </summary>
    public Type Type { get; } = Against.InvalidType(type, typeof(IPropertyDictionary<>), nameof(type));

    /// <summary>
    /// The unique identifiers for the <see cref="Property">properties</see> queried by
    /// the class annotated by this <c>PropertyUsageAttribute</c>.
    /// </summary>
    public IEnumerable<string> Names => names;
}
