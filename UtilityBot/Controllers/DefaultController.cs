using Telegram.Bot.Types;
using Telegram.Bot;

public class DefaultController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;

    public DefaultController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Данный тип сообщений не поддерживается. Пожалуйста отправьте текст.", cancellationToken: ct);
    }
}