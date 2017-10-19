
window.onload = function () {

var canvas = document.getElementById('logo');
var context = canvas.getContext('2d');
var imageObj = new Image();

imageObj.onload = function () {
    context.drawImage(imageObj, 0, 0, 200, 100);
}
imageObj.src = "/Content/images/logo2.png";
};
