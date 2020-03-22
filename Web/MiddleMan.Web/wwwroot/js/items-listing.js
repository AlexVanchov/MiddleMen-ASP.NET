// Config for pages
var maxOffersOnPage = 4;


var page = 1;
var offers = document.getElementsByClassName('offer').length;
var maxPage = Math.round(offers / maxOffersOnPage);
console.log(maxPage);

var onpage = 1;
for (var i = 0; i < offers; i += maxOffersOnPage) {
    if (i === 0) {
        $("#wrap > div").slice(0, maxOffersOnPage).addClass('page1');
        onpage++;
    }
    else {
        var className = 'page' + onpage;
        var to = i + maxOffersOnPage;
        $("#wrap > div").slice(i, to).addClass(className).hide();
        console.log(i + ', ' + to);
        onpage++;
    }
}


$('.next').on('click', function () {
    if (page < maxPage) {
        $("#wrap > div:visible").hide();
        $('.page' + ++page).show();
    }
});
$('.prev').on('click', function () {
    if (page > 1) {
        $("#wrap > div:visible").hide();
        $('.page' + --page).show();
    }
});