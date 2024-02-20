using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

public class MessageController
{
    private readonly ITelegramBotClient _telegramClient;

    public MessageController(ITelegramBotClient telegramBotClient)
    {
        _telegramClient = telegramBotClient;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        Functions func = new Functions();

        switch (message.Text)
        {
            case "/start":
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($" Количество Символов" , $"quantity"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sum")
                    });

                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $" Бот может подсчитать количество символов или подсчитать сумму чисел.{Environment.NewLine}", cancellationToken: ct, replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                try
                {
                    string str = func.SplitNumbers(message.Text);
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"{str}", cancellationToken: ct);
                }
                catch
                {
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"В вашем сообщении: {message.Text.Length} символов", cancellationToken: ct);
                }
                break;
        }
    }
}
