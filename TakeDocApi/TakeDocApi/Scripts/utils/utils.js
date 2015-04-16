utils = {
    setStateInputField: function (divId) {
        var elems = $("#" + divId + " input[mandatory='true']");
        $("#" + divId + " div.has-error").removeClass("has-error");

        var i = 0;
        $.each(elems, function (index, value) {
            if (value.value == "") {
                $("#div" + value.id).addClass("has-error");
                i++;
            }
        });
        return !(i > 0);
    } 
}