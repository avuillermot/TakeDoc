var environnement = {
    isApp: false,
    //UrlBase: "http://192.168.0.12/TakeDocApi/",
    UrlBase: "http://localhost/TakeDocApi/",
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
    },
    getLoginUrl: function () {
        return window.location.origin + window.location.pathname + "#/login"
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
                alert("Votre session a expirée (2).");
                window.location = environnement.getLoginUrl();
            }
        };
        $.ajax({
            type: 'GET',
            async: false,
            url: environnement.UrlBase + 'token/access/' + sessionStorage.getItem('RefreshToken'),
            success: param.success,
            error: param.error
        });
    },
    beforeSend: function () {
        var ticks = ((moment.utc()._d.getTime() * 10000) + 621355968000000000);
        if (sessionStorage.getItem('RefreshTokenTicks') < ticks) {
            alert("Votre session a expirée (1).");
            window.location = environnement.getLoginUrl();
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