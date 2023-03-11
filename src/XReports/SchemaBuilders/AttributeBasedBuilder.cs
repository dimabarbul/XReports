using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.Schema;
using XReports.SchemaBuilder;
using XReports.SchemaBuilder.ReportCellsProviders;

namespace XReports.SchemaBuilders
{
    public class AttributeBasedBuilder : IAttributeBasedBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IAttributeHandler[] handlers;

        public AttributeBasedBuilder(IEnumerable<IAttributeHandler> handlers)
        {
            this.serviceProvider = null;
            this.handlers = handlers.ToArray();
        }

        public AttributeBasedBuilder(IServiceProvider serviceProvider, IEnumerable<IAttributeHandler> handlers)
        {
            this.serviceProvider = serviceProvider;
            this.handlers = handlers.ToArray();
        }

        public IReportSchema<TEntity> BuildSchema<TEntity>()
        {
            ReportAttribute reportAttribute = typeof(TEntity).GetCustomAttribute<ReportAttribute>();

            BuildOptions options = new BuildOptions()
            {
                IsVertical = !(reportAttribute is HorizontalReportAttribute),
                HeaderRowsCount = this.GetHeaderRowsCount<TEntity>(),
            };

            IReportSchemaBuilder<TEntity> schemaBuilder = this.CreateSchemaBuilder<TEntity>(reportAttribute, options);

            return options.IsVertical ?
                (IReportSchema<TEntity>)schemaBuilder.BuildVerticalSchema() :
                schemaBuilder.BuildHorizontalSchema(options.HeaderRowsCount);
        }

        public IReportSchema<TEntity> BuildSchema<TEntity, TBuildParameter>(TBuildParameter parameter)
        {
            ReportAttribute reportAttribute = typeof(TEntity).GetCustomAttribute<ReportAttribute>();

            BuildOptions options = new BuildOptions()
            {
                IsVertical = !(reportAttribute is HorizontalReportAttribute),
                HeaderRowsCount = this.GetHeaderRowsCount<TEntity>(),
            };

            IReportSchemaBuilder<TEntity> schemaBuilder = this.CreateSchemaBuilder<TEntity, TBuildParameter>(reportAttribute, parameter, options);

            return options.IsVertical ?
                (IReportSchema<TEntity>)schemaBuilder.BuildVerticalSchema() :
                schemaBuilder.BuildHorizontalSchema(options.HeaderRowsCount);
        }

        private int GetHeaderRowsCount<TEntity>()
        {
            return typeof(TEntity)
                .GetProperties()
                .Count(p => p.GetCustomAttribute<HeaderRowAttribute>() != null);
        }

