# XReports Documentation

XReports is a library that provides extensible way of building reports and exporting them to different formats: HTML, Excel etc. having the same data source. The main idea of the library is separating report schema and report presentation.

There are two libraries:
- **XReports.Core** - provides classes and interfaces required for the library to work
- **XReports** - provides extra functionality:
    - basic properties: bold, color etc.
    - Html and Excel support: models and property handlers
    - AttributeBasedBuilder: class allowing building reports using C# attributes
    - StringWriter - class allowing writing Html report to string
    - EpplusWriter - class allowing writing Excel report to xlsx file or stream using [Epplus 4](https://github.com/JanKallman/EPPlus)

Both libraries provide integration with [.NET DI](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection).

## Table of Contents
1. XReports.Core
    1. [Building Reports](xreports.core/building-reports.md)
    2. [Value providers](xreports.core/value-providers.md)
    3. [Properties](xreports.core/properties.md)
    4. [Using Report Converter](xreports.core/using-report-converter.md)
    5. [Cell Processors](xreports.core/cell-processors.md)
    6. [Cell Providers](xreports.core/cell-providers.md)
    7. [.NET Core Integration](xreports.core/net-core-integration.md)
2. XReports
    1. [Cell types](xreports/cell-types.md)
    2. [Properties](xreports/properties.md)
    3. [AttributeBasedBuilder](xreports/attribute-based-builder.md)
    4. [Html Writers](xreports/html-writers.md)
    5. [EpplusWriter](xreports/epplus-writer.md)
