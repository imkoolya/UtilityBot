using Telegram.Bot.Types;
using Telegram.Bot;

public interface ISymbols
{
    Task CoutingChars(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}