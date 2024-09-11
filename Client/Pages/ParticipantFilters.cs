using Shared;

// This interface serves as a base for filters that I would like to apply to my participants table
public interface IParticipantFilter
{
    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered);
}

public class TextFilter : IParticipantFilter
{
    public string? FilterText { get ; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => p.LastName!.Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }
}

public class IntegerBoundFilter : IParticipantFilter
{
    public int Min { get; set; }
    public int Max { get; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        return unfiltered.Where(p => p.Age >= Min && p.Age <= Max);
    }
}