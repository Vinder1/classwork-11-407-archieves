```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.409
  [Host]     : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2


```
| Method                           | Mean       | Error     | StdDev    |
|--------------------------------- |-----------:|----------:|----------:|
| MyPriorityQueueAdd               |  30.473 ms | 0.4190 ms | 0.3919 ms |
| BuiltInPriorityQueueAdd          |   4.854 ms | 0.0807 ms | 0.0755 ms |
| MyPriorityQueueAddAndRemove      | 115.580 ms | 2.0298 ms | 1.8986 ms |
| BuiltInPriorityQueueAddAndRemove |  35.730 ms | 0.2781 ms | 0.2601 ms |
