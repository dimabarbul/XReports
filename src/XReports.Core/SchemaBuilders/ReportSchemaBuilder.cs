using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportSchemaCellsProviders;

namespace XReports.SchemaBuilders
{
    public abstract class ReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        private readonly ComplexHeaderBuilder complexHeaderBuilder = new ComplexHeaderBuilder();

        protected List<IReportSchemaCellsProviderBuilder<TSourceEntity>> CellsProviders { get; } = new List<IReportSchemaCellsProviderBuilder<TSourceEntity>>();

        protected Dictionary<string, IReportSchemaCellsProviderBuilder<TSourceEntity>> NamedProviders { get; } = new Dictionary<string, IReportSchemaCellsProviderBuilder<TSourceEntity>>();

        protected Dictionary<string, List<ReportCellProperty>> ComplexHeadersProperties { get; } = new Dictionary<string, List<ReportCellProperty>>();

        protected List<ReportCellProperty> CommonComplexHeadersProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportCellProperty> GlobalProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportTableProperty> TableProperties { get; } = new List<ReportTableProperty>();

        public IReportSchemaBuilder<TSourceEntity> AddGlobalProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.GlobalProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportTableProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.TableProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.ComplexHeadersProperties.ContainsKey(title))
            {
                this.ComplexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.ComplexHeadersProperties[title].AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.CommonComplexHeadersProperties.AddRange(properties);

            return this;
        }

        protected IReportSchemaCellsProviderBuilder<TSourceEntity> GetProvider(string title)
        {
            return this.CellsProviders[this.GetCellsProviderIndex(title)];
        }

        protected ReportSchemaCellsProviderBuilder<TSourceEntity> InsertCellsProvider(int index, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            if (title == null)
            {
                throw new ArgumentException("Title cannot be null", nameof(provider));
            }

            if (index < 0 || index > this.CellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            ReportSchemaCellsProviderBuilder<TSourceEntity> builder = new ReportSchemaCellsProviderBuilder<TSourceEntity>(title, provider);

            this.CellsProviders.Insert(index, builder);
            if (!this.NamedProviders.ContainsKey(title))
            {
                this.NamedProviders[title] = builder;
            }

            return builder;
        }

        protected int GetCellsProviderIndex(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            int index;
            if (!this.NamedProviders.ContainsKey(title) ||
                (index = this.CellsProviders.IndexOf(this.NamedProviders[title])) == -1)
            {
                throw new ArgumentException($"Cells provider with title {title} is not found", nameof(title));
            }

            return index;
        }

        protected ComplexHeaderCell[,] BuildComplexHeader(bool transpose)
        {
            ComplexHeaderCell[,] complexHeader = this.complexHeaderBuilder.Build(
                this.CellsProviders.Select(p => p.Title).ToArray());

            return transpose ? complexHeader.Transpose() : complexHeader;
        }

        private void ValidateAllPropertiesNotNull<TProperty>(TProperty[] properties)
        {
            if (properties.Any(p => p == null))
            {
                throw new ArgumentException("All properties should not be null", nameof(properties));
            }
        }
    }
}
