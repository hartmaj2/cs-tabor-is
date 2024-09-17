public class MealComparer : IComparer<MealDto>
{
    public static MealComparer Instance { get; } = new MealComparer();

    public int Compare(MealDto? x, MealDto? y)
    {
        if (x!.Type != y!.Type) return x.Type.CompareTo(y.Type);
        return x.Name.CompareTo(y.Name);
    }
}