using Benchmark;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Each>();
BenchmarkRunner.Run<Concurrently>();
