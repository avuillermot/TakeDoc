function autocompleteRequest() {

}

autocompleteRequest.get = function (url, success, error) {

    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + url,
        success: function () {
            // oData return
            if (arguments[0].value != null) {
                success(arguments[0].value);
            }
            else {
                alert("ok");
            }
        },
        error: function () {
            if (arguments.length >= 2) error(arguments[2]);
            else error("Une erreur inconnue est survenue");
        }
    });

}