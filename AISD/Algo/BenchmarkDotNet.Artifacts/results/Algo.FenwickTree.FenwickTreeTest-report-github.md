```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.409
  [Host]     : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2


```
| Method         | Mean      | Error     | StdDev    |
|--------------- |----------:|----------:|----------:|
| SegmentTreeSum |  2.432 ms | 0.0299 ms | 0.0280 ms |
| FenwickTreeSum |  1.061 ms | 0.0212 ms | 0.0188 ms |
| SimpleSum      | 61.852 ms | 0.6295 ms | 0.5888 ms |
