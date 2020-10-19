namespace Reports.Interfaces
{
    public interface IValueFormatter<in TValue>
    {
        string Format(TValue value);
    }
}
