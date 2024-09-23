using System.Numerics;

// This file includes classes that are related to filtering participants in tables

// Manages filtering of a table of participants
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

// Objects, whose state can be reset
public interface IResetable
{
    public void Reset();
}

// Serves as a base for all filters applied to my table, I want all of them to be able to be reset

public interface IParticipantFilter : IResetable
{
    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered);
}

// Used to filter participants based on textual values like name, phone number (as string) or birth number (also as string)
public class TextFilter : IParticipantFilter
{

    // Property to be bound to the input text field for the respective column
    public string FilterText { get ; set; } = string.Empty;

    // The selector function that selects the key of participant by which we want to filter (eg. p => p.FirstName)
    public required Func<ParticipantDto,string> FilterKeySelector { private get; init;}

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

// Used to filter numeric values like age etc.
public class NumericBoundFilter<TNumber> : IParticipantFilter where TNumber : INumber<TNumber>
{

    // We need to remember the limits so we can properly reset the filter
    public TNumber MinLimit { get; init; }
    public TNumber MaxLimit { get; init; }

    public TNumber CurrentMin { get; set; }
    public TNumber CurrentMax { get; set; }

    public NumericBoundFilter(TNumber minLimit, TNumber maxLimit)
    {   
        MinLimit = minLimit;
        CurrentMin = minLimit;
        MaxLimit = maxLimit;
        CurrentMax = maxLimit;
    }

    // The selector function that selects the numeric key of participant by which we want to filter
    public required Func<ParticipantDto,TNumber> FilterKeySelector { private get; init;}

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

// Used to filter participants based on chosen diets (in Diets subsection)
// Only participants that have all of the selected diets pass through the filter
public class DietsFilter : IParticipantFilter
{
    public required IList<AllergenSelection> DietSelections { get; init; }

    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        // fold that starts with only the selected diet choices and goes through them and returns only the participants with a diet names list containing the name of the current selection
        return DietSelections
            .Where(selection => selection.IsSelected) // start with only the selected checkboxes
            .Aggregate(
                unfiltered,
                (accumulatedParticipants,currentSelection) => accumulatedParticipants.Where(participant => participant.Diets.Select(diet => diet.Name).Contains(currentSelection.Name))
            );
    }

    public void Reset()
    {
        foreach (var selection in DietSelections) selection.IsSelected = false;
    }
}

