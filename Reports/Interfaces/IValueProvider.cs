namespace Reports.Interfaces
{
    public interface IValueProvider<TValue>
    {
        TValue GetValue();
    }
}
