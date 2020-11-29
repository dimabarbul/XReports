using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Reports.Extensions.Builders.Attributes;
using Reports.Extensions.Builders.Enums;
using Reports.Interfaces;
using Reports.ReportCellsProviders;
using Reports.SchemaBuilders;

namespace Reports.Extensions.Builders.BuilderHelpers
{
    public class EntityAttributeBuilderHelper
    {
        public IReportSchema<TEntity> BuildSchema<TEntity>()
        {
            ReportTypeAttribute reportTypeAttribute = typeof(TEntity).GetCustomAttribute<ReportTypeAttribute>();

            return reportTypeAttribute?.Type == ReportType.Horizontal
                ? (IReportSchema<TEntity>) this.BuildVerticalReport<TEntity>().BuildSchema()
                : (IReportSchema<TEntity>) this.BuildHorizontalReport<TEntity>().BuildSchema();
        }

        private HorizontalReportSchemaBuilder<TEntity> BuildHorizontalReport<TEntity>()
        {
            HorizontalReportSchemaBuilder<TEntity> builder = new HorizontalReportSchemaBuilder<TEntity>();

            ReportVariableData[] reportVariables = this.GetProperties<TEntity>();

            this.AddRows(builder, reportVariables);

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

            this.ApplyAttribute(builder, attribute);
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

        public VerticalReportSchemaBuilder<TEntity> BuildVerticalReport<TEntity>()
        {
            VerticalReportSchemaBuilder<TEntity> builder = new VerticalReportSchemaBuilder<TEntity>();
            ReportVariableData[] properties = this.GetProperties<TEntity>();

            this.AddColumns(builder, properties);
            this.AddComplexHeader(builder, properties);

            return builder;
        }

        private void AddColumns<TEntity>(VerticalReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties)
        {
            foreach (ReportVariableData x in properties)
            {
                this.AddColumn(builder, x.Property, x.Attribute);
            }
        }

        private void AddComplexHeader<TEntity>(VerticalReportSchemaBuilder<TEntity> builder, ReportVariableData[] properties)
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

            this.ApplyAttribute(builder, attribute);
        }

        private void ApplyAttribute<TEntity>(ReportSchemaBuilder<TEntity> builder, ReportVariableAttribute attribute)
        {
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
