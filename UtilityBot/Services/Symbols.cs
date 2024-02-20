using Telegram.Bot.Types;
using Telegram.Bot;

public class Symbols : ISymbols
{
    private readonly ITelegramBotClient _telegramClient;

    public Symbols(ITelegramBotClient telegramClient)
    {
        _telegramClient = telegramClient;
    }
    public async Task CoutingChars(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: cancellationToken);
        }
    }
}