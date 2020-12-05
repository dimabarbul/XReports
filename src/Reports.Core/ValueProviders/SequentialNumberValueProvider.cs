using Reports.Core.Interfaces;

namespace Reports.Core.ValueProviders
{
    public class SequentialNumberValueProvider : IValueProvider<int>
    {
        private int currentNumber;

        public SequentialNumberValueProvider(int startNumber = 1)
        {
            this.currentNumber = startNumber;
        }

        public int GetValue()
        {
            return this.currentNumber++;
        }
    }
}
