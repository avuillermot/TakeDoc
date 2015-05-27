var environnement = {
    //UrlBase: "http://localhost/TakeDocApi/",
    UrlBase: "http://192.168.0.14/TakeDocApi/",
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

