using Shared;

// This interface serves as a base for filters that I would like to apply to my participants table
public interface IQueryableParticipantFilter
{
    public IQueryable<ParticipantDto> GetFiltered(IQueryable<ParticipantDto> unfiltered);
}

// The filtering is done by binding the filtering input field to the FilterText field of this filter
public class QueryableTextFilter : IQueryableParticipantFilter
{
    public string? FilterText { get ; set; }

    public IQueryable<ParticipantDto> GetFiltered(IQueryable<ParticipantDto> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => p.LastName!.Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }
}

public class QueryableIntegerBoundFilter : IQueryableParticipantFilter
{
    public int Min { get; set; }
    public int Max { get; set; }

    public IQueryable<ParticipantDto> GetFiltered(IQueryable<ParticipantDto> unfiltered)
    {
        return unfiltered.Where(p => p.Age >= Min && p.Age <= Max);
    }
}