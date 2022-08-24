# Change Log

## 0.2.0

Released on: *YYYY-MM-DD*

**Added:**
- Benchmarks for new version (for current project code) and old version (the version specified in `csproj` file as dependency)
- `HtmlStreamWriterDI.AddHtmlStreamWriter` and `HtmlStreamWriterDI.AddHtmlStreamCellWriter` to register Html stream writer in DI
- `IHtmlStreamWriter`/`IHtmlStreamCellWriter` and `HtmlStreamWriter`/`HtmlStreamCellWriter`
- New method `BaseReportCell.Clear` resets report cell to initial state, it should be overriden in derived classes

**Changed:**
- Improved performance and memory consumption
- Aligned versions of XReports.Core and XReports libraries
- Report cells are now cached and reused, so reference to cell should not be persisted anywhere; if copy of cell is needed, one can use `BaseReportCell.Clone` method to create shallow copy
- Properties now are processed by the first property handler that marks them processed, i.e., the property is not processed after it's been marked as processed
- Replaced `BaseReportCell.Value` setter with `BaseReportCell.SetValue<TValue>` method, getter – with `BaseReportCell.GetUnderlyingValue` method
- `IPropertyHandler.Handle` method is now `bool`: it should return `true` if the property has been processed, `false` otherwise
- Renamed classes and interfaces:
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

**Performance and Memory Consumption Benchmarks**

Version 0.1.6

