var environnement = {
    isApp: true,
    UrlBase: "http://192.168.0.11/TakeDocApi/",
    //UrlBase: "http://localhost/TakeDocApi/",
    //UrlBase: "https://dev-takedoc.cloudapp.net/",
    setToken: function (user) {
        sessionStorage.setItem('AccessToken', user.AccessToken);
        sessionStorage.setItem('AccessTokenTicks', user.AccessTokenTicks);
        sessionStorage.setItem('RefreshToken', user.RefreshToken);
        sessionStorage.setItem('RefreshTokenTicks', user.RefreshTokenTicks);
    },
    isAuthenticate: function () {
        if (sessionStorage.getItem('RefreshTokenTicks') == null) return false

        var ticks = ((moment.utc()._d.getTime() * 10000) + 621355968000000000);
        if (sessionStorage.getItem('RefreshTokenTicks') < ticks) return false;
        return true;
    }
}

var requestHelper = {
    getNewAccessToken: function () {
        var that = this;
        var param = {
            success: function () {
                sessionStorage.setItem('AccessToken', arguments[0].AccessToken);
                sessionStorage.setItem('AccessTokenTicks', arguments[0].AccessTokenTicks);
            },
            error: function () {
                alert("Vous n'avez pas accès à cette fonctionnalité.");
            }
        };
        $.ajax({
            type: 'GET',
            async: false,
            url: environnement.UrlBase + 'token/access/'+$.cookie('RefreshToken'),
            success: param.success,
            error: param.error
        });
    },
    beforeSend: function () {
        var ticks = ((moment.utc()._d.getTime() * 10000) + 621355968000000000);
        if (sessionStorage.getItem('RefreshTokenTicks') < ticks) {
            alert("Vous n'avez pas accès à cette fonctionnalité.");
            return false;
        }
        else if (sessionStorage.getItem('AccessTokenTicks') < ticks) {
            this.getNewAccessToken();
        }
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", sessionStorage.getItem('AccessToken'));
        }
    }
};