$(function () {

    var chatRoom = $.connection.chatRoom;

    initClientEvents(chatRoom);

    // Set initial focus to message input box.
    $('#messageText').focus();

    $.connection.hub.start().done(function () {
        chatRoom.server.connect(username);

        // Send button on click event
        $('#sendButton').click(function () {

            var message = $('#messageText').val();

            if (message) {
                // Call the processInput method on the hub.
                chatRoom.server.processInput(username, message);
            }

            // Clear text box and reset focus for next comment.
            $('#messageText').val('').focus();
        });

        // Send message also with enter key press event
        $("#messageText").keypress(function (e) {
            if (e.which == 13) {
                $('#sendButton').click();
            }
        });
    });
});

function initClientEvents(hub) {

    // Refresh user screen
    hub.client.refreshScreen = function (userName, allUsers, messages) {

        // Update connected users list
        allUsers.forEach(function (user) {
            var node = document.createElement("li");
            node.id = "user_" + user.ConnectionId;
            node.classList.add("list-group-item");
            if (user.UserName == userName) {
                node.classList.add("list-group-item-success");
            }
            var textnode = document.createTextNode(user.UserName);
            node.appendChild(textnode);
            document.getElementById("usersList").appendChild(node);
        });

        // Add all messages to chat
        for (i = 0; i < messages.length; i++) {
            writeMessageToScreen(messages[i].Owner, messages[i].TextMessage, "", messages[i].Time);
        }
    }

    // Add new user to the users list
    hub.client.notifyNewUserConnected = function (id, name) {
        var node = document.createElement("li");
        node.id = "user_" + id;
        node.classList.add("list-group-item");
        var textnode = document.createTextNode(name);
        node.appendChild(textnode);
        document.getElementById("usersList").appendChild(node);
    }

    // Removes user from the list
    hub.client.notifyUserDisconnected = function (id) {
        var ctrId = 'user_' + id;
        $('#' + ctrId).remove();
    }

    // Prints a message to the screen
    hub.client.printMessage = function (userName, message, msgtype, time) {
        writeMessageToScreen(userName, message, msgtype, time);
    }
}

// Add a message to the chat window.
function writeMessageToScreen(userName, message, msgtype, time) {

    var additionalStyle = "";
    var text = "";

    if (msgtype == "command") {
        additionalStyle = "alert-danger";
        text = '[' + time + '] <strong>Command</strong>: ' + htmlEncode(message);
    } else if (msgtype == "result") {
        additionalStyle = "alert-danger";
        text = '[' + time + '] <strong>' + htmlEncode(userName) + '</strong> says: ' + htmlEncode(message);
    }
    else {
        additionalStyle = "alert-info";
        text = '[' + time + '] <strong>' + htmlEncode(userName) + '</strong> says: ' + htmlEncode(message);
    }

    $('#chat-messages-box').append('<div class="messageText alert ' + additionalStyle + '">' + text + '</div>');

    // Scroll chat window to bottom
}

// Html-encode messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}