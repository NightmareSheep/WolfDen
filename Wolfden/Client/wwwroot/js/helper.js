var helper = {};

helper.SetClassOnElement = function (elementId, className){
    var element = document.getElementById(elementId);
    element.classList.add(className);
}

helper.RemoveClassFromElement = function (elementId, className) {
    var element = document.getElementById(elementId);
    element.classList.remove(className);
}