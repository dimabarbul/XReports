using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Reports.Core.Interfaces;
using Reports.Core.ReportCellsProviders;
using Reports.Core.SchemaBuilders;
using Reports.Extensions.AttributeBasedBuilder.Attributes;
using Reports.Extensions.AttributeBasedBuilder.Enums;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;

namespace Reports.Extensions.AttributeBasedBuilder
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

            this.AddRows(builder, reportVariables);
            this.AddComplexHeader(builder, reportVariables);

            if (reportAttribute?.PostBuilder != null)
            {
                ((IHorizontalReportPostBuilder<TEntity>) this.serviceProvider.GetRequiredService(reportAttribute.PostBuilder)).Build(builder);
            }

            return builder;
        }

        private void AddRows<TEntity>(HorizontalReportSchemaBuilder<TEntity> builder, ReportVariableData[] reportVariables)
        {
            foreach (ReportVariableData x in reportVariables)
            {
                this.AddRow(builder, x.Property, x.Attribute);
            }
        }

        private void AddRow<TEntity>(HorizontalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute)
        {
            IReportCellsProvider<TEntity> instance = this.CreateCellsProvider<TEntity>(property, attribute);

            builder.AddRow(instance);
            builder.AddAlias(property.Name);

            this.ApplyAttribute(builder, property);
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
            ReportVariableData[] properties = this.GetProperties<TEntity>();

            this.AddColumns(builder, properties);
            this.AddComplexHeader(builder, properties);

            if (reportAttribute?.PostBuilder != null)
            {
                ((IVerticalReportPostBuilder<TEntity>) this.serviceProvider.GetRequiredService(reportAttribute.PostBuilder)).Build(builder);
            }

            return builder;
        }

        private void AddColumns<TEntity>(VerticalReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties)
        {
            foreach (ReportVariableData x in properties)
            {
                this.AddColumn(builder, x.Property, x.Attribute);
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

        private void AddColumn<TEntity>(VerticalReportSchemaBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute)
        {
            IReportCellsProvider<TEntity> instance = this.CreateCellsProvider<TEntity>(property, attribute);

            builder.AddColumn(instance);
            builder.AddAlias(property.Name);

            this.ApplyAttribute(builder, property);
        }

        private void ApplyAttribute<TEntity>(ReportSchemaBuilder<TEntity> builder, PropertyInfo property)
        {
            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                foreach (IAttributeHandler handler in this.attributeHandlers)
                {
                    handler.Handle(builder, attribute);
                }
            }
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

        private class ReportVariableData
        {
            public PropertyInfo Property { get; set; }
            public ReportVariableAttribute Attribute { get; set; }
        }
    }
}
