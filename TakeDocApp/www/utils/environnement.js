var environnement = {
    UrlBase: "http://localhost/TakeDocApi/",
    //UrlBase: "http://192.168.1.20/TakeDocApi/",
    isApp: false,
    tokenAuthentification: null,
};

var requestHelper = {
    beforeSend: function() {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", environnement.tokenAuthentification);
        }
    }
};

