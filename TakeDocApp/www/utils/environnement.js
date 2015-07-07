var environnement = {
    UrlBase: "http://localhost/TakeDocApi/",
    //UrlBase: "http://dev-takedoc.cloudapp.net/",
    isApp: false,
    RefreshToken: null,
    AccessToken: null
};

var requestHelper = {
    beforeSend: function() {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", environnement.AccessToken);
        }
    }
};

