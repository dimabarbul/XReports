using System;
using Reports.Interfaces;

namespace Reports.ValueProviders
{
    public class CallbackComputedValueProvider<TSourceEntity, TValue> : IComputedValueProvider<TSourceEntity, TValue>
    {
        private readonly Func<TSourceEntity, TValue> callback;

        public CallbackComputedValueProvider(Func<TSourceEntity, TValue> callback)
        {
            this.callback = callback;
        }

        public TValue GetValue(TSourceEntity entity)
        {
            return this.callback(entity);
        }
    }
}
