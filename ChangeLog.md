# Change Log

## NEXT

Released on: *YYYY-MM-DD*

**Added:**
- Benchmarks for new version (for current project code) and old version (the version specified in `csproj` file as dependency)
- `HtmlStreamWriterDI.AddHtmlStreamWriter` and `HtmlStreamWriterDI.AddHtmlStreamCellWriter` to register Html stream writer in DI
- `IHtmlStreamWriter`/`IHtmlStreamCellWriter` and `HtmlStreamWriter`/`HtmlStreamCellWriter`
- New method `BaseReportCell.Clear` resets report cell to initial state, it should be overriden in derived classes

**Changed:**
- XReports and XReports.Core libraries are now targeting netstandard2.0 and netstandard2.1
- Improved performance and memory consumption
- Aligned versions of XReports.Core and XReports libraries
- DI refactored
- Report cells are now cached and reused, so reference to cell should not be persisted anywhere; if copy of cell is needed, one can use `BaseReportCell.Clone` method to create shallow copy
- Properties now are processed by the first property handler that marks them processed, i.e., the property is not processed after it's been marked as processed
- Replaced `BaseReportCell.Value` setter with `BaseReportCell.SetValue<TValue>` method, getter – with `BaseReportCell.GetUnderlyingValue` method
- `IPropertyHandler.Handle` method is now `bool`: it should return `true` if the property has been processed, `false` otherwise
- All attributes are sealed now
- Renamed classes and interfaces:
  - AttributeBase ⇒ BasePropertyAttribute
  - IStringWriter ⇒ IHtmlStringWriter
    - WriteToStringAsync ⇒ WriteToString
    - removed method `WriteToFileAsync`, `IHtmlStreamWriter` should be used instead
  - IStringCellWriter ⇒ IHtmlStringCellWriter:
    - methods now accept `StringBuilder` instead of returning `string`
  - StringWriter ⇒ HtmlStringWriter
  - StringCellWriter ⇒ HtmlStringCellWriter
  - StringWriterDI ⇒ HtmlStringWriterDI:
    - AddStringWriter ⇒ AddHtmlStringWriter
    - AddStringCellWriter ⇒ AddHtmlStringCellWriter
  - IEpplusWriter:
    - added method `WriteToStream` that accepts existing stream
  - `IReportCellsProvider.CellSelector` property is replaced with `IReportCellsProvider.GetCell` method

**Fixed:**
- Global properties are available to cell processors

**Removed:**
- `ReportCellProperty.Processed` property
- `ReportSchema<>.CreateVertical` and `ReportSchema<>.CreateHorizontal` methods, constructors of `VerticalReportSchema<>` and `HorizontalReportSchema<>` should be used instead

**Performance and Memory Consumption Benchmarks**

Version 0.1.6

