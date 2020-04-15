// input form in create offer page
var btn = document.getElementById("submit-btn");
btn.addEventListener("click", (e) => {
    e.preventDefault();

    var img = document.getElementById("image-input").files[0];

    if (img === undefined) {
        document.getElementById("create-form").submit();
    }
    else if (img.size >= 1048576) {
        alert("Image is too big");
    }
    else {
        document.getElementById("create-form").submit();
    }
});