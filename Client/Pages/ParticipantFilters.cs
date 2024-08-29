using Shared;

public interface IQuickGridFilter
{
    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered);
}

public class LastNameFilter : IQuickGridFilter
{
    public string? FilterText { get ; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => p.LastName!.Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }
}

public class AgeBoundFilter : IQuickGridFilter
{
    public int Min { get; set; } = ParticipantFormData.LowestAge;
    public int Max { get; set; } = ParticipantFormData.HighestAge;

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        return unfiltered.Where(p => p.Age >= Min && p.Age <= Max);
    }
}