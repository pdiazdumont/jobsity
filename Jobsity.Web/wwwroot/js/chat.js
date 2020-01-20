"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

connection.on("NewUserPost", (message) => onNewMessage(message, false));

connection.on("NewBotPost", (message) => onNewMessage(message, true));

const onNewMessage = (message, isBot) => {
    const chatBox = document.getElementById("chat-box");
    chatBox.innerHTML += createChatBubble(message, isBot);

    scrollToLastMessage();
};

connection.start().then(async () => {
    const messages = await makeRequest("/posts", "GET");

    let chatBubbles = "";
    for (let message of messages) {
        chatBubbles += createChatBubble(message);
    }

    const chatBox = document.getElementById("chat-box");
    chatBox.innerHTML = chatBubbles;

    scrollToLastMessage();

    const submit = document.getElementById("message");
    submit.disabled = false;
});

document.addEventListener("DOMContentLoaded", () => {
    const messageBox = document.getElementById("message");

    const chatForm = document.getElementById("chat-form");
    chatForm.addEventListener("submit", async event => {
        event.preventDefault();

        await sendMessage(messageBox.value);
        messageBox.value = "";
    });
});

const sendMessage = async (message) => {
    makeRequest("/posts/new", "POST", {
        text: message
    });
};

const makeRequest = async (url, method = "GET", body) => {
    let request = {
        method,
        credentials: "include",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        }
    };

    if (method.toUpperCase() === "POST") {
        request.body = JSON.stringify(body);
    }

    const response = await fetch(url, request);

    if (response.status === 200) {
        try {
            return await response.json();
        }
        catch (ex) {
            //console.error(ex);
        }
    }
};

const createChatBubble = (message, isBot) => {
    const isCurrentUser = message.userId === window.user.id;
    const date = new Date(message.timestamp);

    const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    const dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

    return `
		<div class="bubble media w-50 mb-3 ${isCurrentUser ? "ml-auto" : ""}" data-message-id=${message.id}>
			<div class="media-body ${isCurrentUser ? "ml-3" : ""}">
				<div class="bg-${isCurrentUser ? "primary" : "light"} rounded py-2 px-3 mb-2">
					<p class="text-small"><b>${isBot ? "Bot" : message.userName}</b></p>
					<p class="text-small mb-0 ${isCurrentUser ? "text-white" : "text-muted"}">${message.text}</p>
				</div>
				<p class="small text-muted">${date.getHours()}:${date.getMinutes()} | ${dayNames[date.getDay()]}, ${monthNames[date.getMonth()]} ${date.getDate()}</p>
			</div>
		</div>
	`;
};

const scrollToLastMessage = () => {
    const lastChatBubble = document.querySelectorAll('#chat-box .bubble:last-child');
    if (lastChatBubble.length > 0) {
        lastChatBubble[0].scrollIntoView();
    }
};