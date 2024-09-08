public class StringSwitchableComparer : ISwitchableComparer<string>
{   
    public bool ReverseSort { get; set; }
    
    public int Compare(string? x, string? y)
    {
        var directionInt = ReverseSort ? -1 : 1;
        return x!.CompareTo(y) * directionInt;
    }
}

public interface ISwitchableComparer<T> : IComparer<T>
{
    public bool ReverseSort { get; set; }
}

public class ParticipantSorter<T>
{
    public Func<ParticipantDto,T> KeySelector { get; set;}
    public ISwitchableComparer<T> KeyComparer { get; set;}

}
