using Amazon.Lambda.Model;
using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using Amazon.LexRuntimeV2.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Mvc;
using MvcChatBotAWS.Models;
using MvcChatBotAWS.Extensions;
using System.Net;
using Newtonsoft.Json.Linq;

public class ChatController : Controller
{
    private readonly AmazonLexRuntimeV2Client lexClient;
    public static List<Assistant> botMessages; // Declarar la lista como estática

    public ChatController()
    {
        lexClient = new AmazonLexRuntimeV2Client(); // Asegúrate de configurar las credenciales adecuadas
    }
    public IActionResult Prueba()
    {
        return View();
    }

    //EJEMPLOS PREGUNTAS CHATBOT:
    //Mas vueltas
    //Mas curvas
    //Mas larga/mayor distancia
    //Piloto o equipo con mas puntos
    //La mas dificil es Monaco
    //QUESTION: ¿Cuál es el circuito más complicado de F1?
    public IActionResult _Chat()
    {
        return Json(botMessages);
    }

    [HttpPost]
    public async Task<IActionResult> _Chat(string userInput)
    {
        // Lógica para procesar la entrada del usuario y obtener la respuesta del bot
        if(botMessages == null)
        {
            botMessages = new List<Assistant>();
        }

        botMessages.Add(new Assistant()
        {
            MsgType = MessageType.UserMessage,
            ChatMessage = userInput
        });

        string respuestaChat = await lexClient.GetMessages("PDQFFBWOFP", "WBACPFOPCQ", "es_ES", "163y68h", userInput);

        if (userInput.Equals("¿Cuál es el piloto con mas puntos?", StringComparison.OrdinalIgnoreCase))
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://ergast.com/api/f1/current/driverstandings.json";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON
                    JObject json = JObject.Parse(responseBody);

                    // Obtener la lista de pilotos
                    List<JToken> driverStandings = json["MRData"]["StandingsTable"]["StandingsLists"][0]["DriverStandings"].Children().ToList();

                    // Encontrar el piloto con más puntos
                    int maxPoints = 0;
                    string driverName = "";

                    foreach (JToken driver in driverStandings)
                    {
                        int points = int.Parse(driver["points"].ToString());
                        if (points > maxPoints)
                        {
                            maxPoints = points;
                            driverName = driver["Driver"]["givenName"].ToString() + " " + driver["Driver"]["familyName"].ToString();
                        }
                    }

                    botMessages.Add(new Assistant()
                    {
                        Id = 0,
                        MsgType = MessageType.BotMessage,
                        ChatMessage = $"El piloto con más puntos es {driverName} con {maxPoints} puntos"
                    });
                }
                else
                {
                    // Manejar el error de la solicitud HTTP aquí
                    botMessages.Add(new Assistant()
                    {
                        Id = 0,
                        MsgType = MessageType.BotMessage,
                        ChatMessage = "Ha ocurrido un error al obtener la información de los pilotos."
                    });
                }
            }
        }
        else
        {
            botMessages.Add(new Assistant()
            {
                Id = 0,
                MsgType = MessageType.BotMessage,
                ChatMessage = respuestaChat
            });
        }
        return Json(botMessages);
    }

    //public IActionResult _Chat()
    //{
    //    return PartialView("_Chat");
    //}

    //[HttpPost]
    //public async Task<IActionResult> _Chat(string userInput)
    //{
    //    if (botMessages == null)
    //    {
    //        botMessages = new List<Assistant>();
    //    }

    //    botMessages.Add(new Assistant()
    //    {
    //        MsgType = MessageType.UserMessage,
    //        ChatMessage = userInput
    //    });

    //    string respuestaChat = await lexClient.GetMessages("PDQFFBWOFP", "WBACPFOPCQ", "es_ES", "163y68h", userInput);

    //    if (userInput.Equals("¿Cuál es el piloto con mas puntos?", StringComparison.OrdinalIgnoreCase))
    //    {
    //        using (HttpClient client = new HttpClient())
    //        {
    //            string url = "http://ergast.com/api/f1/current/driverstandings.json";
    //            HttpResponseMessage response = await client.GetAsync(url);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                string responseBody = await response.Content.ReadAsStringAsync();

    //                // Deserializar la respuesta JSON
    //                JObject json = JObject.Parse(responseBody);

    //                // Obtener la lista de pilotos
    //                List<JToken> driverStandings = json["MRData"]["StandingsTable"]["StandingsLists"][0]["DriverStandings"].Children().ToList();

    //                // Encontrar el piloto con más puntos
    //                int maxPoints = 0;
    //                string driverName = "";

    //                foreach (JToken driver in driverStandings)
    //                {
    //                    int points = int.Parse(driver["points"].ToString());
    //                    if (points > maxPoints)
    //                    {
    //                        maxPoints = points;
    //                        driverName = driver["Driver"]["givenName"].ToString() + " " + driver["Driver"]["familyName"].ToString();
    //                    }
    //                }

    //                botMessages.Add(new Assistant()
    //                {
    //                    Id = 0,
    //                    MsgType = MessageType.BotMessage,
    //                    ChatMessage = $"El piloto con más puntos es {driverName} con {maxPoints} puntos"
    //                });
    //            }
    //            else
    //            {
    //                // Manejar el error de la solicitud HTTP aquí
    //                botMessages.Add(new Assistant()
    //                {
    //                    Id = 0,
    //                    MsgType = MessageType.BotMessage,
    //                    ChatMessage = "Ha ocurrido un error al obtener la información de los pilotos."
    //                });
    //            }
    //        }
    //    }
    //    else
    //    {
    //        botMessages.Add(new Assistant()
    //        {
    //            Id = 0,
    //            MsgType = MessageType.BotMessage,
    //            ChatMessage = respuestaChat
    //        });
    //    }

    //    return PartialView("_Chat", botMessages);
    //}

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
