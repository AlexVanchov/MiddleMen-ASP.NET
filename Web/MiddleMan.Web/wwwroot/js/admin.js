$(document).ready(function () {

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
});
