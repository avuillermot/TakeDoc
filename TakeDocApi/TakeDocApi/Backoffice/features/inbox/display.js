'use strict';
backOffice.controller('displayController', ['$scope', '$rootScope', '$stateParams', '$timeout', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, $timeout, documentDisplay, documentsDirectory) {

    var myDocComplete = new DocumentComplete();
    var wfHistory = new WorkflowHistorys();
    var answers = new WorkflowAnswers();
    var cloneData = new Array();

    // clone metadata to compare if data has changed before save
    var clone = function () {
        cloneData = new Array();
        for (var i = 0; i < $scope.metadatas.length; i++) {
            var current = $scope.metadatas.at(i);
            var id = current.get("id");
            var value = current.get("value");
            cloneData.push({ id: id, value: value });
        }
    };

    // test if data are updated, return true if yes else false
    var hasChanged = function () {
        if ($scope.isUpdate) {
            $scope.isUpdate = false;
            return true;
        }
        for (var i = 0; i < cloneData.length; i++) {
            var current = cloneData[i];
            var meta = $scope.metadatas.where({ id: current.id });
            if (meta[0].get("value") != current.value) return true;
        }
        return false;
    };

    $scope.isUpdate = false;
    $scope.openImage = function () {
        $("#enlarge-page-modal-label").html("Page " + this.page.get("pageNumber"));
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

    $scope.ArchiveIsEnable = function () {
        if (documentDisplay.data.document == null) return false;
        if (documentDisplay.data.document.get("statusReference") == "APPROVE"
            || documentDisplay.data.document.get("statusReference") == "REFUSE") return true;
        return false;
    };

    $scope.workflowIsEnable = false;

    $scope.resetAnswer = function () {
        $scope.currentAnswer = null;
    }
    $scope.doSelectAnswer = function () {
        $scope.currentAnswer = this.answer;
    };

    var loadComplete = function () {
        var success = function () {
            var data = arguments[0];
            $scope.document = data.document;
            $scope.metadatas = data.metadatas;
            $scope.pages = data.pages;
            $scope.historys = [];
            if (!$scope.$$phase) $scope.$apply();
            clone();
            loadHistory();
        };
        var error = function () {
            $rootScope.showError(arguments[0]);
        };
        var context = {
            versionId: documentDisplay.data.document.get("versionId"),
            userId: $rootScope.getUser().Id,
            entityId: documentDisplay.data.document.get("entityId"),
            success: success,
            error: error
        };
        myDocComplete.load(context);
    }
    
    // subscribe to event for display the current document
    $scope.$watch(function () { return documentDisplay.data.calls; }, function () {
        $rootScope.hideLoader();
        if (documentDisplay.data.document != null) loadComplete();
    });

    // display pdf of this document
    $scope.doOpenDocument = function () {
        var success = function () {

        };
        var error = function () {
            $rootScope.showError("Impossible d'ouvrir le document.")
        };

        fileHelper.readDocumentUrl(documentDisplay.data.document.get("versionId"), documentDisplay.data.document.get("entityId"), success, error);
    };

    $scope.doRemove = function () {
       $rootScope.showOkCancelModal("Confirmer la suppression du document", doRemoveRunfunction);
    };

    // remove this document
    var doRemoveRunfunction = function() {
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

    $scope.doSave = function (startWorkflow) {
        var ok = utils.setStateInputField("divDetailDocument");
        if (ok) {
            var success = function () {
                debugger;
                $rootScope.hideLoader();
                
                // refresh screen -> need for metadatafile
                documentDisplay.data.calls = documentDisplay.data.calls + 1;
                if (!$scope.$$phase) $scope.$apply();
            };
            var error = function () {
                debugger;
                $rootScope.hideLoader();
                var err = arguments[0];
                if (err.message == null) err.message = "Une erreur est survenue lors de l'enregistrement.";
                $rootScope.showError(err);
            };

            // data field are mapped to the model here because date picker cause problem
            var elemsDate = $('#divInboxDisplay input[type="date"]');
            $.each(elemsDate, function (index, value) {
                var elemMetaId = value.name;
                var elemMetaValue = value.value;
                var current = $scope.metadatas.where({ id: elemMetaId });
                current[0].set("value", elemMetaValue);
            });
            var context = {
                userId: $rootScope.getUser().Id,
                entityId: $scope.document.get("entityId"),
                startWorkflow: startWorkflow,
                success: success,
                error: error
            };

            if (hasChanged() == true && startWorkflow == false) {
                $rootScope.showLoader("Enregistrement....");
                myDocComplete.save(context);
            }
            if (hasChanged() == true && startWorkflow == true) {
                var fn = function () {
                    $rootScope.showLoader("Enregistrement....");
                    myDocComplete.save(context);
                };
                $rootScope.showOkCancelModal("Confirmer l'envoi du document au back-office", fn);
            }
            else if (startWorkflow) {
                var fn = function () {
                    $rootScope.showLoader("Enregistrement....");
                    myDocComplete.startWorkflow(context);
                };
                $rootScope.showOkCancelModal("Confirmer l'envoi du document au back-office", fn);
            }
        }
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
    /***********************************************/
    // Workflow answer
    /***********************************************/
    $scope.showAnswer = function () {

        $scope.currentAnswer = {
            attributes: {
                label: "(Aucun)"
            }
        };

        var fDisplayModal = function () {
            $("#modalAnswerWorkflow").modal("show");
            $("#comment").val("");
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
        if ($scope.currentAnswer.attributes.id == null) return false;

        var data = {
            comment: $("#comment").val()
        };
        var currentAction = wfHistory.getCurrentAction();
        var param = {
            workflowId: currentAction.get("id"),
            versionId: currentAction.get("versionId"),
            userId: $rootScope.getUser().Id,
            answerId: $scope.currentAnswer.get("id"),
            data: data,
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

    $scope.doArchive = function () {
        var param = {
            documentId: $scope.document.get("id"),
            userId: $rootScope.getUser().Id
        };
        documentService.SetArchive(param);
    }

    /***********************************************/
    // FILE TO UPLOAD
    /***********************************************/
    $scope.fileAdd = function ($file, $event, $flow, metadataId) {
        $scope.isUpdate = true;
        var base64;
        var fileReader = new FileReader();
        fileReader.onload = function (event) {
            var current = $scope.metadatas.where({ id: metadataId });
            if (current.length > 0) {
                base64 = event.target.result;
                var file = current[0].get("file");
                file.set("data", base64);
                file.set("name", $file.name);
                file.set("path", $file.name);
                current[0].set("value", $file.name);
                if (!$scope.$$phase) $scope.$apply();
            }
        };
        fileReader.readAsDataURL($file.file);
    };

    $scope.fileRemove = function (metadataId) {
        $scope.isUpdate = true;
        var current = $scope.metadatas.where({ id: metadataId });
        if (current.length > 0) {
            var file = current[0].get("file");
            file.set("data", null);
            file.set("name", null);
            current[0].set("value", null);
            if (!$scope.$$phase) $scope.$apply();
        }
    };
    
    // display pdf of this document
    $scope.doOpenFile = function (id) {
        if (hasChanged()) {
            $rootScope.showError({ message: "Veuillez enregistrer les modifications." });
            return false;
        }
        var success = function () {

        };
        var error = function () {
            $rootScope.showError("Impossible d'ouvrir le document.")
        };
        fileHelper.readFileUrl(id, documentDisplay.data.document.get("entityId"), success, error);
    };

    /***********************************************/
    // PANE PAGE
    /***********************************************/
    function pageChanged(page) {
        page.set('action', 'update');
        $scope.isUpdate = true;
    };
    $scope.doTurn = function (id) {
        var elem = angular.element("#img-page-" + id);
        var prefix = "inbox-thumbnail-rotate";
        var r000 = elem.hasClass(prefix + "0");
        var r090 = elem.hasClass(prefix + "90");
        var r180 = elem.hasClass(prefix + "180");
        var r270 = elem.hasClass(prefix + "270");

        elem.removeClass(prefix + "0");
        elem.removeClass(prefix + "90");
        elem.removeClass(prefix + "180");
        elem.removeClass(prefix + "270");

        var rotation = 0;
        if (r000) {
            elem.addClass(prefix + "90");
            rotation = 90;
        }
        else if (r090) {
            elem.addClass(prefix + "180");
            rotation = 180;
        }
        else if (r180) {
            elem.addClass(prefix + "270");
            rotation = 270;
        }
        else if (r270) {
            elem.addClass(prefix + "0");
            rotation = 0;
        }
        var page = $scope.pages.where({ id: id });
        page[0].set('rotation', rotation);
        pageChanged(page[0]);
    };
    $scope.moveUp = function (id) {
        var size = $scope.pages.length;
        var page = $scope.pages.where({ id: id });
        var currentIndex = page[0].get('pageNumber');
        if (currentIndex > 1) {
            var pageToMove = $scope.pages.where({ pageNumber: currentIndex - 1 });
            page[0].set('pageNumber', currentIndex - 1);
            pageToMove[0].set('pageNumber', currentIndex);
            pageChanged(page[0]);
        }
    };
    $scope.moveDown = function (id) {
        var size = $scope.pages.length;
        var page = $scope.pages.where({ id: id });
        var currentIndex = page[0].get('pageNumber');
        if (currentIndex < size) {
            var pageToMove = $scope.pages.where({ pageNumber: currentIndex + 1 });
            page[0].set('pageNumber', currentIndex + 1);
            pageToMove[0].set('pageNumber', currentIndex);
            pageChanged(page[0]);
        }
    };
    $scope.deletePicture = function (id) {
        var size = $scope.pages.length;
        $scope.pages.remove({ id: id });
        $scope.numeroter(1, size);
    };
    $scope.numeroter = function (startIndex, size) {
        var index = startIndex;
        var nb = startIndex;
        while (index <= size) {
            var page = $scope.pages.where({ pageNumber: index });
            if (page.length > 0) {
                page[0].set('pageNumber', nb++);
            }
            index++;
        }
    };
}]);