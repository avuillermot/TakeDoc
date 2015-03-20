function popupHelper(ionicPopup, scope ) {
    this.scope = scope;
    this.ionicPopup = ionicPopup;
}

popupHelper.prototype.show = function (title, message, type, onTap) {
    var myPopup = this.ionicPopup.show({
        title: title,
        subTitle: message,
        scope: this.scope,
        buttons: this.getButtonType(type, onTap)
    });
}

popupHelper.prototype.getButtonType = function (type, onTap) {
    var buttons = null;
    if (type == null) type = "Ok";
    if (type == "OkCancel") {
        buttons = [
          {
              text: 'Cancel',
              onTap: function () {
                  this.close();
                  onTap('Cancel');
              }
          },
          {
              text: 'Ok',
              type: 'button-positive',
              onTap: function (e) {
                  this.close();
                  onTap('Ok');
              }
          }
        ];
    }
    else if (type == "Ok") {
        buttons = [
          {
              text: 'Ok',
              onTap: function () {
                  this.close();
                  onTap('Ok');
              }
          }
        ];
    };
    return buttons;
}
