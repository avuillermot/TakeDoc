$.urlParam = function (name) {
    var results = new RegExp('[\?&amp;]' + name + '=([^&amp;#]*)').exec(window.location.href);
    return results[1] || 0;
}

$(document).ready(function () {
    var reference = $.urlParam("ref");
    var url = window.location.protocol + "//" + window.location.hostname + "/takedocapi/identity/activate/" + reference;
    $.ajax({
        type: 'GET',
        url: url,
        success: function () {
            $("#message").html("Votre compte est activé.");
        },
        error: function () {
            $("#message").html(arguments[0].responseText);
        }
    });
});