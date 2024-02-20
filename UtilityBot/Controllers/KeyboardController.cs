using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System;

public class KeyboardController
{
    private readonly ITelegramBotClient _telegramClient;

    public KeyboardController(ITelegramBotClient telegramBotClient)
    {
        _telegramClient = telegramBotClient;
    }

    public async Task Handle(CallbackQuery? callbackQuery, Message message, CancellationToken ct)
    {
        if (callbackQuery?.Data == null)
            return;

        string Function = callbackQuery.Data switch
        {
            "quantity" => "Количество символов",
            "sum" => "Сумма чисел",
            _ => String.Empty
        };

        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Функция - {Function}.", cancellationToken: ct, parseMode: ParseMode.Html);

        Functions func = new Functions();
        if (callbackQuery?.Data == "Количество символов")
        {
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"В вашем сообщении: {message.Text.Length} символов", cancellationToken: ct);
        }
        else if (callbackQuery?.Data == "Сумма чисел")
        {
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Введите числа через пробел", cancellationToken: ct);

            string str = func.SplitNumbers(message.Text);
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"{str}", cancellationToken: ct);
        }
    }
}
