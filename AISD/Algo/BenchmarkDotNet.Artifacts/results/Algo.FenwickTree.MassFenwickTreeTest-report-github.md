```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.409
  [Host]     : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2


```
| Method         | Mean     | Error     | StdDev    |
|--------------- |---------:|----------:|----------:|
| SegmentTreeSum | 4.100 ms | 0.0460 ms | 0.0384 ms |
| FenwickTreeSum | 1.720 ms | 0.0325 ms | 0.0362 ms |
