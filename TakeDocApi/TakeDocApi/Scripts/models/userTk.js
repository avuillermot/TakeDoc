﻿function userTk(user) {
    if (user != null) {
        this.FirstName = user.UserTkFirstName;
        this.LastName = user.UserTkLastName;
        this.Email = user.UserTkEmail
        this.Telephone = "";
        this.Id = user.UserTkId;
        this.Entitys = new Array();
        this.Culture = user.UserTkCulture;
        this.Enable = user.UserTkEnable;
        this.Activate = user.UserTkActivate;
        if (user.GroupTk != null) {
            this.GroupId = user.GroupTk.GroupTkId;
            this.GroupLabel = user.GroupTk.GroupTkLabel;
            this.GroupReference = user.GroupTk.GroupTkReference;
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
    var url = environnement.UrlBase + "identity/update/<userId/>/<firstName/>/<lastName/>/<email/>/<culture/>/<enable/>/<activate/>/<groupId/>";
    $.ajax({
        type: 'PATCH',
        url: url.replace("<userId/>", user.userId).replace("<firstName/>", user.firstName).replace("<lastName/>", user.lastName).replace("<email/>", user.email).replace("<culture/>", user.culture).replace("<enable/>", user.enable).replace("<activate/>", user.activate).replace("<groupId/>", user.groupId),
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

userTkService.generatePassword = function (param) {
    var url = (environnement.UrlBase + "identity/generate/password/<userId/>").replace("<userId/>", param.userId);
    $.ajax({
        type: 'POST',
        url: url,
        success: param.success,
        error: param.error
    });
}

userTkService.search = function (param, success, error) {
    var url = environnement.UrlBase + "search/user";
    $.ajax({
        type: 'POST',
        url: url,
        data: { '': JSON.stringify(param) },
        success: success,
        error: error
    });
}

userTkService.delete = function (param, success, error) {
    var url = (environnement.UrlBase + "identity/delete/<userId/>/<currentUserId/>").replace("<userId/>", param.userId).replace("<currentUserId/>", param.currentUserId);
    $.ajax({
        type: 'DELETE',
        url: url,
        success: param.success,
        error: param.error
    });
}