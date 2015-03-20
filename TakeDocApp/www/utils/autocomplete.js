function autocomplete() {

}

autocomplete.get = function (value, url, success, error) {
    if (value == "") {
        success(null);
        return;
    }
    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + url + value,
        success: function () {
            success(arguments[0]);
        },
        error: function () {
            if (arguments.length >= 2) error(arguments[2]);
            else error("Une erreur inconnue est survenue");
        }
    });
}