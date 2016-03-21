'use strict';
backOffice.controller('detailTypeDocumentController', ['$scope', '$rootScope', '$location', '$stateParams', 'typeDocumentResult', 'inputTypes', function ($scope, $rootScope, $location, $stateParams, typeDocumentResult, inputTypes) {
    var typeDocuments = new TypeDocuments();
    var workflowTypes = new WorkflowTypes();
    // field link to document type
    var fields = new DocumentFields();
    // value link to fields
    var values = new FieldValues();
    // autocomplete configuraton link to field
    var autocompletes = new FieldAutocompletes();
    // all field allow for link to document type
    var dataFields = new DataFields();
    // manager who can administrate this document type
    var managerTypeDocuments = new ManagerTypeDocumnents();
       
    $("#viewRight").css("width", "0%");
    $("#viewLeft").css("width", "100%");

    var setCurrentEntity = function () {
        $.each($rootScope.getUser().Entitys, function (index, value) {
            if (value.Id == $scope.selectedItem.get("entityId")) {
                $scope.currentEntity = value;
                $scope.searchEntityId = value.Id;
            }
        });
    };

    // set field index after update list of field
    var numeroter = function (startIndex, size) {
        var index = startIndex;
        var nb = startIndex;
        while (index <= size) {
            var field = $scope.fields.where({ index: index, deleted: false });
            if (field.length > 0) {
                field[0].set('index', nb++);
            }
            index++;
        }
    };

    //************************************
    // load data context
    //************************************
    var loadDocumentFields = function (typeDocumentId, entityId) {
        var param = {
            id: typeDocumentId,
            deleted: false,
            always: function () {
                loadWorkflowTypes();
            },
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement des champs pour ce type document.")
            },
            success: function () {
                $scope.fields = arguments[0];
            }
        };
        fields.load(param);

    };
    var loadWorkflowTypes = function () {
        var param = {
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement des types de validations.")
            },
            success: function () {
                $scope.validations = arguments[0];
                $scope.selectedValidation = workflowTypes.where({ id: $scope.selectedItem.get("workflowTypeId") })[0];

            }
        };
        workflowTypes.load(param);
    }
    var loadDataFields = function (entityId) {
        var param = {
            entityId: entityId,
            always:null,
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement des champs disponibles.")
            },
            success: function () {
                $scope.dataFields = arguments[0];
            }
        };
        dataFields.load(param);
    }
    var loadManagerTypeDocument = function (typeDocumentId, entityId) {
        var param = {
            typeDocumentId: typeDocumentId,
            entityId: entityId,
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement des gestionnaires.")
            },
            success: function () {
                $scope.managersTypeDoc = arguments[0];
            }
        };
        managerTypeDocuments.load(param);
    }

    //************************************
    // function for datafield admin
    //************************************
    $scope.moveUp = function (id) {
        var size = $scope.fields.length;
        var field = $scope.fields.where({ id: id });
        var currentIndex = field[0].get('index');
        if (currentIndex > 1) {
            var fieldToMove = $scope.fields.where({ index: currentIndex - 1 });
            field[0].set('index', currentIndex - 1);
            fieldToMove[0].set('index', currentIndex);
        }
    };
    $scope.moveDown = function (id) {
        var size = $scope.fields.length;
        var field = $scope.fields.where({ id: id });
        var currentIndex = field[0].get('index');
        if (currentIndex < size) {
            var fieldToMove = $scope.fields.where({ index: currentIndex + 1 });
            field[0].set('index', currentIndex + 1);
            fieldToMove[0].set('index', currentIndex);
        }
    }
    $scope.doRemoveDataField = function (id) {
        var toDel = $scope.fields.where({ id: id })[0]
        toDel.set("deleted", true);
        toDel.set("index", -1);
        numeroter(1, $scope.fields.length + 1);
    };
    $scope.doOpenModalAddField = function () {
        $scope.fieldsToAdd = new Array();

        // we propose field that is not deleted and not already add in document type
        $.each(dataFields.where({ deleted: false }), function (index, value) {
            var already = fields.where({ id: value.get("id"), deleted: false, entityId: $scope.selectedItem.get("entityId")  });
            if (already.length == 0) $scope.fieldsToAdd.push(value);
        });
        if ($scope.fieldsToAdd.length > 0) $("#modalAddFieldToDocumentType").modal("show");
        else $rootScope.showModal("Informations", "Tous les champs disponibles ont été ajoutés.")
    };
    $scope.doAddDataField = function () {
        $("#modalAddFieldToDocumentType").modal("hide");
        // get index of this field, set has last field of document type
        var newIndex = fields.getLastIndex() + 1;

        var already = fields.where({ id: this.fieldsToAdd.get("id"), entityId: $scope.selectedItem.get("entityId") });
        if (already.length == 1) {
            already[0].set("deleted", false);
            already[0].set("index", newIndex);
        }
        else {
            // get type of this field
            var currentInputType = inputTypes.data.inputTypes.where({ id: this.fieldsToAdd.get("typeId") });

            newIndex = (currentInputType[0].get("inputType") == "signature") ? 9999 : newIndex;

            var newField = new DocumentField();
            newField.create(
                this.fieldsToAdd.get("id"),
                this.fieldsToAdd.get("reference"),
                this.fieldsToAdd.get("entityId"),
                this.fieldsToAdd.get("label"),
                currentInputType[0].get("inputType"), newIndex);
            newField.set("isList", this.fieldsToAdd.get("isList"));
            newField.set("isAutocomplete", this.fieldsToAdd.get("isAutocomplete"));
            newField.set("autoCompleteId", this.fieldsToAdd.get("autoCompleteId"));

            fields.add(newField);
        }
        if (!$scope.$$phase) $scope.$apply();
    };

    //******************************************
    // function for manager type document admin
    //******************************************
    var doAddManagerTypeDoc = function () {
        if ($scope.managersTypeDoc != null && $scope.searchUserId != null) {
            var toAdd = $scope.managersTypeDoc.where({ id: $scope.searchUserId });
            if (toAdd.length == 0) {
                var m = new ManagerTypeDocumnent();
                m.set("id", $scope.searchUserId);
                m.set("fullName", $scope.searchUserName);
                m.set("deleted", false);
                m.set("entityId", $scope.selectedItem.get("entityId"));
                $scope.managersTypeDoc.add(m);
            }
            else toAdd[0].set("deleted", false);

            $scope.searchUserId = null;
            $scope.searchUserName = null;
        }
    };
    $scope.doRemoveManagerTypeDoc = function (id) {
        var toDel = $scope.managersTypeDoc.where({ id: id });
        toDel[0].set("deleted", true);
    };

    $scope.$watch(function () { return $scope.searchUserId; }, function () {
        doAddManagerTypeDoc();
    });   

    //************************************
    // ihm function event
    //************************************
    $scope.doGoSeach = function () {
        $location.path("/searchTypeDocument");
    };
    $scope.doSelectValidation = function () {
        $scope.selectedValidation = this.validation;
        $scope.selectedItem.set("workflowTypeId", this.validation.get("id"));
    }
        
    $scope.doSave = function () {
        var ok = utils.setStateInputField("divDetailTypeDocument");

        if (ok == true) {
            $rootScope.showLoader("Enregistrement...");
            var param = {
                fields: fields.toJSON(),
                managersTypeDoc: $scope.managersTypeDoc.toJSON(),
                userId: $rootScope.getUser().Id,
                typeDocument: $scope.selectedItem.toJSON(),
                error: function () {
                    $rootScope.hideLoader();
                    $rootScope.showError(arguments[0]);
                },
                success: function () {
                    $rootScope.hideLoader();
                }
            };
            typeDocuments.update(param);
        }
    };
    $scope.doReset = function () {
        // if datasource is empty, we call api or not contains the document type we want to display
        if (typeDocumentResult.data.typeDocuments != null && typeDocumentResult.data.typeDocuments.where({ id: $stateParams.typeDocument }).length > 0) {
            $scope.selectedItem = typeDocumentResult.data.typeDocuments.where({ id: $stateParams.typeDocument })[0];
            loadDocumentFields($scope.selectedItem.get("id"), $scope.selectedItem.get("entityId"));
            loadDataFields($scope.selectedItem.get("entityId"));
            loadManagerTypeDocument($scope.selectedItem.get("id"), $scope.selectedItem.get("entityId"));
            setCurrentEntity($scope.selectedItem.get("entityId"));
        }
        else {
            var param = {
                id: $stateParams.typeDocument,
                success: function () {
                    $scope.selectedItem = arguments[0].at(0);
                    loadDocumentFields($scope.selectedItem.get("id"));
                    loadDataFields($scope.selectedItem.get("entityId"));
                    loadManagerTypeDocument($scope.selectedItem.get("id"), $scope.selectedItem.get("entityId"));
                    setCurrentEntity($scope.selectedItem.get("entityId"));
                },
                error: function () {
                    $rootScope.showError("Une erreur est survenue lors du chargement du type document.");
                }
            };
            typeDocuments.loadById(param);
        }
    };

    // display values for list
    $scope.displayListValuesInfo = function () {
        var title = this.field.get("label");
        var param = {
            id: this.field.get("id"),
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            success: function () {
                $scope.fieldValues = arguments[0];
                $("#modalFieldValues .modal-title").html(title)
                $("#modalFieldValues").modal("show");
            },
            error: null
        };
        values.load(param);
    };
    // display info for autocompletes
    $scope.displayAutocompleteInfo = function (id) {
        var title = this.field.get("label");
        var param = {
            id: this.field.get("autoCompleteId"),
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            success: function () {
                if (arguments.length > 0 && arguments[0].models != null) {
                    $scope.fieldAutocomplete = arguments[0].models[0];
                    $("#modalFieldAutocompletes .modal-title").html(title)
                    $("#modalFieldAutocompletes").modal("show");
                }
            },
            error: null
        };
        autocompletes.load(param);
    };
    $scope.doReset();
}]);