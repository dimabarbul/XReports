namespace XReports.SchemaBuilder
{
    public interface IValueProvider<out TValue>
    {
        TValue GetValue();
    }
}
