using Shared;

// This interface serves as a base for filters that I would like to apply to my participants table
public interface IParticipantFilter
{
    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered);
}

// The filtering is done by binding the filtering input field to the FilterText field of this filter
public class TextFilter : IParticipantFilter
{
    public string? FilterText { get ; set; }

    public required Func<ParticipantDto,string> FilterKeySelector { get; set;}

    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => FilterKeySelector(p).Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }
}

public class IntegerBoundFilter : IParticipantFilter
{
    public int Min { get; set; }
    public int Max { get; set; }

    public IEnumerable<ParticipantDto> GetFiltered(IEnumerable<ParticipantDto> unfiltered)
    {
        return unfiltered.Where(p => p.Age >= Min && p.Age <= Max);
    }
}