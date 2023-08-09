# Change Log

## 1.0.0

This is the first stable version. It brings many backward incompatible changes. Here are some highlights:

- performance and memory consumption is improved (refer to [benchmark result](#performance-and-memory-consumption-benchmarks))
- XReports and XReports.Core libraries are now targeting netstandard2.0 and netstandard2.1
- one schema builder that allows building vertical and horizontal reports
- cells are reused by converter and cell providers in order not to create many small objects (which improves both - performance and memory consumption)  
in order for this to work report cell now contains 2 important methods which must be overridden when needed: Clone (clone all collections here) and Reset (for updating the cell to its initial state, clear all collections here)  
- namespaces are revised, now they reflect functionality  
for example, XReports.SchemaBuilders namespace contains all classes that relate to building report schema and not just classes that do the building itself
- registering classes in DI container is revised
- much more tests are added to ensure good code quality and stability
- documentation is completed

### Performance and Memory Consumption Benchmarks

Benchmarks use report with 36 columns. Source code can be found [here](benchmarks/XReports.Benchmarks). Benchmarks for XReports 0.1.6 were run on commit "Update benchmarks for XReport=0.1.6" (hash f95256cdad5c30a5419a839504d21cadab2eaf48). Benchmark results are provided to highlight improvements brought by version 1.0.0 comparing to version 0.1.6. Thanks to [l3r0r0](https://github.com/l3r0r0) for help with the benchmarks.

```diff
-Version 0.1.6
+Version 1.0.0

BenchmarkDotNet v0.13.6, Gentoo Linux
Intel Core i3-3217U CPU 1.80GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores
.NET SDK 6.0.404
  [Host]     : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX
  Job-OJWZPB : .NET 6.0.12 (6.0.1222.56807), X64 RyuJIT AVX

IterationCount=4  RunStrategy=Monitoring  WarmupCount=2

 |                                                                     Method | RowCount |        Mean |       Error |    StdDev |         Gen0 |        Gen1 |      Gen2 |  Allocated |
 |--------------------------------------------------------------------------- |--------- |------------:|------------:|----------:|-------------:|------------:|----------:|-----------:|
-|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |  1,529.0 ms |   133.40 ms |  20.60 ms |  340000.0000 |           - |         - |  509.16 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    310.0 ms |   191.46 ms |  29.63 ms |    3000.0000 |           - |         - |    4.96 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |  2,724.0 ms |   289.00 ms |  44.70 ms |  254000.0000 |  16000.0000 |         - |  901.84 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    765.3 ms |   236.76 ms |  36.64 ms |   23000.0000 |   5000.0000 |         - |  157.46 MB |
-|                          'Save vertical HTML report from entities to file' |    10000 |  4,113.0 ms |   372.00 ms |  57.60 ms |  559000.0000 |           - |         - |  836.75 MB |
+|                          'Save vertical HTML report from entities to file' |    10000 |  1,980.7 ms |   458.10 ms |  70.89 ms |   40000.0000 |           - |         - |   59.67 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |  1,114.0 ms |   231.60 ms |  35.80 ms |  278000.0000 |           - |         - |  417.24 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    272.2 ms |   179.82 ms |  27.83 ms |    1000.0000 |           - |         - |    1.99 MB |
-|                          'Save vertical XLSX report from entities to file' |    10000 |  7,770.0 ms |   158.90 ms |  24.60 ms |  425000.0000 |  69000.0000 | 4000.0000 |  916.25 MB |
+|                          'Save vertical XLSX report from entities to file' |    10000 |  5,906.9 ms |   377.81 ms |  58.47 ms |   30000.0000 |  12000.0000 | 4000.0000 |  288.06 MB |
-|                        'Save vertical XLSX report from entities to stream' |    10000 |  7,773.0 ms |   255.00 ms |  39.50 ms |  424000.0000 |  66000.0000 | 3000.0000 |  907.91 MB |
+|                        'Save vertical XLSX report from entities to stream' |    10000 |  5,874.8 ms |   260.48 ms |  40.31 ms |   28000.0000 |  11000.0000 | 3000.0000 |  279.75 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |  1,771.0 ms |   206.00 ms |  31.90 ms |  341000.0000 |           - |         - |  510.76 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    373.3 ms |   329.94 ms |  51.06 ms |    4000.0000 |           - |         - |    6.87 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |  2,926.0 ms |   347.30 ms |  53.70 ms |  268000.0000 |  15000.0000 |         - |  903.45 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |    844.6 ms |   281.27 ms |  43.53 ms |   24000.0000 |   5000.0000 |         - |  159.37 MB |
-|                       'Save vertical HTML report from data reader to file' |    10000 |  4,198.0 ms |   165.90 ms |  25.70 ms |  560000.0000 |           - |         - |  838.35 MB |
+|                       'Save vertical HTML report from data reader to file' |    10000 |  2,073.7 ms |   326.79 ms |  50.57 ms |   41000.0000 |           - |         - |   61.58 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |  1,368.0 ms |   275.20 ms |  42.60 ms |  280000.0000 |           - |         - |  418.85 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    332.7 ms |   166.92 ms |  25.83 ms |    2000.0000 |           - |         - |    3.90 MB |
-|                       'Save vertical XLSX report from data reader to file' |    10000 |  7,766.0 ms |   231.40 ms |  35.80 ms |  426000.0000 |  63000.0000 | 4000.0000 |  917.85 MB |
+|                       'Save vertical XLSX report from data reader to file' |    10000 |  6,076.2 ms |    96.21 ms |  14.89 ms |   29000.0000 |  12000.0000 | 4000.0000 |  289.98 MB |
-|                     'Save vertical XLSX report from data reader to stream' |    10000 |  8,097.0 ms |   274.80 ms |  42.50 ms |  424000.0000 |  72000.0000 | 3000.0000 |  909.52 MB |
+|                     'Save vertical XLSX report from data reader to stream' |    10000 |  6,089.5 ms |   124.79 ms |  19.31 ms |   28000.0000 |  11000.0000 | 3000.0000 |  281.66 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |  1,418.0 ms |   298.20 ms |  46.20 ms |  338000.0000 |           - |         - |  505.75 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    305.2 ms |   205.04 ms |  31.73 ms |    3000.0000 |           - |         - |    4.97 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |    10000 |  2,628.0 ms |   203.00 ms |  31.40 ms |  410000.0000 |  15000.0000 |         - |  897.63 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    738.3 ms |    75.74 ms |  11.72 ms |   23000.0000 |   5000.0000 |         - |  157.11 MB |
-|                                      'Save horizontal HTML report to file' |    10000 |  3,812.0 ms |   347.60 ms |  53.80 ms |  556000.0000 |           - |         - |  832.14 MB |
+|                                      'Save horizontal HTML report to file' |    10000 |  1,695.6 ms |   272.51 ms |  42.17 ms |   41000.0000 |           - |         - |   59.70 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |  1,047.0 ms |   271.90 ms |  42.10 ms |  276000.0000 |           - |         - |  413.90 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    249.9 ms |    77.78 ms |  12.04 ms |    1000.0000 |           - |         - |    2.00 MB |
-|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 | 14,747.0 ms |   355.70 ms |  55.10 ms | 3403000.0000 |   1000.0000 |         - | 5090.69 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  3,100.7 ms |   191.22 ms |  29.59 ms |   32000.0000 |           - |         - |   48.84 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 | 27,353.0 ms |   432.50 ms |  66.90 ms | 2492000.0000 | 139000.0000 | 2000.0000 | 9017.29 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  7,935.1 ms |   298.53 ms |  46.20 ms |  221000.0000 |  56000.0000 | 3000.0000 | 1573.70 MB |
-|                          'Save vertical HTML report from entities to file' |   100000 | 40,257.0 ms | 1,259.10 ms | 194.90 ms | 5594000.0000 |   4000.0000 |         - | 8366.45 MB |
+|                          'Save vertical HTML report from entities to file' |   100000 | 19,930.7 ms | 1,456.90 ms | 225.46 ms |  401000.0000 |   1000.0000 |         - |  595.90 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 | 11,687.0 ms |   341.80 ms |  52.90 ms | 2789000.0000 |   1000.0000 |         - | 4171.61 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |  2,557.1 ms |   192.92 ms |  29.86 ms |   12000.0000 |           - |         - |   19.16 MB |
-|                          'Save vertical XLSX report from entities to file' |   100000 | 54,183.0 ms |   838.40 ms | 129.70 ms | 4424000.0000 | 552000.0000 | 5000.0000 | 7770.61 MB |
+|                          'Save vertical XLSX report from entities to file' |   100000 | 35,932.1 ms | 1,368.36 ms | 211.76 ms |  441000.0000 |  18000.0000 | 5000.0000 | 1451.89 MB |
-|                        'Save vertical XLSX report from entities to stream' |   100000 | 53,141.0 ms |   840.50 ms | 130.10 ms | 4439000.0000 | 545000.0000 | 5000.0000 | 7739.73 MB |
+|                        'Save vertical XLSX report from entities to stream' |   100000 | 35,847.5 ms | 1,218.97 ms | 188.64 ms |  439000.0000 |  19000.0000 | 5000.0000 | 1421.11 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 | 17,106.0 ms |   346.20 ms |  53.60 ms | 3414000.0000 |   1000.0000 |         - | 5106.72 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  3,640.1 ms |   401.63 ms |  62.15 ms |   45000.0000 |           - |         - |   67.91 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 | 28,679.0 ms | 1,068.10 ms | 165.30 ms | 2590000.0000 |  90000.0000 | 3000.0000 | 9033.37 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  8,490.8 ms |   555.29 ms |  85.93 ms |  231000.0000 |  54000.0000 | 3000.0000 | 1592.78 MB |
-|                       'Save vertical HTML report from data reader to file' |   100000 | 42,110.0 ms | 2,651.00 ms | 410.20 ms | 5604000.0000 |   4000.0000 |         - | 8382.48 MB |
+|                       'Save vertical HTML report from data reader to file' |   100000 | 20,493.5 ms | 1,796.01 ms | 277.93 ms |  413000.0000 |   1000.0000 |         - |  614.98 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 | 13,382.0 ms |   115.80 ms |  17.90 ms | 2799000.0000 |   1000.0000 |         - | 4187.64 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  3,234.6 ms |   389.86 ms |  60.33 ms |   25000.0000 |           - |         - |   38.23 MB |
-|                       'Save vertical XLSX report from data reader to file' |   100000 | 57,466.0 ms | 1,532.70 ms | 237.20 ms | 4438000.0000 | 548000.0000 | 5000.0000 | 7786.63 MB |
+|                       'Save vertical XLSX report from data reader to file' |   100000 | 37,412.4 ms |   877.24 ms | 135.75 ms |  440000.0000 |  18000.0000 | 4000.0000 | 1470.96 MB |
-|                     'Save vertical XLSX report from data reader to stream' |   100000 | 54,954.0 ms |   881.80 ms | 136.50 ms | 4433000.0000 | 555000.0000 | 5000.0000 | 7755.76 MB |
+|                     'Save vertical XLSX report from data reader to stream' |   100000 | 36,618.9 ms |   710.68 ms | 109.98 ms |  444000.0000 |  18000.0000 | 5000.0000 | 1440.23 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 | 14,802.0 ms |   236.00 ms |  36.50 ms | 3380000.0000 |   1000.0000 |         - | 5056.43 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  2,773.8 ms |   384.45 ms |  59.49 ms |   32000.0000 |           - |         - |   48.85 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |   100000 | 25,814.0 ms |   674.80 ms | 104.40 ms | 4129000.0000 | 345000.0000 | 1000.0000 | 8974.98 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  7,405.5 ms |   246.73 ms |  38.18 ms |  216000.0000 |  55000.0000 | 3000.0000 | 1570.29 MB |
-|                                      'Save horizontal HTML report to file' |   100000 | 37,262.0 ms | 2,045.50 ms | 316.50 ms | 5566000.0000 |   3000.0000 |         - | 8320.18 MB |
+|                                      'Save horizontal HTML report to file' |   100000 | 16,573.7 ms | 1,850.10 ms | 286.30 ms |  412000.0000 |   1000.0000 |         - |  595.76 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 | 11,098.0 ms |   358.60 ms |  55.50 ms | 2766000.0000 |   1000.0000 |         - | 4138.06 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  2,503.9 ms |   287.41 ms |  44.48 ms |   12000.0000 |           - |         - |   19.17 MB |



BenchmarkDotNet v0.13.6, Windows 10 (10.0.19045.3208/22H2/2022Update)
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 6.0.301
  [Host]     : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  Job-LKDPGJ : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2

IterationCount=4  RunStrategy=Monitoring  WarmupCount=2  

 |                                                                     Method | RowCount |        Mean |       Error |    StdDev |         Gen0 |        Gen1 |      Gen2 |  Allocated |
 |--------------------------------------------------------------------------- |--------- |------------:|------------:|----------:|-------------:|------------:|----------:|-----------:|
-|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    423.2 ms |   107.74 ms |  16.67 ms |   85000.0000 |           - |         - |  509.01 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |    10000 |    130.8 ms |   118.56 ms |  18.35 ms |            - |           - |         - |    4.96 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    762.2 ms |    88.77 ms |  13.74 ms |  142000.0000 |  14000.0000 |         - |  901.77 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |    10000 |    263.0 ms |    79.84 ms |  12.36 ms |   17000.0000 |   4000.0000 |         - |  157.46 MB |
-|                          'Save vertical HTML report from entities to file' |    10000 |  1,049.0 ms |   577.87 ms |  89.43 ms |  139000.0000 |   1000.0000 |         - |  836.52 MB |
+|                          'Save vertical HTML report from entities to file' |    10000 |    555.3 ms |   355.58 ms |  55.03 ms |   10000.0000 |           - |         - |   59.68 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |    335.2 ms |    53.52 ms |   8.28 ms |   69000.0000 |           - |         - |  417.21 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |    10000 |     86.1 ms |    32.46 ms |   5.02 ms |            - |           - |         - |    1.99 MB |
-|                          'Save vertical XLSX report from entities to file' |    10000 |  2,713.7 ms |   266.44 ms |  41.23 ms |  133000.0000 |  37000.0000 | 4000.0000 |  916.19 MB |
+|                          'Save vertical XLSX report from entities to file' |    10000 |  2,349.9 ms | 1,054.51 ms | 163.19 ms |   28000.0000 |  12000.0000 | 4000.0000 |  288.08 MB |
-|                        'Save vertical XLSX report from entities to stream' |    10000 |  2,680.7 ms |   357.67 ms |  55.35 ms |  132000.0000 |  36000.0000 | 3000.0000 |  907.87 MB |
+|                        'Save vertical XLSX report from entities to stream' |    10000 |  2,330.6 ms | 1,019.74 ms | 157.81 ms |   27000.0000 |  11000.0000 | 3000.0000 |  279.76 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    484.1 ms |   374.62 ms |  57.97 ms |   85000.0000 |           - |         - |  510.62 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |    10000 |    138.2 ms |   206.80 ms |  32.00 ms |    1000.0000 |           - |         - |    6.86 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |  1,088.0 ms |   971.02 ms | 150.27 ms |  142000.0000 |  14000.0000 |         - |  903.38 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |    10000 |    292.8 ms |   191.06 ms |  29.57 ms |   19000.0000 |   6000.0000 | 1000.0000 |  159.38 MB |
-|                       'Save vertical HTML report from data reader to file' |    10000 |  1,057.6 ms |   104.24 ms |  16.13 ms |  140000.0000 |   1000.0000 |         - |  838.12 MB |
+|                       'Save vertical HTML report from data reader to file' |    10000 |    491.7 ms |   295.35 ms |  45.71 ms |   10000.0000 |           - |         - |   61.59 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    419.8 ms |   137.79 ms |  21.32 ms |   70000.0000 |           - |         - |  418.81 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |    10000 |    127.0 ms |    76.82 ms |  11.89 ms |            - |           - |         - |    3.90 MB |
-|                       'Save vertical XLSX report from data reader to file' |    10000 |  3,112.9 ms | 2,335.23 ms | 361.38 ms |  133000.0000 |  37000.0000 | 4000.0000 |  917.80 MB |
+|                       'Save vertical XLSX report from data reader to file' |    10000 |  2,411.6 ms |   888.38 ms | 137.48 ms |   29000.0000 |  12000.0000 | 4000.0000 |  289.99 MB |
-|                     'Save vertical XLSX report from data reader to stream' |    10000 |  3,063.2 ms | 1,170.56 ms | 181.15 ms |  132000.0000 |  36000.0000 | 3000.0000 |  909.47 MB |
+|                     'Save vertical XLSX report from data reader to stream' |    10000 |  2,371.1 ms | 1,182.66 ms | 183.02 ms |   28000.0000 |  11000.0000 | 3000.0000 |  281.67 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    506.9 ms |   187.58 ms |  29.03 ms |   84000.0000 |           - |         - |  505.66 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |    10000 |    114.0 ms |    19.84 ms |   3.07 ms |            - |           - |         - |    4.97 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    876.3 ms |   477.04 ms |  73.82 ms |  141000.0000 |  13000.0000 |         - |  897.53 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |    10000 |    290.9 ms |    49.87 ms |   7.72 ms |   17000.0000 |   5000.0000 |         - |  157.12 MB |
-|                                      'Save horizontal HTML report to file' |    10000 |  1,078.1 ms |   172.61 ms |  26.71 ms |  139000.0000 |   1000.0000 |         - |  832.00 MB |
+|                                      'Save horizontal HTML report to file' |    10000 |    465.6 ms |    22.51 ms |   3.48 ms |   10000.0000 |           - |         - |   59.71 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |    452.8 ms |   347.37 ms |  53.76 ms |   69000.0000 |           - |         - |  413.86 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |    10000 |     97.2 ms |    42.52 ms |   6.58 ms |            - |           - |         - |    2.00 MB |
-|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |  4,459.5 ms |   863.24 ms | 133.59 ms |  850000.0000 |   4000.0000 |         - | 5089.25 MB |
+|     'Enumerate vertical HTML report from entities without saving anywhere' |   100000 |    920.6 ms |    63.32 ms |   9.80 ms |    8000.0000 |           - |         - |   48.83 MB |
-|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  7,534.8 ms | 1,622.32 ms | 251.06 ms | 1426000.0000 |  87000.0000 | 4000.0000 | 9016.70 MB |
+|    'Save vertical HTML report from entities to string using StringBuilder' |   100000 |  2,604.4 ms |   283.15 ms |  43.82 ms |  181000.0000 |  49000.0000 | 3000.0000 | 1573.70 MB |
-|                          'Save vertical HTML report from entities to file' |   100000 |  9,903.0 ms | 1,127.62 ms | 174.50 ms | 1398000.0000 |  14000.0000 |         - | 8364.09 MB |
+|                          'Save vertical HTML report from entities to file' |   100000 |  4,606.8 ms |   389.73 ms |  60.31 ms |   99000.0000 |           - |         - |  595.92 MB |
-|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |  3,463.7 ms |   677.35 ms | 104.82 ms |  697000.0000 |   2000.0000 |         - | 4171.22 MB |
+|     'Enumerate vertical XLSX report from entities without saving anywhere' |   100000 |    818.5 ms |   140.53 ms |  21.75 ms |    3000.0000 |           - |         - |   19.16 MB |
-|                          'Save vertical XLSX report from entities to file' |   100000 | 17,340.5 ms | 1,303.55 ms | 201.73 ms | 1195000.0000 | 279000.0000 | 5000.0000 | 7770.09 MB |
+|                          'Save vertical XLSX report from entities to file' |   100000 | 12,610.5 ms |   700.61 ms | 108.42 ms |  139000.0000 |  19000.0000 | 5000.0000 | 1451.99 MB |
-|                        'Save vertical XLSX report from entities to stream' |   100000 | 17,301.7 ms | 1,193.64 ms | 184.72 ms | 1195000.0000 | 279000.0000 | 5000.0000 | 7739.23 MB |
+|                        'Save vertical XLSX report from entities to stream' |   100000 | 13,446.6 ms |   769.47 ms | 119.08 ms |  139000.0000 |  19000.0000 | 5000.0000 | 1421.22 MB |
-|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  4,756.2 ms |   978.71 ms | 151.46 ms |  853000.0000 |   4000.0000 |         - | 5105.27 MB |
+|  'Enumerate vertical HTML report from data reader without saving anywhere' |   100000 |  1,246.5 ms |   273.90 ms |  42.39 ms |   11000.0000 |           - |         - |   67.91 MB |
-| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  8,137.6 ms |   639.34 ms |  98.94 ms | 1429000.0000 |  86000.0000 | 4000.0000 | 9032.72 MB |
+| 'Save vertical HTML report from data reader to string using StringBuilder' |   100000 |  3,162.5 ms |   412.79 ms |  63.88 ms |  184000.0000 |  48000.0000 | 3000.0000 | 1592.78 MB |
-|                       'Save vertical HTML report from data reader to file' |   100000 |  9,929.6 ms |   656.53 ms | 101.60 ms | 1400000.0000 |  14000.0000 |         - | 8380.11 MB |
+|                       'Save vertical HTML report from data reader to file' |   100000 |  5,377.2 ms |   295.50 ms |  45.73 ms |  102000.0000 |           - |         - |  614.98 MB |
-|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  3,868.0 ms |   674.24 ms | 104.34 ms |  699000.0000 |   2000.0000 |         - | 4187.24 MB |
+|  'Enumerate vertical XLSX report from data reader without saving anywhere' |   100000 |  1,047.7 ms |   117.35 ms |  18.15 ms |    6000.0000 |           - |         - |   38.23 MB |
-|                       'Save vertical XLSX report from data reader to file' |   100000 | 17,595.6 ms |   918.50 ms | 142.14 ms | 1197000.0000 | 281000.0000 | 5000.0000 | 7786.12 MB |
+|                       'Save vertical XLSX report from data reader to file' |   100000 | 14,057.1 ms |   851.16 ms | 131.72 ms |  142000.0000 |  17000.0000 | 5000.0000 | 1471.06 MB |
-|                     'Save vertical XLSX report from data reader to stream' |   100000 | 17,591.6 ms |   791.89 ms | 122.55 ms | 1197000.0000 | 281000.0000 | 5000.0000 | 7755.25 MB |
+|                     'Save vertical XLSX report from data reader to stream' |   100000 | 14,103.8 ms | 1,873.73 ms | 289.96 ms |  142000.0000 |  17000.0000 | 5000.0000 | 1440.27 MB |
-|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  4,436.6 ms |   393.07 ms |  60.83 ms |  845000.0000 |   3000.0000 |         - | 5055.52 MB |
+|                 'Enumerate horizontal HTML report without saving anywhere' |   100000 |  1,174.3 ms |   149.57 ms |  23.15 ms |    8000.0000 |           - |         - |   48.85 MB |
-|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  7,387.0 ms |   383.83 ms |  59.40 ms | 1417000.0000 |  84000.0000 | 2000.0000 | 8974.05 MB |
+|                'Save horizontal HTML report to string using StringBuilder' |   100000 |  3,165.9 ms |   195.30 ms |  30.22 ms |  181000.0000 |  54000.0000 | 3000.0000 | 1570.29 MB |
-|                                      'Save horizontal HTML report to file' |   100000 |  9,450.7 ms |   329.67 ms |  51.02 ms | 1391000.0000 |  10000.0000 |         - | 8318.73 MB |
+|                                      'Save horizontal HTML report to file' |   100000 |  5,375.6 ms |   952.86 ms | 147.46 ms |  101000.0000 |           - |         - |  595.88 MB |
-|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |  3,700.9 ms |   365.01 ms |  56.49 ms |  691000.0000 |   1000.0000 |         - | 4137.66 MB |
+|                 'Enumerate horizontal XLSX report without saving anywhere' |   100000 |    951.0 ms |   374.46 ms |  57.95 ms |    3000.0000 |           - |         - |   19.17 MB |

```