```
BenchmarkDotNet=v0.13.1, OS=gentoo
Intel Core i3-3217U CPU 1.80GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-HKENIE : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

IterationCount=10  RunStrategy=Monitoring

|                                                                     Method | RowCount |     Mean |    Error |   StdDev |        Gen 0 |       Gen 1 |     Gen 2 | Allocated |
|---------------------------------------------------------------------------:|---------:|---------:|---------:|---------:|-------------:|------------:|----------:|----------:|
|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |  1.444 s | 0.0575 s | 0.0380 s |  338000.0000 |           - |         - |    506 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |  2.550 s | 0.0446 s | 0.0295 s |  279000.0000 |  14000.0000 | 2000.0000 |    897 MB |
|                          'Save vertical HTML report from entities to file' |    10000 |  3.827 s | 0.0458 s | 0.0303 s |  556000.0000 |           - |         - |    832 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |  1.169 s | 0.0240 s | 0.0159 s |  277000.0000 |           - |         - |    415 MB |
|                          'Save vertical XLSX report from entities to file' |    10000 |  4.943 s | 0.0394 s | 0.0261 s |  427000.0000 |  65000.0000 | 3000.0000 |    791 MB |
|                        'Save vertical XLSX report from entities to stream' |    10000 |  5.173 s | 0.0574 s | 0.0379 s |  429000.0000 |  68000.0000 | 3000.0000 |    788 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |  1.739 s | 0.0509 s | 0.0337 s |  339000.0000 |           - |         - |    508 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |  2.782 s | 0.0434 s | 0.0287 s |  252000.0000 |  18000.0000 | 2000.0000 |    899 MB |
|                       'Save vertical HTML report from data reader to file' |    10000 |  3.781 s | 0.0531 s | 0.0351 s |  557000.0000 |           - |         - |    834 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |  1.215 s | 0.0538 s | 0.0356 s |  278000.0000 |           - |         - |    416 MB |
|                       'Save vertical XLSX report from data reader to file' |    10000 |  5.090 s | 0.0786 s | 0.0520 s |  427000.0000 |  61000.0000 | 3000.0000 |    793 MB |
|                     'Save vertical XLSX report from data reader to stream' |    10000 |  5.502 s | 0.0634 s | 0.0420 s |  426000.0000 |  61000.0000 | 3000.0000 |    790 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |  1.505 s | 0.2781 s | 0.1840 s |  336000.0000 |           - |         - |    503 MB |
|                'Save horizontal HTML report to string using StringBuilder' |    10000 |  2.618 s | 0.1873 s | 0.1239 s |  413000.0000 |  15000.0000 | 2000.0000 |    891 MB |
|                                      'Save horizontal HTML report to file' |    10000 |  3.358 s | 0.1040 s | 0.0688 s |  553000.0000 |           - |         - |    828 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |  1.116 s | 0.2854 s | 0.1888 s |  274000.0000 |           - |         - |    411 MB |
|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 | 16.054 s | 0.0850 s | 0.0562 s | 3385000.0000 |   1000.0000 |         - |  5,063 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 | 28.347 s | 0.0875 s | 0.0579 s | 2580000.0000 | 138000.0000 | 4000.0000 |  8,962 MB |
|                          'Save vertical HTML report from entities to file' |   100000 | 38.491 s | 0.2332 s | 0.1542 s | 5563000.0000 |   3000.0000 |         - |  8,321 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 | 11.871 s | 0.1093 s | 0.0723 s | 2770000.0000 |   1000.0000 |         - |  4,144 MB |
|                          'Save vertical XLSX report from entities to file' |   100000 | 52.711 s | 0.1482 s | 0.0980 s | 4399000.0000 | 521000.0000 | 5000.0000 |  7,682 MB |
|                        'Save vertical XLSX report from entities to stream' |   100000 | 53.734 s | 0.1740 s | 0.1151 s | 4396000.0000 | 515000.0000 | 5000.0000 |  7,654 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 | 19.313 s | 0.1209 s | 0.0800 s | 3396000.0000 |   1000.0000 |         - |  5,079 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 | 29.039 s | 0.0806 s | 0.0533 s | 2493000.0000 |  87000.0000 | 4000.0000 |  8,982 MB |
|                       'Save vertical HTML report from data reader to file' |   100000 | 40.326 s | 0.1650 s | 0.1092 s | 5573000.0000 |   4000.0000 |         - |  8,335 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 | 14.525 s | 0.0852 s | 0.0564 s | 2781000.0000 |   1000.0000 |         - |  4,160 MB |
|                       'Save vertical XLSX report from data reader to file' |   100000 | 54.652 s | 0.1738 s | 0.1150 s | 4437000.0000 | 556000.0000 | 5000.0000 |  7,698 MB |
|                     'Save vertical XLSX report from data reader to stream' |   100000 | 55.499 s | 0.1961 s | 0.1297 s | 4433000.0000 | 567000.0000 | 5000.0000 |  7,670 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 | 14.979 s | 0.0948 s | 0.0627 s | 3362000.0000 |   1000.0000 |         - |  5,029 MB |
|                'Save horizontal HTML report to string using StringBuilder' |   100000 | 27.131 s | 0.1323 s | 0.0875 s | 4089000.0000 | 157000.0000 | 4000.0000 |  8,922 MB |
|                                      'Save horizontal HTML report to file' |   100000 | 34.452 s | 0.1902 s | 0.1258 s | 5533000.0000 |   3000.0000 |         - |  8,273 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 | 10.318 s | 0.1331 s | 0.0881 s | 2748000.0000 |   1000.0000 |         - |  4,111 MB |
```

Version 0.2.0:

