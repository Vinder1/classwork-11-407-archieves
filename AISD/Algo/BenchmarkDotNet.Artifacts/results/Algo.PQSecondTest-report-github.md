```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.408
  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2


```
| Method                                  | Mean         | Error      | StdDev     |
|---------------------------------------- |-------------:|-----------:|-----------:|
| MyPriorityQueueAdd1000                  |     24.52 μs |   0.147 μs |   0.130 μs |
| BuiltInPriorityQueueAdd1000             |     11.06 μs |   0.075 μs |   0.071 μs |
| MyPriorityQueueAdd100_000               |  3,605.96 μs |  12.661 μs |  11.843 μs |
| BuiltInPriorityQueueAdd100_000          |  1,535.82 μs |  11.538 μs |  10.793 μs |
| MyPriorityQueueAddAndRemove1000         |    110.86 μs |   0.402 μs |   0.376 μs |
| BuiltInPriorityQueueAddAndRemove1000    |     63.58 μs |   0.669 μs |   0.626 μs |
| MyPriorityQueueAddAndRemove100_000      | 26,065.46 μs | 121.455 μs | 107.667 μs |
| BuiltInPriorityQueueAddAndRemove100_000 | 10,648.50 μs |  55.210 μs |  51.643 μs |
