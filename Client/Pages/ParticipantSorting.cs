using System.Text;

// Abstract class det defines common behavior of all switchable comparers that can compare general objects
public abstract class ObjectSwitchableComparer : ISwitchableComparer<object>
{
    public bool ReverseSort { get; set; }

    protected int GetDirectionInt()
    {
        return ReverseSort ? -1 : 1;
    }

    public abstract int Compare(object? x, object? y);
}

public class StringSwitchableComparer : ObjectSwitchableComparer, ISwitchableComparer<string>
{   
    
    public int Compare(string? x, string? y)
    {
        return x!.CompareTo(y) * GetDirectionInt();
    }

    // This is kind of workaround because I couldn't have one collection of participant sorters witch comparers for different types 
    public override int Compare(object? x, object? y)
    {
        return Compare((string)x!,(string)y!);
    }
}

public class DietsSwitchableComparer : ObjectSwitchableComparer, ISwitchableComparer<IEnumerable<AllergenDto>>
{

    // converts the list of diets into a string and compares those strings
    public int Compare(IEnumerable<AllergenDto>? dietsX, IEnumerable<AllergenDto>? dietsY)
    {
        StringBuilder builderX = new StringBuilder();
        StringBuilder builderY = new StringBuilder();
        foreach (var name in dietsX!.Select(allergen => allergen.Name)) builderX.Append(name);
        foreach (var name in dietsY!.Select(allergen => allergen.Name)) builderY.Append(name);
        return builderX.ToString().CompareTo(builderY.ToString()) * GetDirectionInt();
    }

    // This is kind of workaround because I couldn't have one collection of participant sorters witch comparers for different types 
    public override int Compare(object? x, object? y)
    {
        return Compare((IEnumerable<AllergenDto>)x!,(IEnumerable<AllergenDto>)y!);
    }
}

public interface ISwitchableComparer<T> : IComparer<T>
{
    public bool ReverseSort { get; set; }
}

// General Participant Sorter that can return a KeySelector and a KeyComparer
public class ParticipantSorter<T>
{
    public required Func<ParticipantDto,T> KeySelector { get; set;}
    public required ISwitchableComparer<T> KeyComparer { get; set;}

}
