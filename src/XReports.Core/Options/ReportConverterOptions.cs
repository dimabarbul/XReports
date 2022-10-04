using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Options
{
    public class ReportConverterOptions<TReportCell>
        where TReportCell : BaseReportCell
    {
        private readonly List<Type> types = new List<Type>();
        private readonly List<Type> interfaces = new List<Type>();
        private readonly List<(Assembly, Type)> assemblies = new List<(Assembly, Type)>();

        public IReadOnlyCollection<Type> Types => this.LoadTypes();

        public ReportConverterOptions<TReportCell> AddHandlers(params Type[] types)
        {
            foreach (Type type in types)
            {
                if (!this.IsTypeValid(type))
                {
                    throw new ArgumentException($"Type {type} is invalid. It should be non-abstract class that implements {typeof(IPropertyHandler<TReportCell>)}.", nameof(types));
                }
            }

            this.types.AddRange(types);

            return this;
        }

        public ReportConverterOptions<TReportCell> AddHandlersFromAssembly(Assembly assembly)
        {
            return this.AddHandlersFromAssembly(assembly, typeof(IPropertyHandler<TReportCell>));
        }

        public ReportConverterOptions<TReportCell> AddHandlersFromAssembly<TPropertyHandler>(Assembly assembly)
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return this.AddHandlersFromAssembly(assembly, typeof(TPropertyHandler));
        }

        public ReportConverterOptions<TReportCell> AddHandlersFromAssembly(Assembly assembly, Type handlersInterface)
        {
            if (!typeof(IPropertyHandler<TReportCell>).IsAssignableFrom(handlersInterface))
            {
                throw new ArgumentException($"{handlersInterface} should be assignable to {typeof(IPropertyHandler<TReportCell>)}", nameof(handlersInterface));
            }

            this.assemblies.Add((assembly, handlersInterface));

            return this;
        }

        public ReportConverterOptions<TReportCell> AddHandlersByInterface<TPropertyHandler>()
        {
            return this.AddHandlersByInterface(typeof(TPropertyHandler));
        }

        public ReportConverterOptions<TReportCell> AddHandlersByInterface(Type type)
        {
            this.interfaces.Add(type);

            return this;
        }

        private Type[] LoadTypes()
        {
            return this.types
                .Concat(this.interfaces.SelectMany(this.GetImplementingTypes))
                .Concat(this.assemblies.SelectMany(this.GetHandlersInAssembly))
                .Distinct()
                .ToArray();
        }

        private IEnumerable<Type> GetImplementingTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(this.IsTypeValid)
                .Where(interfaceType.IsAssignableFrom)
                .ToArray();
        }

        private IEnumerable<Type> GetHandlersInAssembly((Assembly, Type) assemblyLoadInfo)
        {
            return assemblyLoadInfo.Item1.GetTypes()
                .Where(this.IsTypeValid)
                .Where(assemblyLoadInfo.Item2.IsAssignableFrom);
        }

        private bool IsTypeValid(Type t)
        {
            return t.IsClass && !t.IsAbstract && typeof(IPropertyHandler<TReportCell>).IsAssignableFrom(t);
        }
    }
}
