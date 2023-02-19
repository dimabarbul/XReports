# EpplusWriter

EpplusWriter class provides ability to write Excel report to XLSX file (or stream with XLSX content) using [Epplus 4](https://github.com/JanKallman/EPPlus).

EpplusWriter class **is not** thread safe, i.e., you should not use one instance to write several reports in parallel.

## .NET Core Integration

You need to call extension method AddEpplusWriter. There are 3 forms of this method.

```c#
// Registers EpplusWriter as implementation of IEpplusWriter.
services.AddEpplusWriter();

class MyExcelWriter : EpplusWriter {}
// Registers derived class of EpplusWriter as implementation of IEpplusWriter.
// It may be useful if you want to extend/override EpplusWriter methods.
services.AddEpplusWriter<MyExcelWriter>();

interface IExtendedExcelWriter : IEpplusWriter {}
class ExtendedExcelWriter : EpplusWriter, IExtendedExcelWriter {}
// Registers derived class of EpplusWriter as implementation of IEpplusWriter
// and your interface inherited from IEpplusWriter.
// It may be useful if you want to add method(s) to EpplusWriter.
services.AddEpplusWriter<IExtendedExcelWriter, ExtendedExcelWriter>();
```

All of the forms accept 2 optional arguments:
- configuration callback to configure EpplusWriter options
- service lifetime - it applies to EpplusWriter class and all formatters provided in configuration callback

Example:

```c#
// Registers EpplusWriter along with all formatters (implementors of
// IEpplusFormatter interface) from executing assembly as singletons.
services.AddEpplusWriter(
    o => o.AddFromAssembly(Assembly.GetExecutingAssembly()),
    ServiceLifetime.Singleton);
```

## Public Methods

There are 2 public methods to export report:
- **WriteToFile**: saves report as XLSX file
- **WriteToStream**: creates in-memory stream, saves report to it, rewinds and returns it

## Extending EpplusWriter

EpplusWriter class contains number of virtual methods you can override.

General Flow:

- WriteToFile/WriteToStream
    - WriteReportToWorksheet (row=1, column=1)
        - WriteHeader
            - WriteHeaderCell
                - WriteCell
            - FormatHeader
        - WriteBody
            - WriteCell
                - FormatCell (if cell does not have SameColumnFormatProperty)
            - ApplyColumnFormat
                - FormatCell (for columns that has SameColumnFormatProperty)
        - PostCreate

All methods in above list lower WriteReportToWorksheet can be overridden.

## Using Extra Excel Features

EpplusWriter supports limited list of features of Excel cells:
- horizontal alignment
- number format
- bold font
- font and background color

If you want to use more Excel features to apply your cell properties, you have 2 options:
- extend EpplusWriter to handle custom properties
- create formatter class

### Extend EpplusWriter

```c#
// Property marking cells that should be indented.
class IndentationProperty : ReportCellProperty { }

class MyExcelWriter : EpplusWriter
{
    // Override method to handle custom property.
    protected override void FormatCell(ExcelRange worksheetCell, ExcelReportCell cell)
    {
        base.FormatCell(worksheetCell, cell);

        // As we don't have handler of this property during conversion to Excel report
        // the property will remain in cell.
        IndentationProperty indentationProperty = cell.GetProperty<IndentationProperty>();
        if (indentationProperty != null)
        {
            // Indent cell content.
            worksheetCell.Style.Indent = 1;
        }
    }
}
```

### Create Formatter

```c#
// Property marking cells that should be indented.
class IndentationProperty : ReportCellProperty { }

// The class will be automatically picked up and passed to EpplusWriter.
private class ExcelIndentationPropertyFormatter : EpplusFormatter<IndentationProperty>
{
    protected override void Format(ExcelRange worksheetCell, ExcelReportCell cell, IndentationProperty property)
    {
        worksheetCell.Style.Indent = 1;
    }
}
```
