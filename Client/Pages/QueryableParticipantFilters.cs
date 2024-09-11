using Shared;

// This interface serves as a base for filters that I would like to apply to my participants table
public interface IQueryableParticipantFilter
{
    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered);
}

// The filtering is done by binding the filtering input field to the FilterText field of this filter
public class TextFilter : IQueryableParticipantFilter
{
    public string? FilterText { get ; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => p.LastName!.Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }
}

public class IntegerBoundFilter : IQueryableParticipantFilter
{
    public int Min { get; set; }
    public int Max { get; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        return unfiltered.Where(p => p.Age >= Min && p.Age <= Max);
    }
}