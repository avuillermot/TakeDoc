var environnement = {
    UrlBase: "http://localhost/TakeDocApi/",
    tokenAuthentification: null
}

var requestHelper = {
    beforeSend: function () {
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", environnement.tokenAuthentification);
        }
    }
};