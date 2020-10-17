namespace Reports.Interfaces
{
    public interface IValueFormatter<TValue>
    {
        string Format(TValue value);
    }
}
