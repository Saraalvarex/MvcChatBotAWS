// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    loadChatMessages(); // Cargar los mensajes del chat al cargar la página

    $("#sendButton").click(function () {
        var userInput = document.getElementById("userInput").value;

        // Enviar la entrada del usuario al controlador a través de AJAX
        $.ajax({
            type: "POST",
            url: "/Chat/_Chat",
            data: { userInput: userInput },
            success: function (data) {
                // Actualizar la vista con los nuevos mensajes del chat
                updateChatView(data);
            },
            error: function () {
                // Manejar el error de la solicitud AJAX aquí
            }
        });

        // Limpiar el campo de entrada del usuario
        $("#userInput").val("");
    });
});

function loadChatMessages() {
    // Cargar los mensajes del chat al cargar la página
    $.ajax({
        type: "GET",
        url: "/Chat/_Chat",
        success: function (data) {
            // Actualizar la vista con los mensajes del chat
            updateChatView(data);
        },
        error: function () {
            // Manejar el error de la solicitud AJAX aquí
        }
    });
}

function updateChatView(messages) {
    //var chatContainer = $("#chatContainer");
    var chatContainer = document.getElementById("chatContainer");
    // Limpiar el contenedor del chat
/*    chatContainer.empty();*/

    // Recorrer los mensajes y agregarlos al contenedor del chat
    for (var i = 0; i <= messages.length; i++) {
        var message = messages[i];
        var messageHtml = "";
        if (message.msgType == 0) {
            console.log(message.chatMessage);
            // Construir el HTML para el mensaje del usuario
            if (message.chatMessage != null) {
                messageHtml = '<div class="d-flex flex-row p-3 user-mns">' +
                    '<div class="bg-white-chat mr-2 p-3"><span class="text-muted">' + message.chatMessage + '</span></div>' +
                    '<img src="https://img.icons8.com/color/48/000000/circled-user-male-skin-type-7.png" width="30" height="30">' +
                    '</div>';
            }
        } else if (message.msgType == 1) {
            // Construir el HTML para el mensaje del bot
            messageHtml = '<div class="d-flex flex-row p-3">' +
                '<img src="https://img.icons8.com/color/48/000000/circled-user-female-skin-type-7.png" width="30" height="30">' +
                '<div class="chat ml-2 p-3"><span class="text-muted dot">' + message.chatMessage + '</span></div>' +
                '</div>';
        }
        // Agregar el mensaje al contenedor del chat
        chatContainer.innerHTML += messageHtml;
    }
}
