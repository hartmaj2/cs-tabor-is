using System.Text;

// Defines common behavior of all switchable comparers that can compare on given type
public interface ISwitchableComparer<T> : IComparer<T>
{
    public bool ReverseSort { get; set; }
}

// Comparers that can compare on any objects, also provides useful methods for classes that decide to inherit
public abstract class ObjectSwitchableComparer : ISwitchableComparer<object>
{

    // Tracks whether we want the comparer to compare in reversed direction
    public bool ReverseSort { get; set; }

    // Return value by which I can just multiply the sort result
    protected int GetDirectionInt()
    {
        return ReverseSort ? -1 : 1;
    }

    // Null value considered lower than some value
    protected int CompareNull(object? x, object? y)
    {
        var result = 0;
        if (x is null) result--;
        if (y is null) result++;
        return result * GetDirectionInt();
    }

    public abstract int Compare(object? x, object? y);
}

public class StringSwitchableComparer : ObjectSwitchableComparer
{   
    
    public int Compare(string? x, string? y)
    {
        if (x != null && y != null)
        {
            return x.CompareTo(y) * GetDirectionInt();
        }
        return CompareNull(x,y);
    }

    // This is kind of workaround because I couldn't have one collection of participant sorters witch comparers for different types 
    public override int Compare(object? x, object? y)
    {
        if (x is string stringX && y is string stringY)
        {
            return Compare(stringX,stringY);
        }

        // not string considered lower than string
        var result = 0;
        if (x is not string) result--;
        if (y is not string) result++;
        return result;
    }
}

public class IntegerSwitchableComparer : ObjectSwitchableComparer
{   
    
    public int Compare(int x, int y)
    {
        return x.CompareTo(y) * GetDirectionInt();
    }

    // This is kind of workaround because I couldn't have one collection of participant sorters witch comparers for different types 
    public override int Compare(object? x, object? y)
    {
        if (x is int intX && y is int intY)
        {
            return Compare(intX,intY);
        }

        // not int considered lower than int
        var result = 0;
        if (x is not int) result--;
        if (y is not int) result++;
        return result;
    }
}

public class DietsSwitchableComparer : ObjectSwitchableComparer
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



// General Participant Sorter, can return a KeySelector (what property of participant we want to use for sorting) and a KeyComparer (how do we decide on the comparison result)
// Cannot be an interface because we want the properties to be required on initialization
public class ParticipantSorter<T>
{
    public required Func<ParticipantDto,T> KeySelector { get; set;}
    public required ISwitchableComparer<T> KeyComparer { get; set;}

}

// Used by div tables to sort the columns and track the corresponding arrows that should be displayed next to the active sorting column
public class ColumnSortingManager
{
    private ParticipantSorter<object>[] _participantSorters;

    private string[] _headerArrows;

    // Getter only property that makes it impossible to assign to the backing field from the outside of this class
    public IReadOnlyList<string> HeaderArrows => Array.AsReadOnly(_headerArrows);

    // Current index by which we are sorting
    private int activeColumnIndex;

    // Constructor takes: 
    //  1. the array of all sorters (combinations of key selectors and key comparers that can reverse direction) 
    //  2. index of column which I want to have sorted on page initialization
    public ColumnSortingManager(ParticipantSorter<object>[] participantSorters, int initialActiveColumnIndex)
    {
        _participantSorters = participantSorters;
        activeColumnIndex = initialActiveColumnIndex;
        _headerArrows = new string[_participantSorters.Length];
    }

    // The sort click is handled depending if the clicked column was already active or not
    // we flip the sort direction if the column was already active and was clicked
    // otherwise we sort in unreversed direction
    // lastly we have to update our active column
    public void HandleSortClick(int clickedColumn)
    {
        AdjustSorters(clickedColumn);
        AdjustHeaderArrows(clickedColumn);
        activeColumnIndex = clickedColumn;
    }

    public IEnumerable<ParticipantDto> GetSortedParticipants(IEnumerable<ParticipantDto> unsortedParticipants)
    {
        return unsortedParticipants.OrderBy(GetActiveSorterKeySelector(),GetActiveSorterKeyComparer());
    }

    private Func<ParticipantDto,object> GetActiveSorterKeySelector()
    {
        return _participantSorters[activeColumnIndex].KeySelector;
    }

    private ISwitchableComparer<object> GetActiveSorterKeyComparer()
    {
        return _participantSorters[activeColumnIndex].KeyComparer;
    }

    // Adjust the sorter reversed flags based on if we clicked on a new column or not
    private void AdjustSorters(int clickedColumn)
    {
        if (clickedColumn == activeColumnIndex) // clicked on column that was already active
        {
            _participantSorters[clickedColumn].KeyComparer.ReverseSort = !_participantSorters[clickedColumn].KeyComparer.ReverseSort;
        }
        else // clicked on new column
        {
            _participantSorters[clickedColumn].KeyComparer.ReverseSort = false;
        }
    }

    // Adjust the markers showing if sorting ascending or descending
    private void AdjustHeaderArrows(int clickedColumn)
    {
        if (clickedColumn != activeColumnIndex) // clicked on a new column, remove the marker from the old one
        {
            _headerArrows[activeColumnIndex] = "";
        }
        _headerArrows[clickedColumn] = _participantSorters[clickedColumn].KeyComparer.ReverseSort ? "▼" : "▲"; // set current marker depending on if the sorter is reversed
    }
}
