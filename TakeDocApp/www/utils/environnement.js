var environnement = {
    UrlBase: "http://localhost/TakeDocApi/",
    //UrlBase: "http://192.168.0.10/TakeDocApi/",
    tokenAuthentification: null,
};

var requestHelper = {
    beforeSend: function() {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", environnement.tokenAuthentification);
        }
    }
};

