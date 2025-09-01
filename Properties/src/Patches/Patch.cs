namespace Properties.Patches;

/// <summary>
/// The <c>Patch</c> class represents an accounting of differences between two
/// <see cref="IPropertyDictionary{T}" />s.
/// </summary>
public sealed class Patch
{
    private readonly IList<IChange> _changes;

    internal Patch(IEnumerable<IChange> changes)
    {
        _changes = changes.ToList();
    }

    internal IEnumerable<IChange> Changes => _changes;

    /// <summary>
    /// Provides the <see cref="IPropertyDictionary{T}" /> induced by making the
    /// changes defined by this <c>Patch</c> to the specified
    /// <paramref name="dictionary" />.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="Type" /> of <see cref="IPropertyDictionary{T}" /> that the
    /// specified <paramref name="dictionary" /> belongs to.
    /// </typeparam>
    /// <param name="dictionary">
    /// The <see cref="IPropertyDictionary{T}" /> to apply this <c>Patch</c> to.
    /// </param>
    /// <returns>A new <see cref="IPropertyDictionary{T}" />.</returns>
    public T Apply<T>(T dictionary) where T : IPropertyDictionary<T>
    {
        return _changes.Aggregate(dictionary, (dictionary, change) => change.Apply(dictionary));
    }
}
