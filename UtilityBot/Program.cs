using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;

public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();

        Console.WriteLine("Сервис запущен");
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(BuildAppSettings());

        services.AddTransient<KeyboardController>();
        services.AddTransient<MessageController>();
        services.AddTransient<DefaultController>();
        services.AddTransient<ISymbols, Symbols>();
        services.AddTransient<INumbers, Numbers>();

        services.AddSingleton<IStorage, MemoryStorage>();
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        services.AddHostedService<Bot>();
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "TOKEN"
        };
    }
}