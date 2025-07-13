```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.409
  [Host]     : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2


```
| Method                        | Mean          | Error      | StdDev     |
|------------------------------ |--------------:|-----------:|-----------:|
| BadPriorityQueueAdd           |      1.680 ms |  0.0198 ms |  0.0175 ms |
| GoodPriorityQueueAdd          |      5.553 ms |  0.0641 ms |  0.0600 ms |
| BadPriorityQueueAddAndRemove  | 22,755.197 ms | 96.7269 ms | 85.7459 ms |
| GoodPriorityQueueAddAndRemove |     35.760 ms |  0.2263 ms |  0.2006 ms |
| SortedSetAdd                  |     63.084 ms |  0.4951 ms |  0.4631 ms |
| SortedSetAddAndRemove         |     86.852 ms |  0.4270 ms |  0.3994 ms |
