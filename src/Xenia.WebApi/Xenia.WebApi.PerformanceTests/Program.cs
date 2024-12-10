using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using Xenia.WebApi.PerformanceTests;

BenchmarkRunner.Run<AvailabilityHandlerPerformanceTests>(DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator));
//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInnew Availability { AvailableRooms = 10, Date = new DateTime(2024, 1, 1), HotelId = Guid.NewGuid(), RoomType = "Single" }ProcessConfig());