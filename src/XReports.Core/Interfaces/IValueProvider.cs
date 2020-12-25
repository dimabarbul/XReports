namespace XReports.Interfaces
{
    public interface IValueProvider<out TValue>
    {
        TValue GetValue();
    }
}
