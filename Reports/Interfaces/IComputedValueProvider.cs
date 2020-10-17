namespace Reports.Interfaces
{
    public interface IComputedValueProvider<TSourceEntity, TValue>
    {
        TValue GetValue(TSourceEntity entity);
    }
}
