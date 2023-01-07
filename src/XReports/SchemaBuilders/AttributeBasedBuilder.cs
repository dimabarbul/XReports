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

        public AttributeBasedBuilder(IServiceProvider serviceProvider, IEnumerable<IAttributeHandler> handlers = null)
        {
            this.serviceProvider = serviceProvider;
            if (handlers != null)
            {
                this.handlers = handlers.ToArray();
            }
            else
            {
                this.handlers = Array.Empty<IAttributeHandler>();
            }
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

            if (reportAttribute?.PostBuilder != null
                && typeof(IHorizontalReportPostBuilder<TEntity>).IsAssignableFrom(reportAttribute.PostBuilder))
            {
                ((IHorizontalReportPostBuilder<TEntity>)ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, reportAttribute.PostBuilder))
                    .Build(builder);
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

            if (!typeof(IHorizontalReportPostBuilder<TEntity, TBuildParameter>).IsAssignableFrom(reportAttribute.PostBuilder))
            {
                throw new InvalidOperationException($"Type {reportAttribute.PostBuilder} is not assignable to {typeof(IHorizontalReportPostBuilder<TEntity, TBuildParameter>)}.");
            }

            ((IHorizontalReportPostBuilder<TEntity, TBuildParameter>)ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, reportAttribute.PostBuilder))
                .Build(builder, parameter);

            return builder;
        }

        private IHorizontalReportSchemaBuilder<TEntity> BuildHorizontalReportNoPostBuild<TEntity>()
        {
            IHorizontalReportSchemaBuilder<TEntity> builder = new HorizontalReportSchemaBuilder<TEntity>();

            ReportVariableData[] reportVariables = this.GetProperties<TEntity>();
            Attribute[] globalAttributes = this.GetGlobalAttributes<TEntity>();

            this.AddRows(builder, reportVariables, globalAttributes);
            this.AddComplexHeader(builder, reportVariables);

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
            IReportCellsProvider<TEntity> instance = this.CreateCellsProvider<TEntity>(property);

            IReportSchemaCellsProviderBuilder<TEntity> cellsProviderBuilder = builder.AddRow(attribute.Title, instance);

            this.ApplyAttributes(builder, cellsProviderBuilder, property, globalAttributes);
        }

        private IReportCellsProvider<TEntity> CreateCellsProvider<TEntity>(PropertyInfo property)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity));
            MemberExpression memberExpression = Expression.Property(parameter, typeof(TEntity), property.Name);
            LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameter);

            IReportCellsProvider<TEntity> instance = (IReportCellsProvider<TEntity>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(TEntity), property.PropertyType),
                lambdaExpression.Compile());
            return instance;
        }

        private IVerticalReportSchemaBuilder<TEntity> BuildVerticalReport<TEntity>(VerticalReportAttribute reportAttribute)
        {
            IVerticalReportSchemaBuilder<TEntity> builder = this.BuildVerticalReportNoPostBuild<TEntity>();

            if (reportAttribute?.PostBuilder != null
                && typeof(IVerticalReportPostBuilder<TEntity>).IsAssignableFrom(reportAttribute.PostBuilder))
            {
                ((IVerticalReportPostBuilder<TEntity>)ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, reportAttribute.PostBuilder))
                    .Build(builder);
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

            if (!typeof(IVerticalReportPostBuilder<TEntity, TBuildParameter>).IsAssignableFrom(reportAttribute.PostBuilder))
            {
                throw new InvalidOperationException($"Type {reportAttribute.PostBuilder} is not assignable to {typeof(IVerticalReportPostBuilder<TEntity, TBuildParameter>)}.");
            }

            ((IVerticalReportPostBuilder<TEntity, TBuildParameter>)ActivatorUtilities.GetServiceOrCreateInstance(this.serviceProvider, reportAttribute.PostBuilder))
                .Build(builder, parameter);

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
            Dictionary<int, Dictionary<string, List<int>>> complexHeader = new Dictionary<int, Dictionary<string, List<int>>>();

            foreach (ReportVariableAttribute property in properties.Select(p => p.Attribute))
            {
                for (int i = 0; i < property.ComplexHeader.Length; i++)
                {
                    if (!complexHeader.ContainsKey(i))
                    {
                        complexHeader.Add(i, new Dictionary<string, List<int>>());
                    }

                    string title = property.ComplexHeader[i];
                    if (!complexHeader[i].ContainsKey(title))
                    {
                        complexHeader[i].Add(title, new List<int>());
                    }

                    complexHeader[i][title].Add(property.Order);
                }
            }

            int minimumIndex = properties.Min(p => p.Attribute.Order);
            foreach (KeyValuePair<int, Dictionary<string, List<int>>> header in complexHeader)
            {
                foreach (KeyValuePair<string, List<int>> column in header.Value)
                {
                    builder.AddComplexHeader(header.Key, column.Key, column.Value.Min() - minimumIndex, column.Value.Max() - minimumIndex);
                }
            }
        }

        private void ProcessTablePropertyAttributes<TEntity>(IVerticalReportSchemaBuilder<TEntity> builder)
        {
            IEnumerable<TablePropertyAttribute> attributes = typeof(TEntity)
                .GetCustomAttributes<TablePropertyAttribute>();
            foreach (TablePropertyAttribute attribute in attributes)
            {
                foreach (IAttributeHandler handler in this.handlers)
                {
                    handler.Handle(builder, null, attribute);
                }
            }
        }

        private void AddColumn<TEntity>(IVerticalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute, Attribute[] globalAttributes)
        {
            IReportCellsProvider<TEntity> instance = this.CreateCellsProvider<TEntity>(property);

            IReportSchemaCellsProviderBuilder<TEntity> cellsProviderBuilder = builder.AddColumn(attribute.Title, instance);

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
                .Concat(globalAttributes
                    .Where(a => !(a is TablePropertyAttribute))
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
            return typeof(TEntity).GetProperties()
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
        }

        private Attribute[] GetGlobalAttributes<TEntity>()
        {
            return typeof(TEntity).GetCustomAttributes()
                .Where(a => !(a is ReportAttribute))
                .ToArray();
        }

        private class ReportVariableData
        {
            public PropertyInfo Property { get; set; }

            public ReportVariableAttribute Attribute { get; set; }
        }
    }
}
