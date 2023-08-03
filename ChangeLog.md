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

Benchmarks were run on report with 36 columns. Source code can be found [here](benchmarks/XReports.Benchmarks). Benchmarks for XReport 0.1.6 were run on commit "Update benchmarks for XReport=0.1.6" (hash f95256cdad5c30a5419a839504d21cadab2eaf48). Benchmark results are provided to highlight improvements brought by version NEXT comparing to version 0.1.6. Thanks to [l3r0r0](https://github.com/l3r0r0) for help with the benchmarks.

```diff
-Version 0.1.6
+Version NEXT

BenchmarkDotNet v0.13.6, Gentoo Linux
Intel Core i3-3217U CPU 1.80GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores
.NET SDK 6.0.404
  [Host]     : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX
  Job-OJWZPB : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX

IterationCount=4  RunStrategy=Monitoring  WarmupCount=2

 |                                                                     Method | RowCount |        Mean |       Error |    StdDev |         Gen0 |        Gen1 |      Gen2 |  Allocated |
 |--------------------------------------------------------------------------- |--------- |------------:|------------:|----------:|-------------:|------------:|----------:|-----------:|
-|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |  1,529.0 ms |   133.40 ms |  20.60 ms |  340000.0000 |           - |         - |  509.16 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    321.9 ms |    76.19 ms |  11.79 ms |    3000.0000 |           - |         - |    4.96 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |  2,724.0 ms |   289.00 ms |  44.70 ms |  254000.0000 |  16000.0000 |         - |  901.84 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    764.2 ms |   183.91 ms |  28.46 ms |   24000.0000 |   5000.0000 |         - |  157.46 MB |
-|                          'Save vertical HTML report from entities to file' |    10000 |  4,113.0 ms |   372.00 ms |  57.60 ms |  559000.0000 |           - |         - |  836.75 MB |
+|                          'Save vertical HTML report from entities to file' |    10000 |  2,002.1 ms |   303.34 ms |  46.94 ms |   40000.0000 |           - |         - |   59.68 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |  1,114.0 ms |   231.60 ms |  35.80 ms |  278000.0000 |           - |         - |  417.24 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    280.4 ms |    21.97 ms |   3.40 ms |    1000.0000 |           - |         - |    1.99 MB |
-|                          'Save vertical XLSX report from entities to file' |    10000 |  7,770.0 ms |   158.90 ms |  24.60 ms |  425000.0000 |  69000.0000 | 4000.0000 |  916.25 MB |
+|                          'Save vertical XLSX report from entities to file' |    10000 |  5,976.9 ms |   287.88 ms |  44.55 ms |   28000.0000 |  12000.0000 | 4000.0000 |  288.07 MB |
-|                        'Save vertical XLSX report from entities to stream' |    10000 |  7,773.0 ms |   255.00 ms |  39.50 ms |  424000.0000 |  66000.0000 | 3000.0000 |  907.91 MB |
+|                        'Save vertical XLSX report from entities to stream' |    10000 |  5,852.9 ms |   326.65 ms |  50.55 ms |   28000.0000 |  11000.0000 | 3000.0000 |  279.75 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |  1,771.0 ms |   206.00 ms |  31.90 ms |  341000.0000 |           - |         - |  510.76 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    384.5 ms |   230.46 ms |  35.66 ms |    4000.0000 |           - |         - |    6.87 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |  2,926.0 ms |   347.30 ms |  53.70 ms |  268000.0000 |  15000.0000 |         - |  903.45 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |    863.0 ms |   195.32 ms |  30.23 ms |   22000.0000 |   5000.0000 |         - |  159.37 MB |
-|                       'Save vertical HTML report from data reader to file' |    10000 |  4,198.0 ms |   165.90 ms |  25.70 ms |  560000.0000 |           - |         - |  838.35 MB |
+|                       'Save vertical HTML report from data reader to file' |    10000 |  2,060.8 ms |   206.35 ms |  31.93 ms |   41000.0000 |           - |         - |   61.59 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |  1,368.0 ms |   275.20 ms |  42.60 ms |  280000.0000 |           - |         - |  418.85 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    325.9 ms |   200.59 ms |  31.04 ms |    2000.0000 |           - |         - |    3.90 MB |
-|                       'Save vertical XLSX report from data reader to file' |    10000 |  7,766.0 ms |   231.40 ms |  35.80 ms |  426000.0000 |  63000.0000 | 4000.0000 |  917.85 MB |
+|                       'Save vertical XLSX report from data reader to file' |    10000 |  6,042.6 ms |   159.84 ms |  24.74 ms |   29000.0000 |  12000.0000 | 4000.0000 |  289.99 MB |
-|                     'Save vertical XLSX report from data reader to stream' |    10000 |  8,097.0 ms |   274.80 ms |  42.50 ms |  424000.0000 |  72000.0000 | 3000.0000 |  909.52 MB |
+|                     'Save vertical XLSX report from data reader to stream' |    10000 |  5,978.4 ms |   329.38 ms |  50.97 ms |   30000.0000 |  12000.0000 | 3000.0000 |  281.66 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |  1,418.0 ms |   298.20 ms |  46.20 ms |  338000.0000 |           - |         - |  505.75 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    318.9 ms |    79.95 ms |  12.37 ms |    3000.0000 |           - |         - |    4.97 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |    10000 |  2,628.0 ms |   203.00 ms |  31.40 ms |  410000.0000 |  15000.0000 |         - |  897.63 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    759.1 ms |   236.93 ms |  36.67 ms |   23000.0000 |   5000.0000 |         - |  157.11 MB |
-|                                      'Save horizontal HTML report to file' |    10000 |  3,812.0 ms |   347.60 ms |  53.80 ms |  556000.0000 |           - |         - |  832.14 MB |
+|                                      'Save horizontal HTML report to file' |    10000 |  1,807.2 ms |   774.49 ms | 119.85 ms |   41000.0000 |           - |         - |   59.69 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |  1,047.0 ms |   271.90 ms |  42.10 ms |  276000.0000 |           - |         - |  413.90 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    284.8 ms |   249.26 ms |  38.57 ms |    1000.0000 |           - |         - |    2.00 MB |
-|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 | 14,747.0 ms |   355.70 ms |  55.10 ms | 3403000.0000 |   1000.0000 |         - | 5090.69 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  3,123.0 ms |   227.29 ms |  35.17 ms |   32000.0000 |           - |         - |   48.84 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 | 27,353.0 ms |   432.50 ms |  66.90 ms | 2492000.0000 | 139000.0000 | 2000.0000 | 9017.29 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  7,952.8 ms |   274.26 ms |  42.44 ms |  239000.0000 |  58000.0000 | 3000.0000 | 1573.71 MB |
-|                          'Save vertical HTML report from entities to file' |   100000 | 40,257.0 ms | 1,259.10 ms | 194.90 ms | 5594000.0000 |   4000.0000 |         - | 8366.45 MB |
+|                          'Save vertical HTML report from entities to file' |   100000 | 19,800.4 ms |   563.58 ms |  87.21 ms |  400000.0000 |   1000.0000 |         - |  595.89 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 | 11,687.0 ms |   341.80 ms |  52.90 ms | 2789000.0000 |   1000.0000 |         - | 4171.61 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |  2,682.2 ms |   206.02 ms |  31.88 ms |   12000.0000 |           - |         - |   19.15 MB |
-|                          'Save vertical XLSX report from entities to file' |   100000 | 54,183.0 ms |   838.40 ms | 129.70 ms | 4424000.0000 | 552000.0000 | 5000.0000 | 7770.61 MB |
+|                          'Save vertical XLSX report from entities to file' |   100000 | 37,784.3 ms |   326.80 ms |  50.57 ms |  439000.0000 |  19000.0000 | 5000.0000 | 1451.92 MB |
-|                        'Save vertical XLSX report from entities to stream' |   100000 | 53,141.0 ms |   840.50 ms | 130.10 ms | 4439000.0000 | 545000.0000 | 5000.0000 | 7739.73 MB |
+|                        'Save vertical XLSX report from entities to stream' |   100000 | 36,633.3 ms | 1,122.15 ms | 173.65 ms |  439000.0000 |  19000.0000 | 5000.0000 | 1421.11 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 | 17,106.0 ms |   346.20 ms |  53.60 ms | 3414000.0000 |   1000.0000 |         - | 5106.72 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  3,632.3 ms |   104.10 ms |  16.11 ms |   45000.0000 |           - |         - |   67.91 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 | 28,679.0 ms | 1,068.10 ms | 165.30 ms | 2590000.0000 |  90000.0000 | 3000.0000 | 9033.37 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  8,752.2 ms |   458.45 ms |  70.95 ms |  236000.0000 |  54000.0000 | 3000.0000 | 1592.78 MB |
-|                       'Save vertical HTML report from data reader to file' |   100000 | 42,110.0 ms | 2,651.00 ms | 410.20 ms | 5604000.0000 |   4000.0000 |         - | 8382.48 MB |
+|                       'Save vertical HTML report from data reader to file' |   100000 | 20,604.8 ms | 1,628.56 ms | 252.02 ms |  413000.0000 |   1000.0000 |         - |  614.96 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 | 13,382.0 ms |   115.80 ms |  17.90 ms | 2799000.0000 |   1000.0000 |         - | 4187.64 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  3,507.2 ms |   167.72 ms |  25.95 ms |   25000.0000 |           - |         - |   38.23 MB |
-|                       'Save vertical XLSX report from data reader to file' |   100000 | 57,466.0 ms | 1,532.70 ms | 237.20 ms | 4438000.0000 | 548000.0000 | 5000.0000 | 7786.63 MB |
+|                       'Save vertical XLSX report from data reader to file' |   100000 | 37,442.0 ms |   728.50 ms | 112.74 ms |  443000.0000 |  19000.0000 | 5000.0000 | 1470.99 MB |
-|                     'Save vertical XLSX report from data reader to stream' |   100000 | 54,954.0 ms |   881.80 ms | 136.50 ms | 4433000.0000 | 555000.0000 | 5000.0000 | 7755.76 MB |
+|                     'Save vertical XLSX report from data reader to stream' |   100000 | 37,588.3 ms |   919.26 ms | 142.26 ms |  443000.0000 |  19000.0000 | 5000.0000 | 1440.22 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 | 14,802.0 ms |   236.00 ms |  36.50 ms | 3380000.0000 |   1000.0000 |         - | 5056.43 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  2,983.9 ms |   347.03 ms |  53.70 ms |   32000.0000 |           - |         - |   48.85 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |   100000 | 25,814.0 ms |   674.80 ms | 104.40 ms | 4129000.0000 | 345000.0000 | 1000.0000 | 8974.98 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  7,600.0 ms |   259.32 ms |  40.13 ms |  213000.0000 |  54000.0000 | 3000.0000 | 1570.29 MB |
-|                                      'Save horizontal HTML report to file' |   100000 | 37,262.0 ms | 2,045.50 ms | 316.50 ms | 5566000.0000 |   3000.0000 |         - | 8320.18 MB |
+|                                      'Save horizontal HTML report to file' |   100000 | 16,421.4 ms |   481.13 ms |  74.46 ms |  412000.0000 |   1000.0000 |         - |  595.79 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 | 11,098.0 ms |   358.60 ms |  55.50 ms | 2766000.0000 |   1000.0000 |         - | 4138.06 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  2,634.7 ms |   364.53 ms |  56.41 ms |   12000.0000 |           - |         - |   19.17 MB |



BenchmarkDotNet v0.13.6, Windows 10 (10.0.19045.3208/22H2/2022Update)
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 6.0.301
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-LKDPGJ : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2

IterationCount=4  RunStrategy=Monitoring  WarmupCount=2  

 |                                                                     Method | RowCount |        Mean |       Error |    StdDev |         Gen0 |        Gen1 |      Gen2 |  Allocated |
 |--------------------------------------------------------------------------- |--------- |------------:|------------:|----------:|-------------:|------------:|----------:|-----------:|
-|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    423.2 ms |   107.74 ms |  16.67 ms |   85000.0000 |           - |         - |  509.01 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    125.7 ms |    25.39 ms |   3.93 ms |            - |           - |         - |    4.96 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    762.2 ms |    88.77 ms |  13.74 ms |  142000.0000 |  14000.0000 |         - |  901.77 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    318.4 ms |   146.60 ms |  22.69 ms |   17000.0000 |   4000.0000 |         - |  157.46 MB |
-|                          'Save vertical HTML report from entities to file' |    10000 |  1,049.0 ms |   577.87 ms |  89.43 ms |  139000.0000 |   1000.0000 |         - |  836.52 MB |
+|                          'Save vertical HTML report from entities to file' |    10000 |    556.6 ms |   145.65 ms |  22.54 ms |    9000.0000 |           - |         - |   59.68 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    335.2 ms |    53.52 ms |   8.28 ms |   69000.0000 |           - |         - |  417.21 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    110.8 ms |    61.33 ms |   9.49 ms |            - |           - |         - |    1.99 MB |
-|                          'Save vertical XLSX report from entities to file' |    10000 |  2,713.7 ms |   266.44 ms |  41.23 ms |  133000.0000 |  37000.0000 | 4000.0000 |  916.19 MB |
+|                          'Save vertical XLSX report from entities to file' |    10000 |  2,475.3 ms |   210.52 ms |  32.58 ms |   28000.0000 |  12000.0000 | 4000.0000 |  288.08 MB |
-|                        'Save vertical XLSX report from entities to stream' |    10000 |  2,680.7 ms |   357.67 ms |  55.35 ms |  132000.0000 |  36000.0000 | 3000.0000 |  907.87 MB |
+|                        'Save vertical XLSX report from entities to stream' |    10000 |  2,474.6 ms |   436.69 ms |  67.58 ms |   27000.0000 |  11000.0000 | 3000.0000 |  279.76 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    484.1 ms |   374.62 ms |  57.97 ms |   85000.0000 |           - |         - |  510.62 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    146.0 ms |   134.06 ms |  20.75 ms |    1000.0000 |           - |         - |    6.86 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |  1,088.0 ms |   971.02 ms | 150.27 ms |  142000.0000 |  14000.0000 |         - |  903.38 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |    335.8 ms |    64.86 ms |  10.04 ms |   19000.0000 |   6000.0000 | 1000.0000 |  159.38 MB |
-|                       'Save vertical HTML report from data reader to file' |    10000 |  1,057.6 ms |   104.24 ms |  16.13 ms |  140000.0000 |   1000.0000 |         - |  838.12 MB |
+|                       'Save vertical HTML report from data reader to file' |    10000 |    584.7 ms |   138.34 ms |  21.41 ms |   10000.0000 |           - |         - |   61.59 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    419.8 ms |   137.79 ms |  21.32 ms |   70000.0000 |           - |         - |  418.81 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    136.3 ms |    42.08 ms |   6.51 ms |            - |           - |         - |    3.90 MB |
-|                       'Save vertical XLSX report from data reader to file' |    10000 |  3,112.9 ms | 2,335.23 ms | 361.38 ms |  133000.0000 |  37000.0000 | 4000.0000 |  917.80 MB |
+|                       'Save vertical XLSX report from data reader to file' |    10000 |  2,567.5 ms |   334.33 ms |  51.74 ms |   29000.0000 |  12000.0000 | 4000.0000 |  289.99 MB |
-|                     'Save vertical XLSX report from data reader to stream' |    10000 |  3,063.2 ms | 1,170.56 ms | 181.15 ms |  132000.0000 |  36000.0000 | 3000.0000 |  909.47 MB |
+|                     'Save vertical XLSX report from data reader to stream' |    10000 |  2,541.5 ms |   213.83 ms |  33.09 ms |   28000.0000 |  11000.0000 | 3000.0000 |  281.67 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    506.9 ms |   187.58 ms |  29.03 ms |   84000.0000 |           - |         - |  505.66 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    155.8 ms |   138.10 ms |  21.37 ms |            - |           - |         - |    4.97 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    876.3 ms |   477.04 ms |  73.82 ms |  141000.0000 |  13000.0000 |         - |  897.53 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    364.8 ms |    72.61 ms |  11.24 ms |   17000.0000 |   5000.0000 |         - |  157.12 MB |
-|                                      'Save horizontal HTML report to file' |    10000 |  1,078.1 ms |   172.61 ms |  26.71 ms |  139000.0000 |   1000.0000 |         - |  832.00 MB |
+|                                      'Save horizontal HTML report to file' |    10000 |    547.0 ms |    73.38 ms |  11.36 ms |   10000.0000 |           - |         - |   59.71 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    452.8 ms |   347.37 ms |  53.76 ms |   69000.0000 |           - |         - |  413.86 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    160.7 ms |   120.28 ms |  18.61 ms |            - |           - |         - |    2.00 MB |
-|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  4,459.5 ms |   863.24 ms | 133.59 ms |  850000.0000 |   4000.0000 |         - | 5089.25 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  1,166.8 ms |   131.51 ms |  20.35 ms |    8000.0000 |           - |         - |   48.83 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  7,534.8 ms | 1,622.32 ms | 251.06 ms | 1426000.0000 |  87000.0000 | 4000.0000 | 9016.70 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  2,949.1 ms |   438.15 ms |  67.80 ms |  181000.0000 |  49000.0000 | 3000.0000 | 1573.70 MB |
-|                          'Save vertical HTML report from entities to file' |   100000 |  9,903.0 ms | 1,127.62 ms | 174.50 ms | 1398000.0000 |  14000.0000 |         - | 8364.09 MB |
+|                          'Save vertical HTML report from entities to file' |   100000 |  4,828.1 ms |   385.89 ms |  59.72 ms |   99000.0000 |           - |         - |  595.90 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |  3,463.7 ms |   677.35 ms | 104.82 ms |  697000.0000 |   2000.0000 |         - | 4171.22 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |    896.6 ms |   115.68 ms |  17.90 ms |    3000.0000 |           - |         - |   19.16 MB |
-|                          'Save vertical XLSX report from entities to file' |   100000 | 17,340.5 ms | 1,303.55 ms | 201.73 ms | 1195000.0000 | 279000.0000 | 5000.0000 | 7770.09 MB |
+|                          'Save vertical XLSX report from entities to file' |   100000 | 13,242.0 ms | 1,940.45 ms | 300.29 ms |  139000.0000 |  19000.0000 | 5000.0000 | 1451.98 MB |
-|                        'Save vertical XLSX report from entities to stream' |   100000 | 17,301.7 ms | 1,193.64 ms | 184.72 ms | 1195000.0000 | 279000.0000 | 5000.0000 | 7739.23 MB |
+|                        'Save vertical XLSX report from entities to stream' |   100000 | 13,275.8 ms |   886.07 ms | 137.12 ms |  139000.0000 |  19000.0000 | 5000.0000 | 1421.21 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  4,756.2 ms |   978.71 ms | 151.46 ms |  853000.0000 |   4000.0000 |         - | 5105.27 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  1,274.8 ms |    91.63 ms |  14.18 ms |   11000.0000 |           - |         - |   67.91 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  8,137.6 ms |   639.34 ms |  98.94 ms | 1429000.0000 |  86000.0000 | 4000.0000 | 9032.72 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  3,094.8 ms |    74.84 ms |  11.58 ms |  184000.0000 |  48000.0000 | 3000.0000 | 1592.78 MB |
-|                       'Save vertical HTML report from data reader to file' |   100000 |  9,929.6 ms |   656.53 ms | 101.60 ms | 1400000.0000 |  14000.0000 |         - | 8380.11 MB |
+|                       'Save vertical HTML report from data reader to file' |   100000 |  5,108.8 ms | 1,440.08 ms | 222.85 ms |  102000.0000 |           - |         - |  614.98 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  3,868.0 ms |   674.24 ms | 104.34 ms |  699000.0000 |   2000.0000 |         - | 4187.24 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  1,104.8 ms |    92.60 ms |  14.33 ms |    6000.0000 |           - |         - |   38.23 MB |
-|                       'Save vertical XLSX report from data reader to file' |   100000 | 17,595.6 ms |   918.50 ms | 142.14 ms | 1197000.0000 | 281000.0000 | 5000.0000 | 7786.12 MB |
+|                       'Save vertical XLSX report from data reader to file' |   100000 | 13,721.3 ms |   688.75 ms | 106.59 ms |  142000.0000 |  17000.0000 | 5000.0000 | 1471.05 MB |
-|                     'Save vertical XLSX report from data reader to stream' |   100000 | 17,591.6 ms |   791.89 ms | 122.55 ms | 1197000.0000 | 281000.0000 | 5000.0000 | 7755.25 MB |
+|                     'Save vertical XLSX report from data reader to stream' |   100000 | 13,946.8 ms | 4,390.81 ms | 679.48 ms |  142000.0000 |  17000.0000 | 5000.0000 | 1440.28 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  4,436.6 ms |   393.07 ms |  60.83 ms |  845000.0000 |   3000.0000 |         - | 5055.52 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  1,207.1 ms |   150.20 ms |  23.24 ms |    8000.0000 |           - |         - |   48.84 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  7,387.0 ms |   383.83 ms |  59.40 ms | 1417000.0000 |  84000.0000 | 2000.0000 | 8974.05 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  3,217.2 ms |   232.86 ms |  36.03 ms |  181000.0000 |  53000.0000 | 3000.0000 | 1570.28 MB |
-|                                      'Save horizontal HTML report to file' |   100000 |  9,450.7 ms |   329.67 ms |  51.02 ms | 1391000.0000 |  10000.0000 |         - | 8318.73 MB |
+|                                      'Save horizontal HTML report to file' |   100000 |  4,938.6 ms |   693.35 ms | 107.30 ms |  101000.0000 |           - |         - |  595.88 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  3,700.9 ms |   365.01 ms |  56.49 ms |  691000.0000 |   1000.0000 |         - | 4137.66 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  1,160.0 ms |   258.88 ms |  40.06 ms |    3000.0000 |           - |         - |   19.17 MB |

```
