namespace Properties.Patches;

internal static class Diff
{
    public static Patch ComputeFor(IEnumerable<Property> one, IEnumerable<Property> two)
    {
        var x = one.ToList();
        var y = two.ToList();
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
