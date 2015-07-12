function userTk(user, withToken) {
    if (user != null) {
        if (withToken == null) withToken = false;
        this.FirstName = user.UserTkFirstName;
        this.LastName = user.UserTkLastName;
        this.Email = user.UserTkEmail
        this.Telephone = "";
        this.Id = user.UserTkId;
        this.Entitys = new Array();
        this.Culture = user.UserTkCulture;
        this.Enable = user.UserTkEnable;
        this.Activate = user.UserTkActivate;
        if (withToken) environnement.setToken(user);
        if (user.GroupTk != null) {
            this.GroupId = user.GroupTk.GroupTkId;
            this.GroupLabel = user.GroupTk.GroupTkLabel;
            this.GroupReference = user.GroupTk.GroupTkReference;
        }
        this.ExternalAccount = user.UserTkExternalAccount;
        this.ManagerId = user.UserTkManagerId;

        that = this;
        if (user.View_UserEntity != null && user.View_UserEntity.length > 0) {
            $.each(user.View_UserEntity, function (index, value) {
                if (value.EtatDeleteData == false) {
                    var myEntity = new entity(value);
                    that.Entitys.push(myEntity);
                }
            });
        }
        moment.locale(user.UserTkCulture);
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
        error: error,
        beforeSend: requestHelper.beforeSend()
    });
};

userTkService.get = function (userId, success, error) {
    var url = environnement.UrlBase + "identity/user/<userId/>";
    url = url.replace("<userId/>", userId);
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error,
        beforeSend: requestHelper.beforeSend()
    });
};

userTkService.getName = function (userId, success, error) {
    var url = environnement.UrlBase + "identity/name/<userId/>";
    url = url.replace("<userId/>", userId);
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error,
        beforeSend: requestHelper.beforeSend()
    });
};

userTkService.update = function (user, success, error) {
    if (user.userId != user.managerId) {
        var url = environnement.UrlBase + "identity/update/<userId/>/<firstName/>/<lastName/>/<email/>/<culture/>/<enable/>/<activate/>/<groupId/>/<managerId/>";
        $.ajax({
            type: 'PATCH',
            url: url.replace("<userId/>", user.userId).replace("<firstName/>", user.firstName).replace("<lastName/>", user.lastName)
                .replace("<email/>", user.email).replace("<culture/>", user.culture).replace("<enable/>", user.enable)
                .replace("<activate/>", user.activate).replace("<groupId/>", user.groupId).replace("<managerId/>", user.managerId),
            success: success,
            error: error,
            beforeSend: requestHelper.beforeSend()
        });
    }
    else error({ message: "Un utilisateur ne peut pas être son propre manager." });
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
        error: error,
        beforeSend: requestHelper.beforeSend()
    });
}

userTkService.generatePassword = function (param) {
    debugger;
    var url = (environnement.UrlBase + "identity/generate/password/<userId/>").replace("<userId/>", param.userId);
    $.ajax({
        type: 'POST',
        success: param.success,
        error: param.error,
        beforeSend: requestHelper.beforeSend(),
        url: url
    });
}

userTkService.search = function (param, success, error) {
    var url = environnement.UrlBase + "search/user";
    $.ajax({
        type: 'POST',
        url: url,
        data: { '': JSON.stringify(param) },
        success: success,
        error: error,
        beforeSend: requestHelper.beforeSend()
    });
}

userTkService.delete = function (param, success, error) {
    var url = (environnement.UrlBase + "identity/delete/<userId/>/<currentUserId/>").replace("<userId/>", param.userId).replace("<currentUserId/>", param.currentUserId);
    $.ajax({
        type: 'DELETE',
        url: url,
        success: param.success,
        error: param.error,
        beforeSend: requestHelper.beforeSend()
    });
}