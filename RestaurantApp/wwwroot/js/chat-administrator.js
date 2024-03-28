"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, date) {
	appendMessage(`${user}: ${message}`, user === "You" ? 'sent' : 'received', date);
});

connection.start().then(function () {
	document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
	return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
	var message = document.getElementById("messageInput").value;
	var userId = document.getElementById("userId").value;
	connection.invoke("SendMessageToUser", userId, message).catch(function (err) {
		return console.error(err.toString());
	});
	document.getElementById("messageInput").value = '';
	event.preventDefault();
});

function appendMessage(text, type, date) {

	var messagesList = document.getElementById("messagesList");
	var msgDiv = document.createElement("li");
	msgDiv.classList.add("message", type);

	var contentDiv = document.createElement("div");
	contentDiv.textContent = text;

	var timestampDiv = document.createElement("div");
	timestampDiv.classList.add("timestamp");
	timestampDiv.textContent = date;

	msgDiv.appendChild(contentDiv);
	msgDiv.appendChild(timestampDiv);

	messagesList.appendChild(msgDiv);

	messagesList.scrollTop = messagesList.scrollHeight;
}