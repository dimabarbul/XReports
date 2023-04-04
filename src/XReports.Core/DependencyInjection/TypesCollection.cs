using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Collection of non-abstract class types with the same base type.
    /// </summary>
    /// <typeparam name="TBaseType">Base type of types in the collection.</typeparam>
    public class TypesCollection<TBaseType> : IEnumerable<Type>
    {
        private readonly List<Type> types = new List<Type>();
        private readonly List<Type> baseTypes = new List<Type>();
        private readonly List<(Assembly, Type)> assemblies = new List<(Assembly, Type)>();
        private readonly List<Type> exclusions = new List<Type>();

        /// <summary>
        /// Gets types in the collection.
        /// </summary>
        public IReadOnlyCollection<Type> Types => this.LoadTypes();

        /// <inheritdoc />
        public IEnumerator<Type> GetEnumerator()
        {
            return this.Types.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds types to the collection.
        /// </summary>
        /// <param name="types">Types to add. Must be non-abstract classes derived from <typeparamref name="TBaseType"/>.</param>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> Add(params Type[] types)
        {
            foreach (Type type in types)
            {
                ValidateType(type);
            }

            this.types.AddRange(types);

            return this;
        }

        /// <summary>
        /// Adds all types from assembly that derive from base type.
        /// </summary>
        /// <param name="assembly">Assembly to add types from.</param>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> AddFromAssembly(Assembly assembly)
        {
            return this.AddFromAssembly(assembly, typeof(TBaseType));
        }

        /// <summary>
        /// Adds all types from assembly that derive from <typeparamref name="TCustomBaseType"/>.
        /// </summary>
        /// <param name="assembly">Assembly to add types from.</param>
        /// <typeparam name="TCustomBaseType">Base type of types to add. Must be derived from <typeparamref name="TBaseType"/>.</typeparam>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> AddFromAssembly<TCustomBaseType>(Assembly assembly)
            where TCustomBaseType : TBaseType
        {
            return this.AddFromAssembly(assembly, typeof(TCustomBaseType));
        }

        /// <summary>
        /// Adds all types from assembly that derive from <paramref name="baseType"/>.
        /// </summary>
        /// <param name="assembly">Assembly to add types from.</param>
        /// <param name="baseType">Base type of types to add. Must be derived from <typeparamref name="TBaseType"/>.</param>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> AddFromAssembly(Assembly assembly, Type baseType)
        {
            ValidateBaseType(baseType);

            this.assemblies.Add((assembly, baseType));

            return this;
        }

        /// <summary>
        /// Adds all types that derive from <typeparamref name="TCustomBaseType"/> from all assemblies loaded into current application domain.
        /// </summary>
        /// <typeparam name="TCustomBaseType">Base type of types to add. Must be derived from <typeparamref name="TBaseType"/>.</typeparam>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> AddByBaseType<TCustomBaseType>()
        {
            return this.AddByBaseType(typeof(TCustomBaseType));
        }

        /// <summary>
        /// Adds all types that derive from <paramref name="type"/> from all assemblies loaded into current application domain.
        /// </summary>
        /// <param name="type">Base type of types to add. Must be derived from <typeparamref name="TBaseType"/>.</param>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> AddByBaseType(Type type)
        {
            ValidateBaseType(type);

            this.baseTypes.Add(type);

            return this;
        }

        /// <summary>
        /// Removes type <typeparamref name="T"/> from the collection.
        /// </summary>
        /// <typeparam name="T">Type to remove. Must be non-abstract classes derived from <typeparamref name="TBaseType"/>.</typeparam>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> Remove<T>()
        {
            return this.Remove(typeof(T));
        }

        /// <summary>
        /// Remove types from the collection.
        /// </summary>
        /// <param name="types">Types to remove. Must be non-abstract classes derived from <typeparamref name="TBaseType"/>.</param>
        /// <returns>The types collection.</returns>
        public TypesCollection<TBaseType> Remove(params Type[] types)
        {
            foreach (Type type in types)
            {
                ValidateType(type);
            }

            this.exclusions.AddRange(types);

            return this;
        }

        private static bool IsTypeValid(Type type)
        {
            return type.IsClass && !type.IsAbstract && typeof(TBaseType).IsAssignableFrom(type);
        }

        private static void ValidateType(Type type)
        {
            if (!IsTypeValid(type))
            {
                throw new ArgumentException(
                    $"Type {type} is invalid. It should be non-abstract class that implements {typeof(TBaseType)}.",
                    nameof(type));
            }
        }

        private static void ValidateBaseType(Type baseType)
        {
            if (!IsBaseTypeValid(baseType))
            {
                throw new ArgumentException(
                    $"{baseType} should be assignable to {typeof(TBaseType)}",
                    nameof(baseType));
            }
        }

        private static bool IsBaseTypeValid(Type baseType)
        {
            return typeof(TBaseType).IsAssignableFrom(baseType);
        }

        private Type[] LoadTypes()
        {
            return this.types
                .Concat(this.baseTypes.SelectMany(this.GetImplementingTypes))
                .Concat(this.assemblies.SelectMany(this.GetTypesInAssembly))
                .Except(this.exclusions)
                .Distinct()
                .ToArray();
        }

        private IEnumerable<Type> GetImplementingTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(IsTypeValid)
                .Where(interfaceType.IsAssignableFrom)
                .ToArray();
        }

        private IEnumerable<Type> GetTypesInAssembly((Assembly, Type) assemblyLoadInfo)
        {
            return assemblyLoadInfo.Item1.GetTypes()
                .Where(IsTypeValid)
                .Where(assemblyLoadInfo.Item2.IsAssignableFrom);
        }
    }
}
