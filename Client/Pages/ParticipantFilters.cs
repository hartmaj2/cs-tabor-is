using Shared;

public interface IQuickGridFilter
{
    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered);
}

public class TextFilter : IQuickGridFilter
{
    public string? FilterText { get ; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        if (string.IsNullOrWhiteSpace(FilterText)) return unfiltered;
        return unfiltered.Where(p => p.LastName!.Contains(FilterText,StringComparison.CurrentCultureIgnoreCase));
    }
}

public class IntegerBoundFilter : IQuickGridFilter
{
    public int Min { get; set; }
    public int Max { get; set; }

    public IQueryable<Participant> GetFiltered(IQueryable<Participant> unfiltered)
    {
        return unfiltered.Where(p => p.Age >= Min && p.Age <= Max);
    }
}