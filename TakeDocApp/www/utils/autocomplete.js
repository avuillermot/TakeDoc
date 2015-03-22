function autocomplete() {

}

autocomplete.get = function (entityId, userId, value, url, success, error) {
    if (value == "") {
        success(null);
        return;
    }
    url = url.toUpperCase().replace("<ENTITYID/>", entityId);
    url = url.toUpperCase().replace("<USERID/>", userId);
    url = url.toUpperCase().replace("<VALUE/>", value);
    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + url,
        success: function () {
            success(arguments[0]);
        },
        error: function () {
            if (arguments.length >= 2) error(arguments[2]);
            else error("Une erreur inconnue est survenue");
        }
    });
}