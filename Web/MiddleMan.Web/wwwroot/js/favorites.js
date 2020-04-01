$(document).ready(function () {

    $(".addFavorites").on('click', function () {
        event.stopImmediatePropagation();
        var offerId = $(this).val();
        $.ajax({
            url: "/Favorite/Add",
            type: "GET",
            data: "offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $("#remove-" + offerId).removeClass("invisible");
                $("#add-" + offerId).addClass("invisible");
            }
        });
    });

    $(".removeFavorites").on('click', function
        () {
        event.stopImmediatePropagation();
        var offerId = $(this).val();
        $.ajax({
            url: "/Favorite/Remove",
            type: "GET",
            data: "offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $("#remove-" + offerId).addClass("invisible");
                $("#add-" + offerId).removeClass("invisible");
            }
        });
    });
});