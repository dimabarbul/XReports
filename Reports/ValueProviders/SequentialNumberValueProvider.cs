using Reports.Interfaces;

namespace Reports.ValueProviders
{
    public class SequentialNumberValueProvider : IValueProvider
    {
        private int currentNumber;

        public SequentialNumberValueProvider(int startNumber = 1)
        {
            this.currentNumber = startNumber;
        }

        public string GetValue()
        {
            return (this.currentNumber++).ToString();
        }
    }
}
