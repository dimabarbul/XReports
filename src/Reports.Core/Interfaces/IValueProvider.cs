namespace Reports.Core.Interfaces
{
    public interface IValueProvider<out TValue>
    {
        TValue GetValue();
    }
}
