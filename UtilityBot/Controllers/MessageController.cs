using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

public class MessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;

    public MessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":

                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($" Посчитать символы" , $"Символы"),
                        InlineKeyboardButton.WithCallbackData($" Посчитать сумму" , $"Числа")
                    });

                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Бот считает количество символов в тексте и сумму чисел.</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Отправьте текст или числа через пробел для получения результата, но сначала выберите одну из функций бота.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
        }
    }
}