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
        private readonly List<Type> baseTypes = new List<Type>();
        private readonly List<(Assembly, Type)> assemblies = new List<(Assembly, Type)>();

        public IReadOnlyCollection<Type> Types => this.LoadTypes();

        public ReportConverterOptions<TReportCell> AddHandlers(params Type[] types)
        {
            foreach (Type type in types)
            {
                this.ValidateHandlerType(type);
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
            this.ValidateBaseType(handlersInterface);

            this.assemblies.Add((assembly, handlersInterface));

            return this;
        }

        public ReportConverterOptions<TReportCell> AddHandlersByBaseType<TPropertyHandler>()
        {
            return this.AddHandlersByBaseType(typeof(TPropertyHandler));
        }

        public ReportConverterOptions<TReportCell> AddHandlersByBaseType(Type type)
        {
            this.ValidateBaseType(type);

            this.baseTypes.Add(type);

            return this;
        }

        private Type[] LoadTypes()
        {
            return this.types
                .Concat(this.baseTypes.SelectMany(this.GetImplementingTypes))
                .Concat(this.assemblies.SelectMany(this.GetHandlersInAssembly))
                .Distinct()
                .ToArray();
        }

        private IEnumerable<Type> GetImplementingTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(this.IsHandlerTypeValid)
                .Where(interfaceType.IsAssignableFrom)
                .ToArray();
        }

        private IEnumerable<Type> GetHandlersInAssembly((Assembly, Type) assemblyLoadInfo)
        {
            return assemblyLoadInfo.Item1.GetTypes()
                .Where(this.IsHandlerTypeValid)
                .Where(assemblyLoadInfo.Item2.IsAssignableFrom);
        }

        private bool IsHandlerTypeValid(Type t)
        {
            return t.IsClass && !t.IsAbstract && typeof(IPropertyHandler<TReportCell>).IsAssignableFrom(t);
        }

        private bool IsBaseTypeValid(Type t)
        {
            return typeof(IPropertyHandler<TReportCell>).IsAssignableFrom(t);
        }

        private void ValidateHandlerType(Type type)
        {
            if (!this.IsHandlerTypeValid(type))
            {
                throw new ArgumentException(
                    $"Type {type} is invalid. It should be non-abstract class that implements {typeof(IPropertyHandler<TReportCell>)}.",
                    nameof(type));
            }
        }

        private void ValidateBaseType(Type handlersInterface)
        {
            if (!this.IsBaseTypeValid(handlersInterface))
            {
                throw new ArgumentException(
                    $"{handlersInterface} should be assignable to {typeof(IPropertyHandler<TReportCell>)}",
                    nameof(handlersInterface));
            }
        }
    }
}
