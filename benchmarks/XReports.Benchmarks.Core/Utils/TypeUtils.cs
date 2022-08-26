using System.Reflection;

namespace XReports.Benchmarks.Core.Utils;

public static class TypeUtils
{
    public static object InvokeGenericMethod(object o, string methodName, Type genericType, params object[] args)
    {
        MethodInfo methodInfo = o.GetType().GetMethod(methodName)
            ?? throw new ArgumentException($"Object of type {o.GetType()} does not have method {methodName}", nameof(methodName));

        return methodInfo.MakeGenericMethod(genericType).Invoke(o, args);
    }
}
