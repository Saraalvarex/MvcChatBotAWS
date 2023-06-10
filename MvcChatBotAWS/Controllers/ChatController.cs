using Amazon.Lambda.Model;
using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using Amazon.LexRuntimeV2.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Mvc;
using MvcChatBotAWS.Models;
using MvcChatBotAWS.Extensions;
using System.Net;

public class ChatController : Controller
{
    private readonly AmazonLexRuntimeV2Client lexClient;
    public static List<Assistant> botMessages; // Declarar la lista como estática

    public ChatController()
    {
        lexClient = new AmazonLexRuntimeV2Client(); // Asegúrate de configurar las credenciales adecuadas
    }
    //EJEMPLOS PREGUNTAS CHATBOT:
    //Mas vueltas
    //Mas curvas
    //Mas larga/mayor distancia
    //Piloto o equipo con mas puntos
    //La mas dificil es Monaco
    //QUESTION: ¿Cuál es el circuito más complicado de F1?
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string userInput)
    {
        if (botMessages == null)
        {
            botMessages = new List<Assistant>();
        }

        botMessages.Add(new Assistant()
        {
            MsgType = MessageType.UserMessage,
            ChatMessage = userInput
        });

        string respuestaChat = await lexClient.GetMessages("PDQFFBWOFP", "WBACPFOPCQ", "es_ES", "163y68h", userInput);

        botMessages.Add(new Assistant()
        {
            Id = 0,
            MsgType = MessageType.BotMessage,
            ChatMessage = respuestaChat
        });

        return View(botMessages);
    }
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
