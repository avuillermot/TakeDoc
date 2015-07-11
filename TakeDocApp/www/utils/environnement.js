var environnement = {
    UrlBase: "http://localhost/TakeDocApi/",
    //UrlBase: "https://dev-takedoc.cloudapp.net/",
    isApp: false
};

var requestHelper = {
    beforeSend: function() {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization","test");
        }
    }
};

