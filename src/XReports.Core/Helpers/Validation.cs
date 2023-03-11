using System;

namespace XReports.Helpers
{
    internal static class Validation
    {
        public static void NotNegative(string parameterName, int parameter)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Parameter should not be negative");
            }
        }

        public static void Positive(string parameterName, int parameter)
        {
            if (parameter <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Parameter should be positive");
            }
        }

        public static void NotNull(string parameterName, object parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
