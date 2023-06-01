using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using Microsoft.AspNetCore.Mvc;
using System;

public class ChatController: Controller
{
    private readonly AmazonLexRuntimeV2Client lexClient;

    public ChatController()
    {
        lexClient = new AmazonLexRuntimeV2Client(); // Asegúrate de configurar las credenciales adecuadas
    }

    public IActionResult SendMessageToChatBot()
    {
        return View();
    }

    [HttpPost]
    public async Task SendMessageToChatBot(string userInput)
    {
        var request = new RecognizeTextRequest
        {
            BotId = "WBACPFOPCQ",
            BotAliasId = "pruebas",
            LocaleId = "es_ES",
            //SessionId: Es un identificador único que representa una sesión de conversación con el bot.
            //Debes generar un SessionId único para cada usuario o cada vez que inicies una nueva conversación.
            //Puedes generar un SessionId de cualquier manera que sea conveniente para tu aplicación.Por ejemplo,
            //puedes utilizar un identificador único generado por tu aplicación o una combinación de datos del usuario.
            //Es importante que el SessionId sea único para cada sesión de conversación para mantener el seguimiento adecuado
            //del contexto de la conversación.
            SessionId = "163y68h",
            Text = userInput
        };

        //var response = lexClient.RecognizeText(request);
        var response = await lexClient.RecognizeTextAsync(request);

        // Procesa la respuesta del chatbot según sea necesario
        if (response != null && response.Messages.Count > 0)
        {
            foreach (var message in response.Messages)
            {
                Console.WriteLine("Respuesta del chatbot: " + message.Content);
            }
        }
    }
}
