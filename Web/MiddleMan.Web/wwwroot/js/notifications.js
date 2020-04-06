$(document).ready(function () {
    AdminOffersForApproveCount();
    Favorites();
});

function AdminOffersForApproveCount() {
    $.ajax({
        url: "/User/GetAdminOffersForApprove",
        type: "GET",
        dataType: 'json',
        success: function (response) {
            if (response > 0) {
                $("#admin-link").html("(" + response + ")");
                $("#notifications-count-navbar").html("(" + response + ")");
            }
        }
    });
}

function Favorites() {
    $.ajax({
        url: "/Favorites/GetFavoritesCount",
        type: "GET",
        dataType: 'json',
        success: function (response) {
            if (response > 0) {
                $("#favoritesCount").html(response);
            }
        }
    });
}