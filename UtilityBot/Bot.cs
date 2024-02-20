using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private MessageController _messageController;
    private KeyboardController _keyboardController;

    public Bot(ITelegramBotClient telegramClient, MessageController messageController, KeyboardController keyboardController)
    {
        _telegramClient = telegramClient;
        _messageController = messageController;
        _keyboardController = keyboardController;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } },
            cancellationToken: stoppingToken);

        Console.WriteLine("Bot started");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _keyboardController.Handle(update.CallbackQuery, update.Message, cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    await _messageController.Handle(update.Message, cancellationToken);
                    return;
            }
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        Console.WriteLine("Waiting 10 seconds before retry");
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}