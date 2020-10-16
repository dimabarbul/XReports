namespace Reports.Interfaces
{
    public interface IValueProvider<TSourceEntity>
    {
        string GetValue(TSourceEntity entity);
    }

    public interface IValueProvider
    {
        string GetValue();
    }
}
