setup = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/message")
        .build();

    connection.on("SendMessage",
        (message) => {
            let allMessages = document.getElementById("messages");
        });

    connection.start()
        .catch(err => console.error(err.toString()));

};
setup();