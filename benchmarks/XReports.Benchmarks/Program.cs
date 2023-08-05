using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using XReports.Benchmarks;

Job job = Job.Default
    .WithStrategy(RunStrategy.Monitoring)
    .WithWarmupCount(2)
    .WithIterationCount(4)
    .AsDefault();
IConfig config = DefaultConfig.Instance.AddJob(job);
BenchmarkRunner.Run<Benchmarks>(config, args);
