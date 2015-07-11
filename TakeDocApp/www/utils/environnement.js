var environnement = {
    //UrlBase: "http://localhost/TakeDocApi/",
    UrlBase: "https://dev-takedoc.cloudapp.net/",
    isApp: true
};

var requestHelper = {
    beforeSend: function() {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization",null);
        }
    }
};

