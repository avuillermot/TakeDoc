var environnement = {
    //UrlBase: "http://localhost/TakeDocApi/",
    UrlBase: "http://dev-takedoc.cloudapp.net/",
    isApp: true,
    tokenAuthentification: null,
};

var requestHelper = {
    beforeSend: function() {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", environnement.tokenAuthentification);
        }
    }
};

