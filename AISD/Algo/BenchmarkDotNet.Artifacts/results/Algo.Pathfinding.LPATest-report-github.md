```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4349)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.411
  [Host]     : .NET 8.0.17 (8.0.1725.26602), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.17 (8.0.1725.26602), X64 RyuJIT AVX2


```
| Method      | N    | Mean       | Error     | StdDev    | Median     |
|------------ |----- |-----------:|----------:|----------:|-----------:|
| **LpaStarTest** | **500**  |   **399.2 ms** |  **24.03 ms** |  **68.96 ms** |   **382.6 ms** |
| AStarTest   | 500  | 1,134.9 ms |  21.48 ms |  24.73 ms | 1,141.0 ms |
| **LpaStarTest** | **700**  |   **942.6 ms** |  **62.86 ms** | **185.34 ms** |   **903.2 ms** |
| AStarTest   | 700  | 1,955.5 ms |  33.70 ms |  31.52 ms | 1,951.5 ms |
| **LpaStarTest** | **1000** | **2,247.3 ms** | **129.61 ms** | **373.95 ms** | **2,218.8 ms** |
| AStarTest   | 1000 | 4,250.0 ms |  84.59 ms | 172.80 ms | 4,164.8 ms |
