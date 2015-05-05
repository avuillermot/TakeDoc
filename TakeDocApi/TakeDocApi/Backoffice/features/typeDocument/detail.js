'use strict';
backOffice.controller('detailTypeDocumentController', ['$scope', '$rootScope', '$location', '$stateParams', 'typeDocumentResult', 'inputTypes', function ($scope, $rootScope, $location, $stateParams, typeDocumentResult, inputTypes) {
    var typeDocuments = new TypeDocuments();
    var typeValidations = new TypeValidations();
    // field link to document type
    var fields = new DocumentFields();
    // value link to fields
    var values = new FieldValues();
    // autocomplete configuraton link to field
    var autocompletes = new FieldAutocompletes();
    // all field allow for link to document type
    var dataFields = new DataFields();
       
    $("#viewRight").css("width", "0%");
    $("#viewLeft").css("width", "100%");

    var setCurrentEntity = function () {
        $.each($rootScope.getUser().Entitys, function (index, value) {
            if (value.Id == $scope.selectedItem.get("entityId")) $scope.currentEntity = value;
        });
    };

    var loadDocumentFields = function (typeDocumentId) {
        var param = {
            id: typeDocumentId,
            always: function () {
                loadTypeValidations();
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
    var loadTypeValidations = function () {
        var param = {
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement des types de validations.")
            },
            success: function () {
                $scope.validations = arguments[0];
                $scope.selectedValidation = typeValidations.where({ id: $scope.selectedItem.get("typeValidationId") })[0];

            }
        };
        typeValidations.load(param);
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

    var numeroter = function (startIndex, size) {
        var index = startIndex;
        var nb = startIndex;
        while (index <= size) {
            var field = $scope.fields.where({ index: index, delete: false });
            if (field.length > 0) {
                field[0].set('index', nb++);
            }
            index++;
        }
    };

    $scope.doSelectValidation = function () {
    }

    $scope.mooveUp = function (id) {
        var size = $scope.fields.length;
        var field = $scope.fields.where({ id: id });
        var currentIndex = field[0].get('index');
        if (currentIndex > 1) {
            var fieldToMove = $scope.fields.where({ index: currentIndex - 1 });
            field[0].set('index', currentIndex - 1);
            fieldToMove[0].set('index', currentIndex);
        }
    };
    $scope.mooveDown = function (id) {
        var size = $scope.fields.length;
        var field = $scope.fields.where({ id: id });
        var currentIndex = field[0].get('index');
        if (currentIndex < size) {
            var fieldToMove = $scope.fields.where({ index: currentIndex + 1 });
            field[0].set('index', currentIndex + 1);
            fieldToMove[0].set('index', currentIndex);
        }
    }
    $scope.doRemove = function (id) {
        var toDel = $scope.fields.where({ id: id })[0]
        toDel.set("delete", true);
        toDel.set("index", -1);
        numeroter(1, $scope.fields.length + 1);
    };

    $scope.doOpenModalAddField = function () {
        $scope.fieldsToAdd = new Array();

        // we propose field that is not delete and not already add in document type
        $.each(dataFields.where({delete: false}), function (index, value) {
            var already = fields.where({ id: value.get("id"), delete: false });
            if (already.length == 0) $scope.fieldsToAdd.push(value);
        });
        if ($scope.fieldsToAdd.length > 0) $("#modalAddFieldToDocumentType").modal("show");
        else $rootScope.showModal("Informations","Tous les champs disponibles ont été ajoutés.")
    };

    $scope.doAddDataField = function (dataFieldId) {
        $("#modalAddFieldToDocumentType").modal("hide");
        // get type of this field
        var currentInputType = inputTypes.data.inputTypes.where({ id: this.fieldsToAdd.get("typeId") });
        // get index of this field, set has last field of document type
        var newIndex = fields.getLastIndex() + 1;

        var newField = new DocumentField();
        newField.create(this.fieldsToAdd.get("id"), this.fieldsToAdd.get("reference"), this.fieldsToAdd.get("label"), currentInputType[0].get("inputType"), newIndex);
        newField.set("isList", this.fieldsToAdd.get("isList"));
        newField.set("isAutocomplete", this.fieldsToAdd.get("isAutocomplete"));
        newField.set("autoCompleteId", this.fieldsToAdd.get("autoCompleteId"));

        fields.add(newField);
        if (!$scope.$$phase) $scope.$apply();
    };

    $scope.doSave = function () {

    };

    $scope.doReset = function () {
        // if datasource is empty, we call api
        if (typeDocumentResult.data.typeDocuments != null) {
            $scope.selectedItem = typeDocumentResult.data.typeDocuments.where({ id: $stateParams.typeDocument })[0];
            loadDocumentFields($scope.selectedItem.get("id"));
            loadDataFields($scope.selectedItem.get("entityId"));
            setCurrentEntity($scope.selectedItem.get("entityId"));
        }
        else {
            var param = {
                id: $stateParams.typeDocument,
                success: function () {
                    $scope.selectedItem = arguments[0].at(0);
                    loadDocumentFields($scope.selectedItem.get("id"));
                    loadDataFields($scope.selectedItem.get("entityId"));
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
    $scope.onListValues = function () {
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
    $scope.onAutocomplete = function (id) {
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