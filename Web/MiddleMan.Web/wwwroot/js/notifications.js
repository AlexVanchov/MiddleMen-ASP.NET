$(document).ready(function () {
    AdminOffersForApproveCount();
    Favorites();
    UnreadMessages();
});

function AdminOffersForApproveCount() {
    $.ajax({
        url: "/Api/GetAdminOffersForApprove",
        type: "GET",
        dataType: 'json',
        success: function (response) {
            if (response > 0) {
                $("#admin-link").html("(" + response + ")");
                $("#notifications-count-navbar-admin").html("(" + response + ")");
            }
        }
    });
}

function Favorites() {
    $.ajax({
        url: "/Api/GetFavoritesCount",
        type: "GET",
        dataType: 'json',
        success: function (response) {
            if (response > 0) {
                $("#favoritesCount").html(response);
                $("#favoritesCount2").html(response);
            }
            else {
                $("#favoritesCount").html(0);
                $("#favoritesCount2").html(0);
            }
        }
    });
}

function UnreadMessages() {
    $.ajax({
        url: "/Api/GetUnreadMessagesCount",
        type: "GET",
        dataType: 'json',
        success: function (response) {
            if (response > 0) {
                $("#messagesCount").html(response);
                $("#messagesCount2").html(response);
            }
            else {
                $("#messagesCount").html(0);
                $("#messagesCount2").html(0);
            }
        }
    });
}