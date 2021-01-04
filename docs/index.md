# XReports Documentation

XReports is a library intended to provide extendable way of building reports and exporting them to different formats: HTML, Excel etc. having the same data source. There are two libraries:
- **XReports.Core** - provides classes and interfaces required for the library to work
- **XReports** - provides extra functionality:
    - basic properties: bold, color etc.
    - Html and Excel support: models and property handlers
    - AttributeBasedBuilder: class allowing building reports using C# attributes
    - StringWriter - class allowing writing Html report to string
    - EpplusWriter - class allowing writing Excel report to xlsx file or stream using [Epplus 4](https://github.com/JanKallman/EPPlus)

## Table of Contents
1. XReports.Core
    1. [Building Reports](core/1.md)
    2. [Properties](core/2.md)
    3. [Using Report Converter](core/3.md)
    4. Cell Processors
    5. Cells Provider
    6. .NET Core Integration
2. XReports
    1. Properties
    2. Using AttributeBasedBuilder
    3. StringWriter
    4. EpplusWriter
