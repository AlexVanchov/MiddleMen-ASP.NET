$(document).ready(function () {
    $("#sortable").sortable();
});

var editBtns = document.getElementsByClassName("edit-btn");

for (var editBtn of editBtns) {
    editBtn.addEventListener("click", (e) => {
        e.preventDefault();
        var currentBtn = e.currentTarget;
        var btnId = currentBtn.id;
        var id = btnId.replace("edit-btn-", "");

        var inputZone = document.getElementById(id);
        //var editInfoInput = document.getElementById("edit-info");
        var nameOffer = document.getElementById("nameOffer-"+id).innerText.replace(" - Offers:", "");

        var input = document.createElement("input");
        input.setAttribute("id", "save-name-" + id);
        input.setAttribute("value", nameOffer);

        inputZone.innerHTML = " ";
        inputZone.appendChild(input);

        document.getElementById("edit-btn-" + id).style.display="none";
        document.getElementById("save-btn-" + id).style.display = "inline-block";
        document.getElementById("save-btn-" + id)
            .addEventListener("click", (e) => {
                e.preventDefault();
                var currentBtn = e.currentTarget;
                var btnId = currentBtn.id;
                var id = btnId.replace("save-btn-", "");

                var input = document.getElementById("save-name-" + id).value;
                document.getElementById("save-btn-" + id).style.display = "none";
                document.getElementById("edit-btn-" + id).style.display = "inline-block";

                event.stopImmediatePropagation();
                $.ajax({
                    url: "/Api/EditCategory",
                    type: "GET",
                    data: "categoryId=" + id + "&newName=" + input,
                    dataType: 'json',
                    success: function (response) {
                        document.getElementById(id).innerHTML = `<span id="nameOffer-` + id + `" value="` + id + `">` + input + ` - Offers:</span>
                    <span class="badge">`+ response + `</span>`;
                    }
                });
        });
    });
}


var deleteBtns = document.getElementsByClassName("delete-btn");

for (var deleteBtn of deleteBtns) {
    deleteBtn.addEventListener("click", (e) => {
        e.preventDefault();
        var currentBtn = e.currentTarget;
        var deleteBtn = currentBtn.id;
        var id = deleteBtn.replace("delete-btn-", "");
        var category = document.getElementById("remove-category-" + id).style.display = "none";

        event.stopImmediatePropagation();
        $.ajax({
            url: "/Api/DeleteCategory",
            type: "GET",
            data: "categoryId=" + id,
            dataType: 'json',
            success: function (response) {

            }
        });
    });
}

document.getElementById("save-order-btn").addEventListener("click", () => {
    var orders = [];

    var categories = document.getElementById("sortable").getElementsByTagName("li");

    for (var category of categories) {
        var possition = category.value;
        orders.push(possition);
    }

    event.stopImmediatePropagation();
    $.ajax({
        url: "/Api/ReorderCategories",
        type: "GET",
        data: "order="+ orders.toString(),
        dataType: 'json',
        success: function (response) {
            if (response) {
                alert("Success");
            }
        }
    });
});