```
BenchmarkDotNet=v0.13.1, OS=gentoo
Intel Core i3-3217U CPU 1.80GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-YKUQZT : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

IterationCount=10  RunStrategy=Monitoring

|                                                                     Method | RowCount |        Mean |     Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|---------------------------------------------------------------------------:|---------:|------------:|----------:|----------:|------------:|-----------:|----------:|----------:|
|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    275.1 ms |  29.77 ms |  19.69 ms |   4000.0000 |          - |         - |      7 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    682.9 ms |  30.27 ms |  20.02 ms |  23000.0000 |  6000.0000 | 1000.0000 |    154 MB |
|                          'Save vertical HTML report from entities to file' |    10000 |  1,749.0 ms |  90.96 ms |  60.16 ms |  38000.0000 |          - |         - |     57 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    194.4 ms |  18.70 ms |  12.37 ms |   1000.0000 |          - |         - |      2 MB |
|                          'Save vertical XLSX report from entities to file' |    10000 |  3,251.5 ms |  80.79 ms |  53.44 ms |  36000.0000 |  5000.0000 | 3000.0000 |    166 MB |
|                        'Save vertical XLSX report from entities to stream' |    10000 |  3,269.7 ms |  48.71 ms |  32.22 ms |  35000.0000 |  5000.0000 | 3000.0000 |    163 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    331.6 ms |  29.65 ms |  19.61 ms |   5000.0000 |          - |         - |      9 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |    769.5 ms |  25.58 ms |  16.92 ms |  29000.0000 |  7000.0000 | 1000.0000 |    156 MB |
|                       'Save vertical HTML report from data reader to file' |    10000 |  1,907.2 ms |  34.20 ms |  22.62 ms |  40000.0000 |          - |         - |     60 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    247.9 ms |  29.45 ms |  19.48 ms |   2000.0000 |          - |         - |      4 MB |
|                       'Save vertical XLSX report from data reader to file' |    10000 |  3,307.2 ms |  92.71 ms |  61.32 ms |  34000.0000 |  6000.0000 | 3000.0000 |    168 MB |
|                     'Save vertical XLSX report from data reader to stream' |    10000 |  3,346.7 ms |  83.24 ms |  55.06 ms |  35000.0000 |  5000.0000 | 3000.0000 |    165 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    255.2 ms |  56.58 ms |  37.42 ms |   4000.0000 |          - |         - |      7 MB |
|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    662.6 ms | 127.10 ms |  84.07 ms |  24000.0000 |  8000.0000 | 2000.0000 |    154 MB |
|                                      'Save horizontal HTML report to file' |    10000 |  1,616.9 ms | 161.16 ms | 106.60 ms |  39000.0000 |          - |         - |     58 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    222.7 ms | 122.28 ms |  80.88 ms |   1000.0000 |          - |         - |      2 MB |
|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  2,600.9 ms |  40.52 ms |  26.80 ms |  43000.0000 |          - |         - |     66 MB |
|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  7,041.8 ms |  67.32 ms |  44.53 ms | 233000.0000 | 55000.0000 | 3000.0000 |  1,542 MB |
|                          'Save vertical HTML report from entities to file' |   100000 | 17,282.6 ms | 174.70 ms | 115.55 ms | 386000.0000 |  1000.0000 |         - |    573 MB |
|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |  2,007.5 ms |  82.10 ms |  54.30 ms |  12000.0000 |          - |         - |     19 MB |
|                          'Save vertical XLSX report from entities to file' |   100000 | 33,660.1 ms | 145.13 ms |  95.99 ms | 451000.0000 | 15000.0000 | 5000.0000 |  1,394 MB |
|                        'Save vertical XLSX report from entities to stream' |   100000 | 33,153.7 ms | 207.60 ms | 137.32 ms | 450000.0000 | 14000.0000 | 5000.0000 |  1,366 MB |
|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  3,292.5 ms |  45.76 ms |  30.27 ms |  56000.0000 |          - |         - |     85 MB |
| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  8,152.7 ms |  82.62 ms |  54.65 ms | 239000.0000 | 54000.0000 | 3000.0000 |  1,558 MB |
|                       'Save vertical HTML report from data reader to file' |   100000 | 18,866.7 ms | 227.72 ms | 150.62 ms | 399000.0000 |  1000.0000 |         - |    593 MB |
|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  2,491.1 ms |  45.00 ms |  29.76 ms |  25000.0000 |          - |         - |     38 MB |
|                       'Save vertical XLSX report from data reader to file' |   100000 | 34,631.0 ms | 219.86 ms | 145.42 ms | 453000.0000 | 18000.0000 | 5000.0000 |  1,413 MB |
|                     'Save vertical XLSX report from data reader to stream' |   100000 | 34,768.2 ms | 148.24 ms |  98.05 ms | 454000.0000 | 16000.0000 | 5000.0000 |  1,385 MB |
|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  2,464.9 ms |  96.09 ms |  63.56 ms |  43000.0000 |          - |         - |     66 MB |
|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  6,724.0 ms |  69.33 ms |  45.86 ms | 222000.0000 | 57000.0000 | 3000.0000 |  1,535 MB |
|                                      'Save horizontal HTML report to file' |   100000 | 15,526.3 ms | 369.03 ms | 244.09 ms | 390000.0000 |  1000.0000 |         - |    572 MB |
|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  1,990.5 ms |  48.77 ms |  32.26 ms |  12000.0000 |          - |         - |     19 MB |
```
