// This file includes classes that are related to filtering participants in tables

// Serves as a base for all filters applied to my table, I want all of them to be able to be reset
public interface IParticipantFilter : IResetable
{
    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered);
}

// Objects, whose state can be reset
public interface IResetable
{
    public void Reset();
}

// Used to filter participants based on textual values like name, phone number (as string) or birth number (also as string)
public class TextFilter : IParticipantFilter
{

    // Property to be bound to the input text field for the respective column
    public string FilterText { get ; set; } = string.Empty;

    // The selector function that selects the key of participant by which we want to filter (eg. p => p.FirstName)
    public required Func<ParticipantDto,string> FilterKeySelector { get; set;}

    // Returns either the input enumerable of participants if nothing was entered or if there is something, we apply the filter
    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        FilterText = FilterText.TrimStart(); // we don't want the user to have just white spaces there but we want them to be able to enter a white space after a word
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => FilterKeySelector(p).Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }

    // Reset the filter by setting the filter text to empty string
    public void Reset()
    {
        FilterText = string.Empty;
    }
}

public class IntegerBoundFilter : IParticipantFilter
{

    public int MinLimit { get; init; }
    public int MaxLimit { get; init; }

    public int CurrentMin { get; set; }
    public int CurrentMax { get; set; }

    public IntegerBoundFilter(int minLimit, int maxLimit)
    {   
        MinLimit = minLimit;
        CurrentMin = minLimit;
        MaxLimit = maxLimit;
        CurrentMax = maxLimit;
    }

    // The selector function that selects the key of participant by which we want to filter
    public required Func<ParticipantDto,int> FilterKeySelector { get; set;}

    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        return unfiltered.Where(p => FilterKeySelector(p) >= CurrentMin && FilterKeySelector(p) <= CurrentMax);
    }

    public void Reset()
    {
        CurrentMin = MinLimit;
        CurrentMax = MaxLimit;
    }
}

public class DietsFilter : IParticipantFilter
{
    public required IList<AllergenSelection> DietSelections { get; set; }

    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        // fold that starts with only the selected diet choices and goes through them and returns only the participants with a diet names list containing the name of the current selection
        return DietSelections
            .Where(selection => selection.IsSelected) // start with only the selected checkboxes
            .Aggregate(
                unfiltered,
                (accFiltered,currentSelection) => accFiltered.Where(participant => participant.Diets.Select(diet => diet.Name).Contains(currentSelection.Name))
            );
    }

    public void Reset()
    {
        foreach (var selection in DietSelections) selection.IsSelected = false;
    }
}

public class ColumnFilteringManager
{
    // Stores all the filters to be applied to participants
    public IList<IParticipantFilter> Filters { get; private set; }

    public ColumnFilteringManager(IList<IParticipantFilter> participantFilters)
    {
        Filters = participantFilters;
    }

    // Resets all filters so we can see all participants
    public void ResetFilters()
    {
        foreach (var filter in Filters)
        {
            filter.Reset();
        }
    }

    // filter all participants by folding the get filtered function over all filters with participantsDtos as starting point
    public IEnumerable<ParticipantDto> GetFilteredParticipants(IEnumerable<ParticipantDto> unfilteredParticipants)
    {
        return Filters.Aggregate(unfilteredParticipants, (accumulatedParticipants,currentFilter) => currentFilter.GetFiltered(accumulatedParticipants));
    }
}