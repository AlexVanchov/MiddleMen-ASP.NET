﻿$(document).ready(function () {

    $(".addFavorites").on('click', function () {
        event.stopImmediatePropagation();
        let offerId = $(this).val();
        $.ajax({
            url: "/Favorites/Add",
            type: "GET",
            data: "offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $("#remove-" + offerId).removeClass("invisible");
                $("#add-" + offerId).addClass("invisible");
                Favorites();
            }
        });
    });

    $(".removeFavorites").on('click', function
        () {
        event.stopImmediatePropagation();
        let offerId = $(this).val();
        $.ajax({
            url: "/Favorites/Remove",
            type: "GET",
            data: "offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $("#remove-" + offerId).addClass("invisible");
                $("#add-" + offerId).removeClass("invisible");
                Favorites();
            }
        });
    });
});