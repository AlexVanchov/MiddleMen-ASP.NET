setup = () => {
    var connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/details")
            .build();

    connection.on("NewMessage",
        function (message) {

            var chatInfo = `<div class="card-body msg_card_body"><p>`;
            var senderId = $("#senderId").val();

            if (senderId === message.senderId) {
                chatInfo += `<div class="d-flex justify-content-end mb-4">
                        <div class="msg_cotainer_send">
                            ${message.messageContent}/end
                            ${message.sideA.username}
                            <span class="msg_time_send">${message.sentOn}</span>
                        </div>
                        <div class="img_cont_msg">
                            <img src="${message.sideA.profilePicUrl}" class="rounded-circle user_img_msg" />
                        </div>
                    </div>`;
            }
            else {
                chatInfo += `<div class="d-flex justify-content-start mb-4">
                        <div class="img_cont_msg">
                            <img src="${message.sideA.profilePicUrl}" class="rounded-circle user_img_msg">
                        </div>
                        <div class="msg_cotainer">
                            ${message.messageContent}/start
                            ${message.sideA.username}
                            <span class="msg_time">${message.sentOn}</span>
                        </div>
                    </div>`;
            }
            chatInfo += `</p></div>`;
            var messagesList = document.getElementById("messages");
            messagesList.innerHTML += chatInfo;
        });

    $("#sendButton").click(function (data) {
        var content = $("#messageInput").val();
        var offerId = $("#offerId").val();
        var senderId = $("#senderId").val();
        var recipientId = $("#recipientId").val();
        connection.invoke("SendMessage", offerId, senderId, recipientId, content);
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

};
setup();