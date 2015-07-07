var environnement = {
    UrlBase: "http://localhost/TakeDocApi/",
    RefreshToken: null,
    AccessToken: null,
    AccessTokenTicks: null,
    RefreshTokenTicks: null,
}

var requestHelper = {
    _lock: false,
    getNewAccessToken: function () {
        var that = this;
        var param = {
            success: function () {
                environnement.AccessToken = arguments[0].AccessToken;
                environnement.AccessTokenTicks = arguments[0].AccessTokenTicks;
                alert("access token refresh ok");
                that._lock = false;
            },
            error: function () {
                alert("access token refresh error");
            }
        };
        $.ajax({
            type: 'GET',
            url: environnement.UrlBase + 'token/access/'+environnement.RefreshToken,
            success: param.success,
            error: param.error
        });
    },
    beforeSend: function () {
        this._lock = false;
        var ticks = ((moment.utc()._d.getTime() * 10000) + 621355968000000000);
        if (environnement.AccessTokenTicks < ticks) {
            this._lock = true;
            this.getNewAccessToken();
        }
        while (this._lock == true);
        if (environnement.RefreshTokenTicks < ticks) {
            alert("refresh token expired");
            return false;
        }
        return function (xhr, settings) {
            xhr.setRequestHeader("Authorization", environnement.AccessToken);
        }
    }
};