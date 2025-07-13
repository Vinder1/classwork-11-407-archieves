```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.408
  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2


```
| Method                        | Mean           | Error       | StdDev      |
|------------------------------ |---------------:|------------:|------------:|
| BadPriorityQueueAdd           |       469.2 μs |     5.35 μs |     5.00 μs |
| GoodPriorityQueueAdd          |     1,859.7 μs |     8.65 μs |     8.09 μs |
| BadPriorityQueueAddAndRemove  | 2,518,933.5 μs | 5,805.78 μs | 5,146.67 μs |
| GoodPriorityQueueAddAndRemove |    11,379.1 μs |    68.69 μs |    64.25 μs |
| SortedSetAdd                  |    16,469.7 μs |   135.59 μs |   120.20 μs |
| SortedSetAddAndRemove         |    24,252.4 μs |   256.49 μs |   239.92 μs |
