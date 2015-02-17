function questionChainItem() {
    this.questions = new Array();
    this.previous = null;
    this.next = null;
}

function questionChainItemService() {
}

questionChainItemService.get = function (chainId, entity) {
    var retour = new questionChainItem();
    var success = function () {
        var data = arguments[0];
        $.each(data, function (index, value) {
            var newItem = null;
            if (value.typeData == "checkbox"){
                newItem = questionService.createCheckbox("title", "description", "label", "observation", "typeData");
            }
            else if (value.typeData == "text") {
                newItem = questionService.createText("title", "description", "label", "textValue", "mandatory", "observation", "typeData");
            }
            retour.previous = "before";
            retour.next = "next";
            retour.questions.push(newItem);
        });
    };

    var error = function () {
    };
    return retour;
}