        private IReportCellsProvider<TEntity> CreateCellsProvider<TEntity>(PropertyInfo property)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity));
            MemberExpression memberExpression = Expression.Property(parameter, typeof(TEntity), property.Name);
            LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameter);

            IReportCellsProvider<TEntity> cellsProvider = (IReportCellsProvider<TEntity>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(TEntity), property.PropertyType),
                lambdaExpression.Compile());
            return cellsProvider;
        }

        private IReportSchemaBuilder<TEntity> CreateSchemaBuilder<TEntity>(ReportAttribute reportAttribute, BuildOptions options)
        {
            IReportSchemaBuilder<TEntity> builder = this.CreateSchemaBuilderNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder != null)
            {
                this.ExecutePostBuilder<IReportPostBuilder<TEntity>>(
                    reportAttribute.PostBuilder, builder, options);
            }

            return builder;
        }

        private IReportSchemaBuilder<TEntity> CreateSchemaBuilder<TEntity, TBuildParameter>(ReportAttribute reportAttribute, TBuildParameter parameter, BuildOptions options)
        {
            IReportSchemaBuilder<TEntity> builder = this.CreateSchemaBuilderNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder == null)
            {
                throw new InvalidOperationException($"Type {typeof(TEntity)} does not have post-builder.");
            }

            this.ExecutePostBuilder<IReportPostBuilder<TEntity, TBuildParameter>>(
                reportAttribute.PostBuilder, builder, parameter, options);

            return builder;
        }

        private IReportSchemaBuilder<TEntity> CreateSchemaBuilderNoPostBuild<TEntity>()
        {
            IReportSchemaBuilder<TEntity> builder = new ReportSchemaBuilder<TEntity>();
            Attribute[] globalAttributes = this.GetGlobalAttributes<TEntity>();
            PropertyAttribute[] properties = this.GetReportColumns<TEntity>();

            this.AddHeaderRows(builder, this.GetHeaderRowProperties<TEntity>());
            this.AddColumns(builder, properties, globalAttributes);
            this.AddComplexHeader(builder, properties);

            this.ProcessTablePropertyAttributes<TEntity>(builder);

            return builder;
        }

        private void AddHeaderRows<TEntity>(IReportSchemaBuilder<TEntity> builder, PropertyAttribute[] propertyAttributes)
        {
            foreach (PropertyAttribute propertyAttribute in propertyAttributes)
            {
                this.AddHeaderRow(builder, propertyAttribute);
            }
        }

        private void AddHeaderRow<TEntity>(IReportSchemaBuilder<TEntity> builder, PropertyAttribute propertyAttribute)
        {
            IReportCellsProvider<TEntity> cellsProvider = this.CreateCellsProvider<TEntity>(propertyAttribute.Property);

            IReportColumnBuilder<TEntity> cellsProviderBuilder = builder.AddColumn(propertyAttribute.Attribute.Title, cellsProvider);

            this.ApplyAttributes(builder, cellsProviderBuilder, propertyAttribute.Property, Array.Empty<Attribute>());
        }

        private void AddColumns<TEntity>(IReportSchemaBuilder<TEntity> builder, PropertyAttribute[] properties, Attribute[] globalAttributes)
        {
            foreach (PropertyAttribute x in properties)
            {
                this.AddColumn(builder, x.Property, x.Attribute, globalAttributes);
            }
        }

        private void AddComplexHeader<TEntity>(IReportSchemaBuilder<TEntity> builder, PropertyAttribute[] properties)
        {
            IEnumerable<ComplexHeaderAttribute> complexHeaderAttributes = typeof(TEntity).GetCustomAttributes<ComplexHeaderAttribute>();
            Dictionary<int, int> normalizedIndexes = properties
                .OrderBy(p => p.Attribute.Order)
                .Select((p, i) => new { Index = i, p.Attribute.Order })
                .ToDictionary(x => x.Order, x => x.Index);

            foreach (ComplexHeaderAttribute attribute in complexHeaderAttributes)
            {
                if (attribute.UseIndexes)
                {
                    int startIndex = normalizedIndexes.ContainsKey(attribute.StartIndex) ?
                        normalizedIndexes[attribute.StartIndex] :
                        throw new ArgumentException($"Start index {attribute.StartIndex} does not exist for type {typeof(TEntity)}");
                    int? endIndex = attribute.EndIndex == null ?
                        (int?)null :
                        normalizedIndexes.ContainsKey(attribute.EndIndex.Value) ?
                            normalizedIndexes[attribute.EndIndex.Value] :
                            throw new ArgumentException($"End index {attribute.EndIndex} does not exist for type {typeof(TEntity)}");

                    builder.AddComplexHeader(
                        attribute.RowIndex,
                        attribute.RowSpan,
                        attribute.Title,
                        startIndex,
                        endIndex);
                }
                else if (attribute.UseId)
                {
                    builder.AddComplexHeader(
                        attribute.RowIndex,
                        attribute.RowSpan,
                        attribute.Title,
                        new ColumnId(attribute.StartTitle),
                        attribute.EndTitle == null ?
                            null :
                            new ColumnId(attribute.EndTitle));
                }
                else
                {
                    builder.AddComplexHeader(
                        attribute.RowIndex,
                        attribute.RowSpan,
                        attribute.Title,
                        attribute.StartTitle,
                        attribute.EndTitle);
                }
            }
        }

        private void ProcessTablePropertyAttributes<TEntity>(IReportSchemaBuilder<TEntity> builder)
        {
            IEnumerable<TableAttribute> attributes = typeof(TEntity)
                .GetCustomAttributes<TableAttribute>();
            foreach (TableAttribute attribute in attributes)
            {
                foreach (IAttributeHandler handler in this.handlers)
                {
                    handler.Handle(builder, null, attribute);
                }
            }
        }

        private void AddColumn<TEntity>(IReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportColumnAttribute attribute, Attribute[] globalAttributes)
        {
            IReportCellsProvider<TEntity> cellsProvider = this.CreateCellsProvider<TEntity>(property);

            IReportColumnBuilder<TEntity> cellsProviderBuilder = builder.AddColumn(new ColumnId(property.Name), attribute.Title, cellsProvider);

            this.ApplyAttributes(builder, cellsProviderBuilder, property, globalAttributes);
        }

        private void ApplyAttributes<TEntity>(
            IReportSchemaBuilder<TEntity> builder,
            IReportColumnBuilder<TEntity> cellsProviderBuilder,
            PropertyInfo property,
            Attribute[] globalAttributes)
        {
            Attribute[] propertyAttributes = property.GetCustomAttributes().ToArray();
            Attribute[] attributes = this.MergeAndFilterAttributes(propertyAttributes, globalAttributes);

            foreach (Attribute attribute in attributes)
            {
                foreach (IAttributeHandler handler in this.handlers)
                {
                    handler.Handle(builder, cellsProviderBuilder, attribute);
                }
            }
        }

        private Attribute[] MergeAndFilterAttributes(Attribute[] propertyAttributes, Attribute[] globalAttributes)
        {
            return propertyAttributes
                .Where(a => !(a is ReportColumnAttribute) && !(a is HeaderRowAttribute))
                .Concat(globalAttributes
                    .Where(a => !(a is TableAttribute) && !(a is ReportAttribute))
                    .Where(a =>
                        (
                            !(a is BasePropertyAttribute)
                            && propertyAttributes.All(pa => pa.GetType() != a.GetType()))
                        || (
                            a is BasePropertyAttribute
                            && propertyAttributes.All(pa =>
                                pa.GetType() != a.GetType()
                                || ((BasePropertyAttribute)pa).IsHeader != ((BasePropertyAttribute)a).IsHeader))))
                .ToArray();
        }

        private PropertyAttribute[] GetReportColumns<TEntity>()
        {
            PropertyAttribute[] reportVariables = typeof(TEntity).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ReportColumnAttribute>(),
                    HeaderRowAttribute = p.GetCustomAttribute<HeaderRowAttribute>(),
                })
                .Where(x => x.Attribute != null && x.HeaderRowAttribute == null)
                .Select(x => new PropertyAttribute()
                {
                    Property = x.Property,
                    Attribute = x.Attribute,
                })
                .OrderBy(x => x.Attribute.Order)
                .ToArray();

            if (reportVariables.Length == 0)
            {
                throw new ArgumentException($"There are no report variables in {typeof(TEntity)}");
            }

            if (reportVariables.Select(v => v.Attribute.Order).Distinct().Count() != reportVariables.Length)
            {
                throw new ArgumentException("Order of report variables should be unique");
            }

            return reportVariables;
        }

        private PropertyAttribute[] GetHeaderRowProperties<TEntity>()
        {
            PropertyAttribute[] properties = typeof(TEntity).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ReportColumnAttribute>(),
                    HeaderRowAttribute = p.GetCustomAttribute<HeaderRowAttribute>(),
                })
                .Where(x => x.Attribute != null && x.HeaderRowAttribute != null)
                .Select(x => new PropertyAttribute()
                {
                    Property = x.Property,
                    Attribute = x.Attribute,
                })
                .OrderBy(x => x.Attribute.Order)
                .ToArray();

            if (properties.Select(v => v.Attribute.Order).Distinct().Count() != properties.Length)
            {
                throw new ArgumentException("Order of header rows should be unique");
            }

            return properties;
        }

        private Attribute[] GetGlobalAttributes<TEntity>()
        {
            return typeof(TEntity).GetCustomAttributes()
                .Where(a => !(a is ReportAttribute))
                .ToArray();
        }

        private void ExecutePostBuilder<TPostBuilderType>(Type postBuilderType, params object[] arguments)
        {
            if (!typeof(TPostBuilderType).IsAssignableFrom(postBuilderType))
            {
                throw new ArgumentException($"Type {postBuilderType} should implement {typeof(TPostBuilderType)}");
            }

            MethodInfo buildMethod = typeof(TPostBuilderType).GetMethod(
                "Build", arguments.Select(a => a.GetType()).ToArray());
            if (buildMethod == null)
            {
                throw new ArgumentException($"Cannot find method \"Build\" in type {typeof(TPostBuilderType)}");
            }

            (TPostBuilderType postBuilder, bool shouldDispose) = this.CreatePostBuilder<TPostBuilderType>(postBuilderType);

            buildMethod.Invoke(postBuilder, arguments);

            if (shouldDispose)
            {
                this.DisposePostBuilder(postBuilder);
            }
        }

        private (TPostBuilderType postBuilder, bool shouldDispose) CreatePostBuilder<TPostBuilderType>(Type postBuilderType)
        {
            bool shouldDispose;
            TPostBuilderType postBuilder;

            if (this.serviceProvider == null)
            {
                postBuilder = (TPostBuilderType)Activator.CreateInstance(postBuilderType);
                shouldDispose = true;
            }
            else if ((postBuilder = (TPostBuilderType)this.serviceProvider.GetService(postBuilderType)) != null)
            {
                shouldDispose = false;
            }
            else
            {
                postBuilder = (TPostBuilderType)ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, postBuilderType);
                shouldDispose = true;
            }

            return (postBuilder, shouldDispose);
        }

        private void DisposePostBuilder<TPostBuilderType>(TPostBuilderType postBuilder)
        {
            if (postBuilder is IAsyncDisposable && !(postBuilder is IDisposable))
            {
                throw new ArgumentException($"Post builder implements {typeof(IAsyncDisposable)}, but not {typeof(IDisposable)}. Please, implement {typeof(IDisposable)}.");
            }

            (postBuilder as IDisposable)?.Dispose();
        }

        private class PropertyAttribute
        {
            public PropertyInfo Property { get; set; }

            public ReportColumnAttribute Attribute { get; set; }
        }
    }
}
