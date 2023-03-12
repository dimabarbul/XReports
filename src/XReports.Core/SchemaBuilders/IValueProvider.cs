namespace XReports.SchemaBuilders
{
    public interface IValueProvider<out TValue>
    {
        TValue GetValue();
    }
}