```
BenchmarkDotNet v0.13.6, Gentoo Linux
Intel Core i3-3217U CPU 1.80GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores
.NET SDK 6.0.404
  [Host]     : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX
  Job-OJWZPB : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX

IterationCount=4  RunStrategy=Monitoring  WarmupCount=2

|                                                                     Method | RowCount |     Mean |    Error |   StdDev |         Gen0 |        Gen1 |      Gen2 |  Allocated |
|--------------------------------------------------------------------------- |--------- |---------:|---------:|---------:|-------------:|------------:|----------:|-----------:|
|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |  1.529 s | 0.1334 s | 0.0206 s |  340000.0000 |           - |         - |  509.16 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |  2.724 s | 0.2890 s | 0.0447 s |  254000.0000 |  16000.0000 |         - |  901.84 MB |
|                          'Save vertical HTML report from entities to file' |    10000 |  4.113 s | 0.3720 s | 0.0576 s |  559000.0000 |           - |         - |  836.75 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |  1.114 s | 0.2316 s | 0.0358 s |  278000.0000 |           - |         - |  417.24 MB |
|                          'Save vertical XLSX report from entities to file' |    10000 |  7.770 s | 0.1589 s | 0.0246 s |  425000.0000 |  69000.0000 | 4000.0000 |  916.25 MB |
|                        'Save vertical XLSX report from entities to stream' |    10000 |  7.773 s | 0.2550 s | 0.0395 s |  424000.0000 |  66000.0000 | 3000.0000 |  907.91 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |  1.771 s | 0.2060 s | 0.0319 s |  341000.0000 |           - |         - |  510.76 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |  2.926 s | 0.3473 s | 0.0537 s |  268000.0000 |  15000.0000 |         - |  903.45 MB |
|                       'Save vertical HTML report from data reader to file' |    10000 |  4.198 s | 0.1659 s | 0.0257 s |  560000.0000 |           - |         - |  838.35 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |  1.368 s | 0.2752 s | 0.0426 s |  280000.0000 |           - |         - |  418.85 MB |
|                       'Save vertical XLSX report from data reader to file' |    10000 |  7.766 s | 0.2314 s | 0.0358 s |  426000.0000 |  63000.0000 | 4000.0000 |  917.85 MB |
|                     'Save vertical XLSX report from data reader to stream' |    10000 |  8.097 s | 0.2748 s | 0.0425 s |  424000.0000 |  72000.0000 | 3000.0000 |  909.52 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |  1.418 s | 0.2982 s | 0.0462 s |  338000.0000 |           - |         - |  505.75 MB |
|                'Save horizontal HTML report to string using StringBuilder' |    10000 |  2.628 s | 0.2030 s | 0.0314 s |  410000.0000 |  15000.0000 |         - |  897.63 MB |
|                                      'Save horizontal HTML report to file' |    10000 |  3.812 s | 0.3476 s | 0.0538 s |  556000.0000 |           - |         - |  832.14 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |  1.047 s | 0.2719 s | 0.0421 s |  276000.0000 |           - |         - |   413.9 MB |
|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 | 14.747 s | 0.3557 s | 0.0551 s | 3403000.0000 |   1000.0000 |         - | 5090.69 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 | 27.353 s | 0.4325 s | 0.0669 s | 2492000.0000 | 139000.0000 | 2000.0000 | 9017.29 MB |
|                          'Save vertical HTML report from entities to file' |   100000 | 40.257 s | 1.2591 s | 0.1949 s | 5594000.0000 |   4000.0000 |         - | 8366.45 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 | 11.687 s | 0.3418 s | 0.0529 s | 2789000.0000 |   1000.0000 |         - | 4171.61 MB |
|                          'Save vertical XLSX report from entities to file' |   100000 | 54.183 s | 0.8384 s | 0.1297 s | 4424000.0000 | 552000.0000 | 5000.0000 | 7770.61 MB |
|                        'Save vertical XLSX report from entities to stream' |   100000 | 53.141 s | 0.8405 s | 0.1301 s | 4439000.0000 | 545000.0000 | 5000.0000 | 7739.73 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 | 17.106 s | 0.3462 s | 0.0536 s | 3414000.0000 |   1000.0000 |         - | 5106.72 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 | 28.679 s | 1.0681 s | 0.1653 s | 2590000.0000 |  90000.0000 | 3000.0000 | 9033.37 MB |
|                       'Save vertical HTML report from data reader to file' |   100000 | 42.110 s | 2.6510 s | 0.4102 s | 5604000.0000 |   4000.0000 |         - | 8382.48 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 | 13.382 s | 0.1158 s | 0.0179 s | 2799000.0000 |   1000.0000 |         - | 4187.64 MB |
|                       'Save vertical XLSX report from data reader to file' |   100000 | 57.466 s | 1.5327 s | 0.2372 s | 4438000.0000 | 548000.0000 | 5000.0000 | 7786.63 MB |
|                     'Save vertical XLSX report from data reader to stream' |   100000 | 54.954 s | 0.8818 s | 0.1365 s | 4433000.0000 | 555000.0000 | 5000.0000 | 7755.76 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 | 14.802 s | 0.2360 s | 0.0365 s | 3380000.0000 |   1000.0000 |         - | 5056.43 MB |
|                'Save horizontal HTML report to string using StringBuilder' |   100000 | 25.814 s | 0.6748 s | 0.1044 s | 4129000.0000 | 345000.0000 | 1000.0000 | 8974.98 MB |
|                                      'Save horizontal HTML report to file' |   100000 | 37.262 s | 2.0455 s | 0.3165 s | 5566000.0000 |   3000.0000 |         - | 8320.18 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 | 11.098 s | 0.3586 s | 0.0555 s | 2766000.0000 |   1000.0000 |         - | 4138.06 MB |
```

Version NEXT

