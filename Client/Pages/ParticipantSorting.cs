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

public class IntegerSwitchableComparer : ObjectSwitchableComparer, ISwitchableComparer<int>
{   
    
    public int Compare(int x, int y)
    {
        return x.CompareTo(y) * GetDirectionInt();
    }

    // This is kind of workaround because I couldn't have one collection of participant sorters witch comparers for different types 
    public override int Compare(object? x, object? y)
    {
        return Compare((int)x!,(int)y!);
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

public class ColumnSortingManager
{
    public ParticipantSorter<object>[] ParticipantSorters { get; private set; }

    public string[] HeaderArrows { get; private set; }

    private int activeColumnIndex;

    public ColumnSortingManager(ParticipantSorter<object>[] participantSorters, int initialActiveColumnIndex)
    {
        ParticipantSorters = participantSorters;
        activeColumnIndex = initialActiveColumnIndex;
        HeaderArrows = new string[ParticipantSorters.Length];
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

    public Func<ParticipantDto,object> GetActiveSorterKeySelector()
    {
        return ParticipantSorters[activeColumnIndex].KeySelector;
    }

    public ISwitchableComparer<object> GetActiveSorterKeyComparer()
    {
        return ParticipantSorters[activeColumnIndex].KeyComparer;
    }

    private void AdjustSorters(int clickedColumn)
    {
        if (clickedColumn == activeColumnIndex)
        {
            ParticipantSorters[clickedColumn].KeyComparer.ReverseSort = !ParticipantSorters[clickedColumn].KeyComparer.ReverseSort;
        }
        else
        {
            ParticipantSorters[clickedColumn].KeyComparer.ReverseSort = false;
        }
    }

    private void AdjustHeaderArrows(int clickedColumn)
    {
        if (clickedColumn == activeColumnIndex)
        {
            HeaderArrows[activeColumnIndex] = (HeaderArrows[activeColumnIndex] == "▲") ? "▼" : "▲";
        }
        else
        {
            HeaderArrows[activeColumnIndex] = "";
            HeaderArrows[clickedColumn] = "▲";
        }
    }
}
