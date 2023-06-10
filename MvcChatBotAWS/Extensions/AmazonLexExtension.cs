using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using System.Threading.Tasks;

namespace MvcChatBotAWS.Extensions
{
    public static class AmazonLexExtension
    {
        public static async Task<string> ChatBotConnection(this AmazonLexRuntimeV2Client lexClient, string botId, string botAliasId, string localeId, string sessionId, string userInput)
        {
            var request = new RecognizeTextRequest
            {
                BotId = botId,
                BotAliasId = botAliasId,
                LocaleId = localeId,
                SessionId = sessionId,
                Text = userInput
            };

            var response = await lexClient.RecognizeTextAsync(request);
            string respuestaChat = response.Messages[0].Content;
            return respuestaChat;
        }
        public static async Task<string> GetMessages(this AmazonLexRuntimeV2Client lexClient, string botId, string botAliasId, string localeId, string sessionId, string userInput)
        {
            string respuestaChat = await lexClient.ChatBotConnection(botId, botAliasId, localeId, sessionId, userInput);
            return respuestaChat;
        }
    }
}
