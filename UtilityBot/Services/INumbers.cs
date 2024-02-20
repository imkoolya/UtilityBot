using Telegram.Bot.Types;
using Telegram.Bot;

public interface INumbers
{
    Task SummingNumbers(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}