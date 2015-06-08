'use strict';
backOffice.controller('displayController', ['$scope', '$rootScope', '$stateParams', '$timeout', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, $timeout, documentDisplay, documentsDirectory) {

    var pages = new Pages();
    var wfHistory = new WorkflowHistorys();
    var answers = new WorkflowAnswers();
    var cloneData = new Array();

    // clone metadata to compare if data has changed before save
    var clone = function () {
        cloneData = new Array();
        for (var i = 0; i < documentDisplay.data.metadatas.length; i++) {
            var current = documentDisplay.data.metadatas.at(i);
            var id = current.get("id");
            var value = current.get("value");
            cloneData.push({id: id, value: value});
        }
    };

    // test if data are updated, return true if yes else false
    var hasChanged = function () {
        for (var i = 0; i < cloneData.length; i++) {
            var current = cloneData[i];
            var meta = documentDisplay.data.metadatas.where({ id: current.id });
            if (meta[0].get("value") != current.value) return true;
        }
        return false;
    };

    $scope.openImage = function () {
        $("#enlarge-page-modal-label").html("Page " + this.page.get("index"));
        $("#enlarge-page-modal-body").html('<img style="width:100%" src="'+this.page.get("base64Image")+'" class="inbox-thumbnail-rotate'+this.page.get("rotation")+'" />');
        $('#enlarge-page-modal').modal('show');
    };

    $scope.pdfIsEnable = function () {
        if (documentDisplay.data.document == null) return false;
        if (documentDisplay.data.document.get("statusReference") == "CREATE"
            || documentDisplay.data.document.get("statusReference") == "COMPLETE"
            || documentDisplay.data.document.get("statusReference") == "INCOMPLETE") return false;
        return true;
    };

    $scope.workflowIsEnable = false;
    
    // subscribe to event for display the current document
    $scope.$watch(function () { return documentDisplay.data.calls; }, function () {
        $rootScope.hideLoader();
        if (documentDisplay.data.document != null) {
            $scope.document = documentDisplay.data.document;
            $scope.metadatas = documentDisplay.data.metadatas.models;
            $scope.title = $scope.document.get("label") + " - (" + $scope.document.get("entityLabel") + ")";

            loadImage();
            loadHistory();
        }
        else {
            $scope.document = {};
            $scope.metadatas = [];
            $scope.title = null;
        }
        clone();
    });

    // display pdf of this document
    $scope.doOpenDocument = function () {
        var success = function () {

        };
        var error = function () {
            $rootScope.showError("Impossible d'ouvrir le document.")
        };

        fileHelper.readUrl(documentDisplay.data.document.get("versionId"), documentDisplay.data.document.get("entityId"), success, error);
    };

    // remove this document
    $scope.doRemove = function () {
        var documentsExt = new DocumentsExtended();
        var param = {
            documentId: $scope.document.get("id"),
            entityId: $scope.document.get("entityId"),
            userId: $rootScope.getUser().Id,
            success: function () {
                $rootScope.hideLoader();
                documentsDirectory.data.documents.remove(documentDisplay.data.document);
                documentsDirectory.data.calls = documentsDirectory.data.calls + 1;

                documentDisplay.data.document = null;
                documentDisplay.data.calls = documentDisplay.data.calls + 1;

                $scope.pages = null;
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.hideLoader();
            }
        };
        $rootScope.showLoader("Suppression....");
        documentsExt.delete(param);
    };

    $scope.doSave = function () {
        var ok = utils.setStateInputField("divDetailDocument");
        if (ok) {
            var success = function () {
                $rootScope.hideLoader();
                if ($scope.pdfIsEnable() == true && hasChanged() == true) generatePdf();
                clone();
            };
            var error = function () {
                $rootScope.hideLoader();
                var err = arguments[0];
                if (err.message == null) err.message = "Une erreur est survenue lors de l'enregistrement.";
                $rootScope.showError(err);
            };
            var param = {
                userId: $rootScope.getUser().Id,
                entityId: $scope.document.get("entityId"),
                versionId: $scope.document.get("versionId")
            };

            // data field are mapped to the model here because date picker cause problem
            var elemsDate = $('#divInboxDisplay input[type="date"]');
            $.each(elemsDate, function (index, value) {
                var elemMetaId = value.name;
                var elemMetaValue = value.value;
                var current = documentDisplay.data.metadatas.where({ id: elemMetaId });
                current[0].set("value", elemMetaValue);
            });

            if (hasChanged()) {
                $rootScope.showLoader("Enregistrement....");
                documentDisplay.data.metadatas.save(param, success, error);
            }
        }
    };

    var generatePdf = function () {
        var param = {
            userId: $rootScope.getUser().Id,
            entityId: $scope.document.get("entityId"),
            versionId: $scope.document.get("versionId"),
            success: function () {
                $rootScope.hideLoader();
            },
            error: function () {
                $rootScope.hideLoader();
                $rootScope.showError("La générartion du PDF est en erreur");
            }
        };
        $rootScope.showLoader("Génération PDF....");
        documentDisplay.data.metadatas.generatePdf(param);
    };

    var loadImage = function () {
        var success = function () {
            $scope.pages = arguments[0];
            if (!$scope.$$phase) $scope.$apply();
        };

        var param = {
            userId: $rootScope.getUser().Id,
            entityId: $scope.document.get("entityId"),
            versionId: $scope.document.get("versionId"),
            success: success,
            error: function () {
                $rootScope.showError({ message: "Les images ne sont pas disponibles" });
            }
        };
        pages.load(param);
    };

    var loadHistory = function () {
        var success = function () {
            $scope.historys = arguments[0];
            if ($scope.document.get("ownerId") == $rootScope.getUser().Id) $scope.workflowIsEnable = false;
            else {
                var steps = $scope.historys.where({ statusId: $scope.document.get("statusId") });
                if (steps.length > 0) {
                    $scope.workflowIsEnable = steps[0].actions.length > 0;
                }
            }
            if (!$scope.$$phase) $scope.$apply();
        };

        var param = {
            entityId: $scope.document.get("entityId"),
            documentId: $scope.document.get("id"),
            success: success,
            error: function () {
                $rootScope.showError({ message: "L'historique n'est pas disponible" });
            }
        };
        wfHistory.load(param);
    };

    $scope.showAnswer = function () {
        var fDisplayModal = function () {
            $("#modalAnswerWorkflow").modal("show");
        };
        
        var param = {
            action: wfHistory.getCurrentAction(),
            success: function () {
                $scope.answers = arguments[0];
                fDisplayModal();
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {

            }
        };
        answers.load(param);
    };

    $scope.doValidSelectedAnswer = function () {
        var currentAction = wfHistory.getCurrentAction();
        var param = {
            workflowId: currentAction.get("id"),
            versionId: currentAction.get("versionId"),
            userId: $rootScope.getUser().Id,
            answerId: this.answer.get("id"),
            always: function () {
                $("#modalAnswerWorkflow").modal("hide");
            },
            success: function() {
                loadHistory();
            },
            error: function () {
                $rootScope.showError({ message: "Votre réponse n'est pas prise en compte." })
            }
        };
        answers.answer(param);
       
    };
}]);