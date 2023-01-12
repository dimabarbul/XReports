using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using XReports.Attributes;
using XReports.Enums;
using XReports.Interfaces;
using XReports.ReportCellsProviders;

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

            return reportAttribute?.Type == ReportType.Horizontal ?
                (IReportSchema<TEntity>)this.BuildHorizontalReport<TEntity>(reportAttribute as HorizontalReportAttribute).BuildSchema() :
                this.BuildVerticalReport<TEntity>(reportAttribute as VerticalReportAttribute).BuildSchema();
        }

        public IReportSchema<TEntity> BuildSchema<TEntity, TBuildParameter>(TBuildParameter parameter)
        {
            ReportAttribute reportAttribute = typeof(TEntity).GetCustomAttribute<ReportAttribute>();

            return reportAttribute?.Type == ReportType.Horizontal ?
                (IReportSchema<TEntity>)this.BuildHorizontalReport<TEntity, TBuildParameter>(reportAttribute as HorizontalReportAttribute, parameter).BuildSchema() :
                this.BuildVerticalReport<TEntity, TBuildParameter>(reportAttribute as VerticalReportAttribute, parameter).BuildSchema();
        }

        private IHorizontalReportSchemaBuilder<TEntity> BuildHorizontalReport<TEntity>(HorizontalReportAttribute reportAttribute)
        {
            IHorizontalReportSchemaBuilder<TEntity> builder = this.BuildHorizontalReportNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder != null)
            {
                this.ExecutePostBuilder<IHorizontalReportPostBuilder<TEntity>>(
                    reportAttribute.PostBuilder, builder);
            }

            return builder;
        }

        private IHorizontalReportSchemaBuilder<TEntity> BuildHorizontalReport<TEntity, TBuildParameter>(
            HorizontalReportAttribute reportAttribute, TBuildParameter parameter)
        {
            IHorizontalReportSchemaBuilder<TEntity> builder = this.BuildHorizontalReportNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder == null)
            {
                throw new InvalidOperationException($"Type {typeof(TEntity)} does not have post-builder.");
            }

            this.ExecutePostBuilder<IHorizontalReportPostBuilder<TEntity, TBuildParameter>>(
                reportAttribute.PostBuilder, builder, parameter);

            return builder;
        }

        private IHorizontalReportSchemaBuilder<TEntity> BuildHorizontalReportNoPostBuild<TEntity>()
        {
            IHorizontalReportSchemaBuilder<TEntity> builder = new HorizontalReportSchemaBuilder<TEntity>();

            ReportVariableData[] reportVariables = this.GetProperties<TEntity>();
            Attribute[] globalAttributes = this.GetGlobalAttributes<TEntity>();

            this.AddRows(builder, reportVariables, globalAttributes);
            this.AddComplexHeader(builder, reportVariables);

            this.ProcessTablePropertyAttributes<TEntity>(builder);

            return builder;
        }

        private void AddRows<TEntity>(IHorizontalReportSchemaBuilder<TEntity> builder, ReportVariableData[] reportVariables, Attribute[] globalAttributes)
        {
            foreach (ReportVariableData x in reportVariables)
            {
                this.AddRow(builder, x.Property, x.Attribute, globalAttributes);
            }
        }

        private void AddRow<TEntity>(IHorizontalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute, Attribute[] globalAttributes)
        {
            IReportCellsProvider<TEntity> cellsProvider = this.CreateCellsProvider<TEntity>(property);

            IReportSchemaCellsProviderBuilder<TEntity> cellsProviderBuilder = builder.AddRow(attribute.Title, cellsProvider);

            this.ApplyAttributes(builder, cellsProviderBuilder, property, globalAttributes);
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

        private IVerticalReportSchemaBuilder<TEntity> BuildVerticalReport<TEntity>(VerticalReportAttribute reportAttribute)
        {
            IVerticalReportSchemaBuilder<TEntity> builder = this.BuildVerticalReportNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder != null)
            {
                this.ExecutePostBuilder<IVerticalReportPostBuilder<TEntity>>(
                    reportAttribute.PostBuilder, builder);
            }

            return builder;
        }

        private IVerticalReportSchemaBuilder<TEntity> BuildVerticalReport<TEntity, TBuildParameter>(
            VerticalReportAttribute reportAttribute, TBuildParameter parameter)
        {
            IVerticalReportSchemaBuilder<TEntity> builder = this.BuildVerticalReportNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder == null)
            {
                throw new InvalidOperationException($"Type {typeof(TEntity)} does not have post-builder.");
            }

            this.ExecutePostBuilder<IVerticalReportPostBuilder<TEntity, TBuildParameter>>(
                reportAttribute.PostBuilder, builder, parameter);

            return builder;
        }

        private IVerticalReportSchemaBuilder<TEntity> BuildVerticalReportNoPostBuild<TEntity>()
        {
            IVerticalReportSchemaBuilder<TEntity> builder = new VerticalReportSchemaBuilder<TEntity>();
            Attribute[] globalAttributes = this.GetGlobalAttributes<TEntity>();
            ReportVariableData[] properties = this.GetProperties<TEntity>();

            this.AddColumns(builder, properties, globalAttributes);
            this.AddComplexHeader(builder, properties);

            this.ProcessTablePropertyAttributes<TEntity>(builder);

            return builder;
        }

        private void AddColumns<TEntity>(IVerticalReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties, Attribute[] globalAttributes)
        {
            foreach (ReportVariableData x in properties)
            {
                this.AddColumn(builder, x.Property, x.Attribute, globalAttributes);
            }
        }

        private void AddComplexHeader<TEntity>(IReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties)
        {
            IEnumerable<ComplexHeaderAttribute> complexHeaderAttributes = typeof(TEntity).GetCustomAttributes<ComplexHeaderAttribute>();
            Dictionary<int, int> normalizedIndexes = properties
                .OrderBy(p => p.Attribute.Order)
                .Select((p, i) => new { Index = i, p.Attribute.Order })
                .ToDictionary(x => x.Order, x => x.Index);

            foreach (ComplexHeaderAttribute attribute in complexHeaderAttributes)
            {
                if (attribute.UsesIndexes)
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

        private void AddColumn<TEntity>(IVerticalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute, Attribute[] globalAttributes)
        {
            IReportCellsProvider<TEntity> cellsProvider = this.CreateCellsProvider<TEntity>(property);

            IReportSchemaCellsProviderBuilder<TEntity> cellsProviderBuilder = builder.AddColumn(attribute.Title, cellsProvider);

            this.ApplyAttributes(builder, cellsProviderBuilder, property, globalAttributes);
        }

        private void ApplyAttributes<TEntity>(
            IReportSchemaBuilder<TEntity> builder,
            IReportSchemaCellsProviderBuilder<TEntity> cellsProviderBuilder,
            PropertyInfo property,
            Attribute[] globalAttributes)
        {
            Attribute[] propertyAttributes = property.GetCustomAttributes().ToArray();
            Attribute[] attributes = this.MergeGlobalAttributes(propertyAttributes, globalAttributes);

            foreach (Attribute attribute in attributes)
            {
                foreach (IAttributeHandler handler in this.handlers)
                {
                    handler.Handle(builder, cellsProviderBuilder, attribute);
                }
            }
        }

        private Attribute[] MergeGlobalAttributes(Attribute[] propertyAttributes, Attribute[] globalAttributes)
        {
            return propertyAttributes
                .Where(a => !(a is ReportVariableAttribute))
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

        private ReportVariableData[] GetProperties<TEntity>()
        {
            ReportVariableData[] reportVariables = typeof(TEntity).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ReportVariableAttribute>(),
                })
                .Where(x => x.Attribute != null)
                .Select(x => new ReportVariableData()
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

        private class ReportVariableData
        {
            public PropertyInfo Property { get; set; }

            public ReportVariableAttribute Attribute { get; set; }
        }
    }
}
