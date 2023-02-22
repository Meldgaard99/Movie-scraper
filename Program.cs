
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("6252326329:AAFib-vrENRuwdL9SISmbixTMveGcSiGyuA");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n hehe" + messageText,
        cancellationToken: cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}






static async Task Main(string[] args)
{
    HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
    HtmlAgilityPack.HtmlDocument doc = web.Load("https://yts.mx/");
    var nodes = doc.DocumentNode.SelectNodes("//a[@class='browse-movie-title']");
    string message = "";
    for (int i = 0; i < 4 && i < nodes.Count; i++)
    {
        var item = nodes[i];
        message += $"{i + 1}. {item.InnerText}\n";
    }

    var botClient = new TelegramBotClient("6252326329:AAFib-vrENRuwdL9SISmbixTMveGcSiGyuA");
    await botClient.SendTextMessageAsync(chatId: "5717281789"  + "", text: message);


}




