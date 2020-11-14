using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Reports.Builders;
using Reports.Extensions.Builders.Attributes;
using Reports.Interfaces;
using Reports.ReportCellsProviders;

namespace Reports.Extensions.Builders.BuilderHelpers
{
    public class EntityAttributeBuilderHelper
    {
        public void BuildVerticalReport<TEntity>(VerticalReportBuilder<TEntity> builder)
        {
            ReportVariableData[] properties = typeof(TEntity).GetProperties()
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

            this.AddColumns(builder, properties);
            this.AddComplexHeader(builder, properties);
        }

        private void AddColumns<TEntity>(VerticalReportBuilder<TEntity> builder, ReportVariableData[] properties)
        {
            foreach (ReportVariableData x in properties)
            {
                this.AddColumn(builder, x.Property, x.Attribute);
            }
        }

        private void AddComplexHeader<TEntity>(VerticalReportBuilder<TEntity> builder, ReportVariableData[] properties)
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

        private void AddColumn<TEntity>(VerticalReportBuilder<TEntity> builder, PropertyInfo property, ReportVariableAttribute attribute)
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

            builder.AddColumn(instance);

            this.ApplyAttribute(instance, attribute);
        }

        private void ApplyAttribute<TEntity>(IReportCellsProvider<TEntity> provider, ReportVariableAttribute attribute)
        {
        }

        private class ReportVariableData
        {
            public PropertyInfo Property { get; set; }
            public ReportVariableAttribute Attribute { get; set; }
        }
    }
}
