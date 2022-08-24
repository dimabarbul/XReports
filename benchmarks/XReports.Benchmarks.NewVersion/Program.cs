using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using XReports.Benchmarks.NewVersion;

Job job = Job.Default
    .WithStrategy(RunStrategy.Monitoring)
    .WithIterationCount(1)
    .AsDefault();
IConfig config = DefaultConfig.Instance.AddJob(job);
BenchmarkRunner.Run<Benchmarks>(config, args);
