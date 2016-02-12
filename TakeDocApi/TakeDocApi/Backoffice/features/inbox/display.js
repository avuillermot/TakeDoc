'use strict';
backOffice.controller('displayController', ['$scope', '$rootScope', '$stateParams', '$timeout', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, $timeout, documentDisplay, documentsDirectory) {

    var myDocComplete = new DocumentComplete();
    var wfHistory = new WorkflowHistorys();
    var answers = new WorkflowAnswers();
    var cloneData = new Array();

    $scope.open = function ($event) {
        $scope.status.opened = true;
    };

    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.status = {
        opened: false
    };

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
    $scope.hasChanged = function () {
        if ($scope.isUpdate) return true;
        for (var i = 0; i < cloneData.length; i++) {
            var current = cloneData[i];
            var meta = $scope.metadatas.where({ id: current.id });
            if (meta != null && meta[0] != null && meta[0].get("value") != current.value) return true;
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
        $scope.isUpdate = false;
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
                $rootScope.hideLoader();

                $scope.isUpdate = false;
                // refresh screen -> need for metadatafile
                documentDisplay.data.calls = documentDisplay.data.calls + 1;
                if (!$scope.$$phase) $scope.$apply();
            };
            var error = function () {
                $rootScope.hideLoader();
                var err = arguments[0];
                if (err.responseText != null) $rootScope.showError({ message: err.responseText });
                else $rootScope.showError({ message: "Une erreur est survenue." });
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

            if ($scope.hasChanged() == true && startWorkflow == false) {
                $rootScope.showLoader("Enregistrement....");
                myDocComplete.save(context);
            }
            if ($scope.hasChanged() == true && startWorkflow == true) {
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
        else $('#myTabPanel a[href="#detail"]').tab("show");
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
    // PANE PAGE
    /***********************************************/
    function pageChanged(page, action) {
        page.set('action', action);
        $scope.isUpdate = true;
    };
    $scope.doTurn = function (id) {
        var elem = angular.element("#img-page-" + id);
        var prefix = "inbox-thumbnail-rotate";
        var r000 = elem.hasClass(prefix + "000");
        var r090 = elem.hasClass(prefix + "090");
        var r180 = elem.hasClass(prefix + "180");
        var r270 = elem.hasClass(prefix + "270");

        elem.removeClass(prefix + "000");
        elem.removeClass(prefix + "090");
        elem.removeClass(prefix + "180");
        elem.removeClass(prefix + "270");

        var rotation = 0;
        if (r000) {
            elem.addClass(prefix + "090");
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
            elem.addClass(prefix + "000");
            rotation = 0;
        }
        var page = $scope.pages.where({ id: id });
        page[0].set('rotation', rotation);
        pageChanged(page[0],'update');
    };
    $scope.moveUp = function (id) {
        var size = $scope.pages.length;
        var page = $scope.pages.where({ id: id });
        var currentIndex = page[0].get('pageNumber');
        if (currentIndex > 1) {
            var pageToMove = $scope.pages.where({ pageNumber: currentIndex - 1 });
            page[0].set('pageNumber', currentIndex - 1);
            pageToMove[0].set('pageNumber', currentIndex);
            pageChanged(page[0], 'update');
            pageChanged(pageToMove[0], 'update');
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
            pageChanged(page[0], 'update');
            pageChanged(pageToMove[0], 'update');
        }
    };
    $scope.deletePicture = function (id) {
        var size = $scope.pages.length;
        $scope.pages.remove(id);
        $scope.isUpdate = true;
        $scope.numeroter(1, size);
    };
    $scope.numeroter = function (startIndex, size) {
        var index = startIndex;
        var nb = startIndex;
        while (index <= size) {
            var page = $scope.pages.where({ pageNumber: index });
            if (page.length > 0) {
                if (page[0].get('action') != 'delete') {
                    page[0].set('pageNumber', nb++);
                    page[0].set('action', 'update');
                    $scope.isUpdate = true;
                }
            }
            index++;
        }
    };

    $scope.notDeletedPage = function (item) {
        return item.get("action") != "delete";
    };
}]);