// This interface serves as a base for filters that I would like to apply to my participants table
public interface IParticipantFilter : IResetable
{
    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered);
}

public interface IResetable
{
    public void Reset();
}

// The filtering is done by binding the filtering input field to the FilterText field of this filter
public class TextFilter : IParticipantFilter
{

    // This is the input text we use to filter the participants
    public string? FilterText { get ; set; }

    // The selector function that selects the key of participant by which we want to filter
    public required Func<ParticipantDto,string?> FilterKeySelector { get; set;}

    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => FilterKeySelector(p) != null && FilterKeySelector(p)!.Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }

    public void Reset()
    {
        FilterText = null;
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