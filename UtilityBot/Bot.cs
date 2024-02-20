using Microsoft.Extensions.Hosting;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private KeyboardController _keyboardController;
    private MessageController _messageController;
    private MessageController _defaultController;
    private ISymbols _symbolsServices;
    private INumbers _numbersServices;


    public Bot(ITelegramBotClient telegramClient, KeyboardController keyboardController, MessageController messageController, DefaultController defaulController, ISymbols symbolsServices, INumbers numbersServices)
    {
        _telegramClient = telegramClient;
        _keyboardController = keyboardController;
        _messageController = messageController;
        _defaultController = messageController;
        _symbolsServices = symbolsServices;
        _numbersServices = numbersServices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } },
            cancellationToken: stoppingToken);

        Console.WriteLine("Бот запущен");
    }

    public string ActionCommand { get; set; }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            ActionCommand = await _keyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    if (ActionCommand != null)
                    {
                        CommandHandler(botClient, update, cancellationToken, ActionCommand);
                    }
                    await _messageController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _defaultController.Handle(update.Message, cancellationToken);
                    return;
            }
        }
    }

    async Task CommandHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string actionCommand)
    {
        if (update.Type == UpdateType.Message)
        {
            if (actionCommand == "Посчитать символы")
            {
                await _symbolsServices.CoutingChars(botClient, update, cancellationToken);
            }

            if ((actionCommand == "Посчитать сумму"))
            {
                await _numbersServices.SummingNumbers(botClient, update, cancellationToken);
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
        Console.WriteLine("Ожидание 10 секунд перед перезагрузкой...");
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}