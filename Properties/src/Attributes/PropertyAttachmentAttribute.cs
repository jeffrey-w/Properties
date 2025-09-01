using Extra.Guard;

namespace Properties.Attributes;

/// <summary>
/// The <c>PropertyAttachmentAttribute</c> annotates classes with information
/// regarding the <see cref="Property">properties</see> that they associate with
/// objects they instantiate.
/// </summary>
/// <param names="type">
/// The <see cref="Type" /> of <see cref="IPropertyDictionary{T}" /> that the class
/// annotated by the new <c>PropertyAttachmentAttribute</c> instantiates.
/// </param>
/// <param names="names">
/// The unique identifiers for the <see cref="Property">properties</see> that the
/// class annotated by the new <c>PropertyAttachmentAttribute</c> associates with
/// the objects it instantiates.
/// </param>
/// <exception cref="ArgumentException">
/// If the specified <paramref name="type" /> does not implement the
/// <see cref="IPropertyDictionary{T}" /> interface.
/// </exception>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PropertyAttachmentAttribute(Type type, params string[] names) : Attribute
{
    /// <summary>
    /// The <see cref="Type" /> of <see cref="IPropertyDictionary{T}" /> that the class
    /// annotated by this <c>PropertyAttachmentAttribute</c> instantiates.
    /// </summary>
    public Type Type { get; } = Against.InvalidType(type, typeof(IPropertyDictionary<>), nameof(type));

    /// <summary>
    /// The unique identifiers for the <see cref="Property">properties</see> that the
    /// class annotated by this <c>PropertyAttachmentAttribute</c> associates with the
    /// objects it instantiates.
    /// </summary>
    public IEnumerable<string> Names => names;
}
