namespace MvcChatBotAWS.Models
{
    public class Assistant
    {
        //0 -> UserMessage 
        //1 -> BotMessage
        public int Id { get; set; }
        public MessageType MsgType { get; set; }
        public string ChatMessage { get; set; }
    }

    public enum MessageType
    {
        UserMessage,
        BotMessage
    }
}
