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

        public bool UseId { get; }
        public bool UseIndexes { get; }

        public ComplexHeaderAttribute(int rowIndex, string title, int startIndex)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartIndex = startIndex;

            this.UseIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, string title, int startIndex, int endIndex)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;

            this.UseIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, string title, string startTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartTitle = startTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        public ComplexHeaderAttribute(int rowIndex, string title, string startTitle, string endTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.Title = title;
            this.StartTitle = startTitle;
            this.EndTitle = endTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, int startIndex)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartIndex = startIndex;

            this.UseIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, int startIndex, int endIndex)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;

            this.UseIndexes = true;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, string startTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartTitle = startTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string title, string startTitle, string endTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Title = title;
            this.StartTitle = startTitle;
            this.EndTitle = endTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }
    }
}
