function userTk(user) {
    if (user != null) {
        this.FirstName = user.UserTkFirstName;
        this.LastName = user.UserTkLastName;
        this.Email = user.UserTkEmail
        this.Telephone = "";
        this.Id = user.UserTkId;
        this.Entitys = new Array();
        this.Culture = user.UserTkCulture;
        if (user.GroupTk != null) {
            this.GroupTkId = user.GroupTk.GroupTkId;
            this.GroupTkLabel = user.GroupTk.GroupTkLabel;
            this.GroupTkReference = user.GroupTk.GroupTkReference;
        }
        this.ExternalAccount = user.UserTkExternalAccount;

        that = this;
        if (user.View_UserEntity != null && user.View_UserEntity.length > 0) {
            $.each(user.View_UserEntity, function (index, value) {
                if (value.EtatDeleteData == false) {
                    var myEntity = new entity(value);
                    that.Entitys.push(myEntity);
                }
            });
        }
    }
};

function userTkService() {

}

userTkService.logon = function (login, pwd, success, error) {
    var url = environnement.UrlBase + "identity/logon";
    var data = { login: login, password: pwd };
    $.ajax({
        type: 'POST',
        url: url,
        data: { '': JSON.stringify(data)},
        success: success,
        error: error
    });
};

userTkService.dashboard = function (userId, success, error) {
    var url = environnement.UrlBase + "identity/dashboard/<userId/>";
    url = url.replace("<userId/>", userId);
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error
    });
};

userTkService.get = function (userId, success, error) {
    var url = environnement.UrlBase + "identity/user/<userId/>";
    url = url.replace("<userId/>", userId);
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error
    });
};

userTkService.update = function (user, success, error) {

    var url = environnement.UrlBase + "identity/update/<userId/>/<firstName/>/<lastName/>/<email/>/<culture/>";
    $.ajax({
        type: 'PATCH',
        url: url.replace("<userId/>", user.userId).replace("<firstName/>", user.firstName).replace("<lastName/>", user.lastName).replace("<email/>", user.email).replace("<culture/>", user.culture),
        success: success,
        error: error
    });
}

userTkService.changePassword = function (param, success, error) {
    var data = {
        userId: param.userId,
        olderPassword: param.olderPassword,
        newPassword: param.newPassword
    };
    var url = environnement.UrlBase + "identity/changepassword";
    $.ajax({
        type: 'PATCH',
        url: url,
        data: { '': JSON.stringify(data) },
        success: success,
        error: error
    });
}

userTkService.search = function (param, success, error) {
    var query = "";
    if (param != null) {
        query = query + " 1 eq 1 "
        if (param.firstName != null && param.firstName != "") query = query + " and startswith(UserTkFirstName,'" + odataHelper.replaceAll(param.firstName,"\'", "\''") + "')";
        if (param.lastName != null && param.lastName != "") query = query + " and startswith(UserTkLastName,'" + odataHelper.replaceAll(param.lastName,"\'", "\''") + "')";
        if (param.email != null && param.email != "") query = query + " and startswith(UserTkEmail,'" + odataHelper.replaceAll(param.email,"\'", "\''") + "')";
        if (param.entityId != null && param.entityId != "") query = query + " and View_UserEntity/any(View_UserEntity: View_UserEntity/EntityId eq guid'" + odataHelper.replaceAll(param.entityId,"'", "\''") + "')";
    }
    if (query != "") query = "?$filter=" + query;

    var url = environnement.UrlBase + "odata/UserTks" + query;
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error
    });
}