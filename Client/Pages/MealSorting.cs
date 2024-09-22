// Comparer used to sort meals based on meal type (soup,main) first and then by name
public class MealComparer : IComparer<MealDto>
{

    // Used so I don't have to create a new instance each time I am sorting inside some component
    // I can do this because the MealComparer instances don't have any internal state
    public static MealComparer Instance { get; } = new MealComparer();

    public int Compare(MealDto? x, MealDto? y)
    {
        if (x!.Type != y!.Type) return x.Type.CompareTo(y.Type);
        return x.Name.CompareTo(y.Name);
    }
}