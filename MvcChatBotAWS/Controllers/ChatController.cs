using Amazon.Lambda.Model;
using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using Amazon.LexRuntimeV2.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Mvc;
using MvcChatBotAWS.Models;
using System.Net;

public class ChatController : Controller
{
    private readonly AmazonLexRuntimeV2Client lexClient;

    public ChatController()
    {
        lexClient = new AmazonLexRuntimeV2Client(); // Asegúrate de configurar las credenciales adecuadas
    }
    //public IActionResult Index()
    //{
    //    return View();
    //}
    public IActionResult Index()
    {
        return View();
    }
    //EJEMPLOS PREGUNTAS CHATBOT: (Preguntas sencillas y con respuestas cortas)
    //Mas vueltas
    //Mas curvas
    //Mas larga/mayor distancia
    //Piloto o equipo con mas puntos
    //La mas dificil es Monaco
    //QUESTION: ¿Cuál es el circuito más complicado de F1?
    [HttpPost]
    //SendMensajeChatbot y getrespuesta
    public async Task<IActionResult> Index(string userInput)
    {
        //si la lista esta vacia tal y sino tal
        List<Assistant> botMessages = new List<Assistant>();
        //Assistant assistant = new Assistant();
        var request = new RecognizeTextRequest
        {
            BotId = "PDQFFBWOFP",
            BotAliasId = "WBACPFOPCQ",
            LocaleId = "es_ES",
            //SessionId: Es un identificador único que representa una sesión de conversación con el bot.
            //SessionId único para cada usuario o cada vez que se inicie una nueva conversación (IdUser)
            SessionId = "163y68h",
            Text = userInput
        };
        var response = await lexClient.RecognizeTextAsync(request);
        //response.SessionStateValue.DialogAction.SlotToElicit;
        string respuestaChat = response.Messages[0].Content;
        //ViewBag.BOTMENSAJE = respuestaChat;
        //assistant.ChatMessage = respuestaChat;
        botMessages.Add(
            new Assistant()
            {
                Id = 0,
                MsgType = MessageType.BotMessage,
                ChatMessage = respuestaChat
            });
        return View(botMessages);
    }
    #region codigosuelto
    //if (response != null && response.Interpretations != null && response.Interpretations.Count > 0)
    //{
    //    var interpretation = response.Interpretations[0];
    //    //obtengo el nombre de la ranura y el valor asignado si la respuesta es de éxito
    //    if (interpretation.Intent.Name == "OrderFlowers")
    //    {
    //        // Cojo los slots posibles
    //        var slots = interpretation.Intent.Slots;
    //        // Si mi slot es "FlowerType", obtengo su valor
    //        if (slots.ContainsKey("FlowerType"))
    //        {
    //            string slotValue = slots["FlowerType"].Value.InterpretedValue;
    //            ViewBag.A = slotValue;
    //        }
    //    }
    //}
    #endregion
}