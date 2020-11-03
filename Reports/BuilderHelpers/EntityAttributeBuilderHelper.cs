using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Reports.Attributes;
using Reports.Builders;
using Reports.Interfaces;
using Reports.ReportCellsProviders;

namespace Reports.BuilderHelpers
{
    public class EntityAttributeBuilderHelper
    {
        public void BuildVerticalReport<TEntity>(VerticalReportBuilder<TEntity> builder)
        {
            foreach (var x in typeof(TEntity).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ReportVariableAttribute>(),
                })
                .Where(x => x.Property != null)
                .OrderBy(x => x.Attribute.Order))
            {
                this.AddColumn(builder, x.Property, x.Attribute);
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
    }
}
