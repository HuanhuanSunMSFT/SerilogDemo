using Serilog;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

public class SerilogAsyncDemo
{
    static void Main()
    {
        Init();

        TestLogAsync().Wait();
    }

    private static async Task TestLogAsync()
    {
        using (LogContext.PushProperty("OperationId", 120))
        {
            Console.WriteLine("TestLogAsync is on thread: " + Thread.CurrentThread.ManagedThreadId);
            Log.Information("TestLogAsync is on thread: " + Thread.CurrentThread.ManagedThreadId);

            await TestLog2Async();
        }
    }

    private static Task TestLog2Async()
    {
        return Task.Run(() =>
        {
            Console.WriteLine("TestLog2Async  is on thread: " + Thread.CurrentThread.ManagedThreadId);
            Log.Logger.Information("TestLog2Async  is on thread: " + Thread.CurrentThread.ManagedThreadId);
        });
    }

    private static void Init()
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .Enrich.WithProperty("MachineName", Environment.MachineName)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:hh:mm:ss} {Level:u3}] {Message,-30:lj} {Properties:j}{NewLine}{Exception}")
        .CreateLogger();
    }

}

