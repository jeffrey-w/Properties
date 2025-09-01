namespace Properties.Patches;

/// <summary>
/// The <c>Diff</c> class provides a means of inferring the changes required to
/// transform one <see cref="IPropertyDictionary{T}" /> to another.
/// </summary>
public static class Diff
{
    /// <summary>
    /// Provides the <see cref="Patch" /> that defines the transformation from the
    /// <paramref name="first" /> collection of <see cref="Property">properties</see>
    /// to the <paramref name="second" />.
    /// </summary>
    /// <param name="first">
    /// The source collection of <see cref="Property">properties</see>.
    /// </param>
    /// <param name="second">
    /// The target collection of <see cref="Property">properties</see>.
    /// </param>
    /// <returns>
    /// A <see cref="Patch" /> to obtain the <paramref name="second" /> collection of
    /// <see cref="Property">properties</see> from the <paramref name="first" />.
    /// </returns>
    public static Patch ComputeFor(IEnumerable<Property> first, IEnumerable<Property> second)
    {
        var x = first.ToList();
        var y = second.ToList();
        return new Patch(
            x
               .Except(y)
               .Select(Change.Addition)
               .Concat(
                    y
                       .Except(x)
                       .Select(Change.Deletion)));
    }
}
