using System;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ComplexHeaderAttribute : Attribute
    {
        public int RowIndex { get; }
        public string Title { get; }
        public int RowSpan { get; } = 1;

        public int StartIndex { get; }
        public int? EndIndex { get; }

        public string StartTitle { get; }
        public string EndTitle { get; }

        public bool UsesIndexes { get; }

        public ComplexHeaderAttribute(int rowIndex, string title, int startIndex)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartIndex = startIndex;

            this.UsesIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, string title, int startIndex, int endIndex)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;

            this.UsesIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, string title, string startTitle)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartTitle = startTitle;

            this.UsesIndexes = false;
        }

        public ComplexHeaderAttribute(int rowIndex, string title, string startTitle, string endTitle)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartTitle = startTitle;
            this.EndTitle = endTitle;

            this.UsesIndexes = false;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, int startIndex)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartIndex = startIndex;

            this.UsesIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, int startIndex, int endIndex)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;

            this.UsesIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, string startTitle)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartTitle = startTitle;

            this.UsesIndexes = false;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, string startTitle, string endTitle)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartTitle = startTitle;
            this.EndTitle = endTitle;

            this.UsesIndexes = false;
        }
    }
}
