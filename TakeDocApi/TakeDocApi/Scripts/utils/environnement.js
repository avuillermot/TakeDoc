var environnement = {
    isApp: false,
    durationCookieDay: 1,
    UrlBase: "http://localhost/TakeDocApi/",
    //UrlBase: "https://dev-takedoc.cloudapp.net/",
    setToken: function (user) {
        $.cookie('AccessToken', user.AccessToken, { expires: environnement.durationCookieDay });
        $.cookie('AccessTokenTicks', user.AccessTokenTicks, { expires: environnement.durationCookieDay });
        $.cookie('RefreshToken', user.RefreshToken, { expires: environnement.durationCookieDay });
        $.cookie('RefreshTokenTicks', user.RefreshTokenTicks, { expires: environnement.durationCookieDay });
    },
    isAuthenticate: function () {
        if ($.cookie('RefreshTokenTicks') == null) return false

        var ticks = ((moment.utc()._d.getTime() * 10000) + 621355968000000000);
        if ($.cookie('RefreshTokenTicks') < ticks) return false;
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
                $.cookie('AccessToken', arguments[0].AccessToken, { expires: environnement.durationCookieDay });
                $.cookie('AccessTokenTicks', arguments[0].AccessTokenTicks, { expires: environnement.durationCookieDay });
            },
            error: function () {
                alert("Votre session a expirée (2).");
                window.location = environnement.getLoginUrl();
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
        if ($.cookie('RefreshTokenTicks') < ticks) {
            alert("Votre session a expirée (1).");
            window.location = environnement.getLoginUrl();
            return false;
        }
        else if ($.cookie('AccessTokenTicks') < ticks) {
            this.getNewAccessToken();
        }
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", $.cookie('AccessToken'));
        }
    },
    getAccessToken: function () {
        var ticks = ((moment.utc()._d.getTime() * 10000) + 621355968000000000);
        if ($.cookie('RefreshTokenTicks') < ticks) {
            alert("Votre session a expirée (1).");
            window.location = environnement.getLoginUrl();
            return false;
        }
        else if ($.cookie('AccessTokenTicks') < ticks) {
            this.getNewAccessToken();
        }
        return $.cookie('AccessToken');
    }
};