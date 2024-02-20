using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

public class KeyboardController
{
    private readonly IStorage _memoryStorage;
    private readonly ITelegramBotClient _telegramClient;

    public KeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }

    public async Task<string> Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if (callbackQuery?.Data == null)
            return null;

        _memoryStorage.GetSession(callbackQuery.From.Id).ActionCode = callbackQuery.Data;

        string actionText = callbackQuery.Data switch
        {
            "Символы" => "Посчитать символы",
            "Числа" => "Посчитать сумму",
            _ => String.Empty
        };

        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"<b>Функция бота - {actionText}.{Environment.NewLine}</b>" +
            $"{Environment.NewLine}Поменять можно в меню выбора.", cancellationToken: ct, parseMode: ParseMode.Html);

        return actionText;
    }
}