```
BenchmarkDotNet v0.13.6, Gentoo Linux
Intel Core i3-3217U CPU 1.80GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores
.NET SDK 6.0.404
  [Host]     : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX
  Job-IYPEDP : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX

Server=False  IterationCount=4  RunStrategy=Monitoring
WarmupCount=2

|                                                                     Method | RowCount |        Mean |       Error |    StdDev |        Gen0 |       Gen1 |      Gen2 |  Allocated |
|--------------------------------------------------------------------------- |--------- |------------:|------------:|----------:|------------:|-----------:|----------:|-----------:|
|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    320.2 ms |    73.21 ms |  11.33 ms |   3000.0000 |          - |         - |    4.96 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    783.6 ms |   193.43 ms |  29.93 ms |  22000.0000 |  5000.0000 |         - |  157.46 MB |
|                          'Save vertical HTML report from entities to file' |    10000 |  2,229.5 ms |   163.76 ms |  25.34 ms |  40000.0000 |          - |         - |   59.78 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    279.3 ms |   156.38 ms |  24.20 ms |   1000.0000 |          - |         - |    1.99 MB |
|                          'Save vertical XLSX report from entities to file' |    10000 |  5,972.7 ms |   455.16 ms |  70.44 ms |  28000.0000 | 12000.0000 | 4000.0000 |  288.07 MB |
|                        'Save vertical XLSX report from entities to stream' |    10000 |  5,886.5 ms |   260.38 ms |  40.29 ms |  31000.0000 | 11000.0000 | 3000.0000 |  279.75 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    381.9 ms |   149.71 ms |  23.17 ms |   4000.0000 |          - |         - |    6.87 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |    865.3 ms |    17.32 ms |   2.68 ms |  25000.0000 |  7000.0000 | 1000.0000 |  159.37 MB |
|                       'Save vertical HTML report from data reader to file' |    10000 |  2,249.4 ms |   527.39 ms |  81.61 ms |  41000.0000 |          - |         - |    61.7 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    321.2 ms |   206.48 ms |  31.95 ms |   2000.0000 |          - |         - |     3.9 MB |
|                       'Save vertical XLSX report from data reader to file' |    10000 |  6,020.6 ms |   201.20 ms |  31.14 ms |  29000.0000 | 12000.0000 | 4000.0000 |  289.98 MB |
|                     'Save vertical XLSX report from data reader to stream' |    10000 |  5,919.5 ms |   210.66 ms |  32.60 ms |  30000.0000 | 12000.0000 | 3000.0000 |  281.66 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    304.3 ms |   118.03 ms |  18.27 ms |   3000.0000 |          - |         - |    4.97 MB |
|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    771.7 ms |   184.96 ms |  28.62 ms |  23000.0000 |  5000.0000 |         - |  157.11 MB |
|                                      'Save horizontal HTML report to file' |    10000 |  1,813.6 ms |   217.90 ms |  33.72 ms |  41000.0000 |          - |         - |    59.8 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    278.7 ms |   131.74 ms |  20.39 ms |   1000.0000 |          - |         - |       2 MB |
|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  3,333.8 ms |   290.40 ms |  44.94 ms |  32000.0000 |          - |         - |   48.84 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  8,027.7 ms |    72.25 ms |  11.18 ms | 239000.0000 | 59000.0000 | 3000.0000 | 1573.71 MB |
|                          'Save vertical HTML report from entities to file' |   100000 | 21,390.2 ms |   539.42 ms |  83.48 ms | 401000.0000 |  2000.0000 |         - |     597 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |  2,680.7 ms |    80.77 ms |  12.50 ms |  12000.0000 |          - |         - |   19.16 MB |
|                          'Save vertical XLSX report from entities to file' |   100000 | 35,729.5 ms |   666.38 ms | 103.12 ms | 440000.0000 | 20000.0000 | 5000.0000 | 1451.89 MB |
|                        'Save vertical XLSX report from entities to stream' |   100000 | 35,755.5 ms |   633.35 ms |  98.01 ms | 441000.0000 | 18000.0000 | 5000.0000 | 1421.11 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  3,732.9 ms |    53.80 ms |   8.33 ms |  45000.0000 |          - |         - |   67.91 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  8,708.1 ms |   111.63 ms |  17.27 ms | 236000.0000 | 57000.0000 | 3000.0000 | 1592.79 MB |
|                       'Save vertical HTML report from data reader to file' |   100000 | 22,932.7 ms | 1,480.10 ms | 229.05 ms | 413000.0000 |  2000.0000 |         - |  616.09 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  3,242.1 ms |   225.51 ms |  34.90 ms |  25000.0000 |          - |         - |   38.23 MB |
|                       'Save vertical XLSX report from data reader to file' |   100000 | 37,411.2 ms | 1,015.13 ms | 157.09 ms | 443000.0000 | 18000.0000 | 5000.0000 | 1470.99 MB |
|                     'Save vertical XLSX report from data reader to stream' |   100000 | 37,150.4 ms |   608.46 ms |  94.16 ms | 442000.0000 | 19000.0000 | 5000.0000 | 1440.22 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  3,041.9 ms |   347.01 ms |  53.70 ms |  32000.0000 |          - |         - |   48.85 MB |
|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  7,579.0 ms |   155.18 ms |  24.01 ms | 210000.0000 | 52000.0000 | 3000.0000 | 1570.28 MB |
|                                      'Save horizontal HTML report to file' |   100000 | 18,344.8 ms |   857.07 ms | 132.63 ms | 412000.0000 |  1000.0000 |         - |  596.96 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  2,637.9 ms |   374.68 ms |  57.98 ms |  12000.0000 |          - |         - |   19.17 MB |
```
