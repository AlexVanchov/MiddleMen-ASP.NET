// input form in create offer page
var btn = document.getElementById("submit-btn");
btn.addEventListener("click", (e) => {

    var img = document.getElementById("image-input").files[0];

    if (img !== undefined) {
        if (img.size >= 1048576) {
            e.preventDefault();
            alert("Image is too big");
        }
    }
});