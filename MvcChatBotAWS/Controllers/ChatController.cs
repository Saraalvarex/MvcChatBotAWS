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

        if (userInput.Equals("¿Cuál es el piloto con mas puntos?", StringComparison.OrdinalIgnoreCase))
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://ergast.com/api/f1/current/driverstandings.json";
                HttpResponseMessage response = await client.GetAsync(url);
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
                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    // Procesar los datos JSON de los resultados de la clasificación aquí
                    botMessages.Add(new Assistant()
                    {
                        Id = 0,
                        MsgType = MessageType.BotMessage,
                        //personalizar respuesta
                        ChatMessage = $"El piloto con más puntos es {driverName} con {maxPoints} puntos)"
                    });
                }
                //else if (userInput=="¿Cuál es el equipo con mas puntos?")
                //{

                //}
                else
                {
                    // Manejar el error de la solicitud HTTP aquí
                    botMessages.Add(new Assistant()
                    {
                        Id = 0,
                        MsgType = MessageType.BotMessage,
                        //personalizar respuesta
                        ChatMessage = "nores"
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
                //personalizar respuesta
                ChatMessage = respuestaChat
            });
        }

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
