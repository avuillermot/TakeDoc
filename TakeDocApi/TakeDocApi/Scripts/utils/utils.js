utils = {
    setStateInputField: function (container) {
        var elems = $("#" + container + " [mandatory='true']");
        $("#" + container + " div.has-error").removeClass("has-error");

        var i = 0;
        $.each(elems, function (index, value) {
            if (value.value.replace("? object:null ?","") == "") {
                $("#div" + value.id).addClass("has-error");
                i++;
            }
        });
        return !(i > 0);
    }
}

Array.prototype.clear = function () {
    while (this.length) {
        this.pop();
    }
};