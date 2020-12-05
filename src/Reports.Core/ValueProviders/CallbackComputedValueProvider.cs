using System;
using Reports.Core.Interfaces;

namespace Reports.Core.ValueProviders
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