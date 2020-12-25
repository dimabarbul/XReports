using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using XReports.ReportCellsProviders;
using XReports.Attributes;
using XReports.Enums;
using XReports.Interfaces;

namespace XReports.SchemaBuilders
{
    public class AttributeBasedBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly List<IAttributeHandler> attributeHandlers = new List<IAttributeHandler>();

        public AttributeBasedBuilder(IServiceProvider serviceProvider, IEnumerable<IAttributeHandler> handlers = null)
        {
            this.serviceProvider = serviceProvider;
            if (handlers != null)
            {
                this.attributeHandlers.AddRange(handlers);
            }
        }

        public void AddAttributeHandler(IAttributeHandler handler)
        {
            this.attributeHandlers.Add(handler);
        }

        public IReportSchema<TEntity> BuildSchema<TEntity>()
        {
            ReportAttribute reportAttribute = typeof(TEntity).GetCustomAttribute<ReportAttribute>();

            return reportAttribute?.Type == ReportType.Horizontal
                ? (IReportSchema<TEntity>) this.BuildHorizontalReport<TEntity>(reportAttribute as HorizontalReportAttribute).BuildSchema()
                : (IReportSchema<TEntity>) this.BuildVerticalReport<TEntity>(reportAttribute as VerticalReportAttribute).BuildSchema();
        }

        private HorizontalReportSchemaBuilder<TEntity> BuildHorizontalReport<TEntity>(HorizontalReportAttribute reportAttribute = null)
        {
            HorizontalReportSchemaBuilder<TEntity> builder = new HorizontalReportSchemaBuilder<TEntity>();

            ReportVariableData[] reportVariables = this.GetProperties<TEntity>();
            Attribute[] tableAttributes = this.GetTableAttributes<TEntity>();

            this.AddRows(builder, reportVariables, tableAttributes);
            this.AddComplexHeader(builder, reportVariables);

            if (reportAttribute?.PostBuilder != null)
            {
                ((IHorizontalReportPostBuilder<TEntity>) this.serviceProvider.GetRequiredService(reportAttribute.PostBuilder)).Build(builder);
            }

            return builder;
        }

        private void AddRows<TEntity>(HorizontalReportSchemaBuilder<TEntity> builder, ReportVariableData[] reportVariables, Attribute[] tableAttributes)
        {
            foreach (ReportVariableData x in reportVariables)
            {
                this.AddRow(builder, x.Property, x.Attribute, tableAttributes);
            }
        }

        private void AddRow<TEntity>(HorizontalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute, Attribute[] tableAttributes)
        {
            IReportCellsProvider<TEntity> instance = this.CreateCellsProvider<TEntity>(property, attribute);

            builder.AddRow(instance);
            builder.AddAlias(property.Name);

            this.ApplyAttributes(builder, property, tableAttributes);
        }

        private IReportCellsProvider<TEntity> CreateCellsProvider<TEntity>(PropertyInfo property, ReportVariableAttribute attribute)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            MemberExpression memberExpression = Expression.Property(parameter, typeof(TEntity), property.Name);
            LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameter);

            IReportCellsProvider<TEntity> instance = (IReportCellsProvider<TEntity>) Activator.CreateInstance(
                typeof(EntityPropertyReportCellsProvider<,>)
                    .MakeGenericType(typeof(TEntity), property.PropertyType),
                attribute.Title,
                lambdaExpression.Compile()
            );
            return instance;
        }

        public VerticalReportSchemaBuilder<TEntity> BuildVerticalReport<TEntity>(VerticalReportAttribute reportAttribute = null)
        {
            VerticalReportSchemaBuilder<TEntity> builder = new VerticalReportSchemaBuilder<TEntity>();
            Attribute[] tableAttributes = this.GetTableAttributes<TEntity>();
            ReportVariableData[] properties = this.GetProperties<TEntity>();

            this.AddColumns(builder, properties, tableAttributes);
            this.AddComplexHeader(builder, properties);

            if (reportAttribute?.PostBuilder != null)
            {
                ((IVerticalReportPostBuilder<TEntity>) this.serviceProvider.GetRequiredService(reportAttribute.PostBuilder)).Build(builder);
            }

            return builder;
        }

        private void AddColumns<TEntity>(VerticalReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties, Attribute[] tableAttributes)
        {
            foreach (ReportVariableData x in properties)
            {
                this.AddColumn(builder, x.Property, x.Attribute, tableAttributes);
            }
        }

        private void AddComplexHeader<TEntity>(ReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties)
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
            foreach ((int index, Dictionary<string, List<int>> header) in complexHeader)
            {
                foreach ((string title, List<int> columns) in header)
                {
                    builder.AddComplexHeader(index, title, columns.Min() - minimumIndex, columns.Max() - minimumIndex);
                }
            }
        }

        private void AddColumn<TEntity>(VerticalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute, Attribute[] tableAttributes)
        {
            IReportCellsProvider<TEntity> instance = this.CreateCellsProvider<TEntity>(property, attribute);

            builder.AddColumn(instance);
            builder.AddAlias(property.Name);

            this.ApplyAttributes(builder, property, tableAttributes);
        }

        private void ApplyAttributes<TEntity>(ReportSchemaBuilder<TEntity> builder, PropertyInfo property, Attribute[] tableAttributes)
        {
            Attribute[] propertyAttributes = property.GetCustomAttributes().ToArray();
            Attribute[] attributes = this.MergeTableAttributes(propertyAttributes, tableAttributes);

            foreach (Attribute attribute in attributes)
            {
                foreach (IAttributeHandler handler in this.attributeHandlers)
                {
                    handler.Handle(builder, attribute);
                }
            }
        }

        private Attribute[] MergeTableAttributes(Attribute[] propertyAttributes, Attribute[] tableAttributes)
        {
            return propertyAttributes
                .Concat(tableAttributes
                    .Where(ta =>
                        (
                            !(ta is AttributeBase)
                            && propertyAttributes.All(pa => pa.GetType() != ta.GetType())
                        )
                        || (
                            ta is AttributeBase
                            && propertyAttributes.All(pa =>
                                pa.GetType() != ta.GetType()
                                || ((AttributeBase)pa).IsHeader != ((AttributeBase)ta).IsHeader
                            )
                        )
                    )
                )
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

        private Attribute[] GetTableAttributes<TEntity>()
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
