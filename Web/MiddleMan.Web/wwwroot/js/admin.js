$(document).ready(function () {

    $.ajax({
        url: "/User/GetAdminOffersForApprove",
        type: "GET",
        dataType: 'json',
        success: function (response) {
            $("#admin-link").html("(" + response + ")");
            $("#notifications-count-navbar").html("(" + response + ")");
        }
    });
});
