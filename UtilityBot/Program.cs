using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;

static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();

        Console.WriteLine("Starting Service");
        await host.RunAsync();
        Console.WriteLine("Service stopped");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7141051558:AAGU1MUYda-Up7U2dz46qSsFXjfY27fC5KI"));
        services.AddHostedService<Bot>();
        services.AddTransient<MessageController>();
        services.AddTransient<KeyboardController>();
        services.AddTransient<Functions>();
    }
}