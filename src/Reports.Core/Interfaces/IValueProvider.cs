namespace Reports.Interfaces
{
    public interface IValueProvider<out TValue>
    {
        TValue GetValue();
    }
}
