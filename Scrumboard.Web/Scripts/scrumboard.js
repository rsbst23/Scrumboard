jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

function getWorkItemType(pbiId) {
    var domItem = $('dl[id="' + pbiId + '"]');

    if (domItem.hasClass('pbi')) {
        return (domItem.hasClass('bug') ? 'Bug' : 'Story');
    }

    return 'Task';
}

function getAllPbis() {
    return $('dl.pbi').map(function (index, domElement) {
        return $(domElement);
    });
}

// Abbreviation management
//var abbreviationManager = new AbbreviationManager(function () {
//    return $("dd.abbr.owner:not([title=''])").map(function (index, domElement) {
//        return $(domElement).attr("title");
//    });
//}).abbreviationChanged(function (fullName, oldAbbreviation, newAbbreviation) {
//    $("dd.abbr.owner[title='" + fullName + "']").text(newAbbreviation);
//});

function recheckNameAbbreviations() {
    abbreviationManager.recheck();
}

// Story + Bug context menu
function pbiContextMenuItemSelected(itemKey, options) {
    var pbi = $(this),
        pbiId = pbi.attr('id'),
        menuPosition = $(options.$menu[0]).position(),
        taskForm,
        index;

    // Generate Tasks
    if (itemKey === 'generateTasks') {
        taskForm = $('#taskForm');

        // Position the task form at the position the context menu is currently at
        taskForm.css('top', menuPosition.top);
        taskForm.css('left', menuPosition.left);

        // Save the pbi id for later retrieval
        taskForm.data('pbiId', pbiId);

        taskForm.html('Loading...');

        // Show the form and finish up
        taskForm.show();

        taskForm.attr('src', './Task/Create?backlogItemId=' + taskForm.data('pbiId'));

        return;
    }
}

function editTask(task) {
    var menuPosition = task.position(),
        taskId = task.closest('dl[class^="sbi"]')[0].id,
        taskForm;

    taskForm = $('#taskForm');

    // Position the task form at the position the context menu is currently at
    taskForm.css('top', menuPosition.top);
    taskForm.css('left', menuPosition.left);

    // Save the pbi id for later retrieval
    taskForm.data('taskId', taskId);

    taskForm.html('Loading...');

    // Show the form and finish up
    taskForm.show();

    taskForm.attr('src', './Task/Edit/' + taskId);

    return false;
}

function editStory(story) {
    var storyId = story.closest('dl[class^="pbi"]')[0].id;

    window.open('/Story/Edit/' + storyId);

    return false;
}

function saveTasks() {
    var taskTypes = [],
        createTasksForm = $('#createTasksForm');

    // Determine the tasks to create
    createTasksForm.find('input[type=checkbox]:checked').each(function (index, checkbox) {
        taskTypes.push($(checkbox).val());
    });

    // If no tasks were selected, abort early
    if (taskTypes.length === 0) {
        return;
    }

    // Confirm that they wish to create the tasks in TFS
    $.confirm('This action will create tasks for this ' + getWorkItemType(createTasksForm.data('pbiId')).toLowerCase() +
              ' in TFS. Are you sure you want to continue?').done(function () {

                  // Create the tasks in TFS
                  LoadingScreen.show();
                  $.ajax({
                      type: 'POST',
                      url: './Scrumboard/GenerateTasks',
                      data: {
                          pbiId: createTasksForm.data('pbiId'),
                          taskTemplateIds: taskTypes
                      },
                      traditional: true
                  }).done(function (data) {
                      var icon = 'success',
                          message = data.nTasksCreated + ' task' + (data.nTasksCreated !== 1 ? 's were' : ' was') +
                              ' successfully created. ';
                      if (data.nNotRecreated > 0) {
                          message += data.nNotRecreated + ' task' + (data.nNotRecreated !== 1 ? 's were' : ' was') +
                              ' skipped, as the task template(s) responsible were marked as \'Do not recreate\' and a task was found ' +
                              'that already matched the generated task. ';
                          icon = 'warning';
                      }
                      message += 'The scrumboard will now refresh.';
                      alert(message, icon).done(function () {
                          refreshScrumboard({ scrollBack: true });
                      });
                  }).always(function () {
                      LoadingScreen.hide();
                  });
              });
}

function cancelCreateTasks() {
    $("#createTasksForm").hide();
}

// Task context menu
function taskContextMenuItemSelected(itemKey) {
    var task = $(this),
        taskId = task.attr('id'),
        workRemainingElement = task.find('dd.workRemaining');

    // Assign to me
    if (itemKey === 'assignMe') {
        assignTaskTo(taskId, getCurrentUserDisplayName());
        return;
    }

    // Assign to a team member
    if (/^at-/.test(itemKey)) {
        assignTaskTo(taskId, itemKey.substring(3));
        return;
    }

    // Unassign
    if (itemKey === 'unassign') {
        unassignTaskFrom(taskId);
        return;
    }

    // Add hours
    if (/^addHour/.test(itemKey)) {
        addTaskHours(taskId, workRemainingElement, workRemainingElement.text(), itemKey.substr('addHour'.length));
        return;
    }

    // Sync hours
    if (itemKey === 'syncHours') {
        syncTaskHours(taskId, workRemainingElement, workRemainingElement.text());
        return;
    }

    // Burn remaining hours
    if (itemKey === 'burnRemaining') {
        removeTaskHours(taskId, workRemainingElement, workRemainingElement.text(), 'all');
        return;
    }

    // Burn hours
    if (/^burnHour/.test(itemKey)) {
        removeTaskHours(taskId, workRemainingElement, workRemainingElement.text(), itemKey.substr('burnHour'.length));
        return;
    }
}

function assignTaskTo(taskId, assignToName, suppressConfirm) {
    var ownerElement = $('dl#' + taskId + ' > dd.owner'),
        confirmPromise,
        assignedTo = ownerElement.attr('title');

    if (assignedTo.length > 0 && !suppressConfirm) {
        confirmPromise = $.confirm('This task is currently assigned to ' + assignedTo + '. Are you sure you would like to remove ' +
            assignedTo.substring(0, assignedTo.indexOf(' ')) + ' and assign yourself?', 'notice');
    } else {
        confirmPromise = $.Deferred().resolve().promise();
    }

    confirmPromise.done(function () {
        LoadingScreen.show();
        $.ajax({
            type: 'POST',
            url: './Scrumboard/AssignTask',
            data: {
                id: taskId,
                name: assignToName,
                currentName: assignedTo
            }
        }).done(function (data) {
            if (data.success) {
                ownerElement.attr("title", data.displayName).text(data.abbreviation);
                recheckNameAbbreviations();
                alert('Task number ' + taskId + ' has been successfully assigned to ' +
                    (data.displayName === getCurrentUserDisplayName() ? 'you' :
                            data.displayName) + '.', 'success');
            } else if (data.requireConfirm) {
                ownerElement.attr('title', data.newCurrentName).text(data.abbreviation);
                recheckNameAbbreviations();
                $.confirm(data.confirmMessage, 'notice').done(function () {
                    assignTaskTo(taskId, assignToName, true);
                });
            } else {
                alert(data.message, 'error');
            }
        }).always(function () {
            LoadingScreen.hide();
        });
    });
}

function unassignTaskFrom(taskId) {
    var ownerElement = $('dl#' + taskId + ' > dd.owner'),
        assignedTo = ownerElement.attr('title')

    LoadingScreen.show();
    $.ajax({
        type: 'POST',
        url: './Scrumboard/UnassignTask',
        data: {
            id: taskId,
            currentName: assignedTo
        }
    }).done(function (data) {
        if (data.success) {
            ownerElement.attr('title', '').text('');
            recheckNameAbbreviations();
            alert('Task number ' + taskId + ' has been successfully unassigned from ' +
                (assignedTo === getCurrentUserDisplayName() ? 'you' : assignedTo) + '.',
                'success');
        } else if (data.requireConfirm) {
            ownerElement.attr('title', data.newCurrentName).text(data.abbreviation);
            recheckNameAbbreviations();
            $.confirm(data.confirmMessage, 'notice').done(function () {
                unassignTaskFrom(taskId);
            });
        } else {
            alert(data.message, 'error');
        }
    }).always(function () {
        LoadingScreen.hide();
    });
}

function addTaskHours(taskId, workRemainingElement, currentHours, addAmount) {
    LoadingScreen.show();
    $.ajax({
        type: 'POST',
        url: './Scrumboard/AddTaskHours',
        data: {
            id: taskId,
            addAmount: addAmount,
            currentHours: currentHours,
            inTaskingMode: $('#inTaskingMode').is(':checked')
        }
    }).done(function (data) {
        if (data.success) {
            workRemainingElement.attr('title', 'Work Remaining: ' + data.resultHours + ' hour' +
                (Number.parseInt(data.resultHours) !== 1 ? 's' : '')).text(data.resultHours);
            alert('Task number ' + taskId + ' has been given ' + addAmount + ' additional hour' +
                (Number.parseInt(addAmount) !== 1 ? 's' : '') + '.', 'success');
        } else {
            if (data.requireConfirm) {
                workRemainingElement.attr('title', 'Work Remaining: ' + data.newCurrentHours + ' hour' +
                    (Number.parseInt(data.newCurrentHours) !== 1 ? 's' : '')).text(data.newCurrentHours);
                $.confirm(data.confirmMessage, 'notice').done(function () {
                    addTaskHours(taskId, workRemainingElement, data.newCurrentHours, addAmount);
                });
            } else {
                alert(data.message, 'error');
            }
        }
    }).always(function () {
        LoadingScreen.hide();
    });
}

function removeTaskHours(taskId, workRemainingElement, currentHours, removeAmount) {
    LoadingScreen.show();
    $.ajax({
        type: 'POST',
        url: './Scrumboard/RemoveTaskHours',
        data: {
            id: taskId,
            removeAmount: removeAmount,
            currentHours: workRemainingElement.text(),
            inTaskingMode: $('#inTaskingMode').is(':checked')
        }
    }).done(function (data) {
        if (data.success) {
            workRemainingElement.attr('title', 'Work Remaining: ' + data.resultHours + ' hour' +
                (Number.parseInt(data.resultHours) !== 1 ? 's' : '')).text(data.resultHours);
            if (removeAmount === 'all') {
                alert('All hours have been burned for task number ' + taskId + '.', 'success');
            } else {
                alert(removeAmount + ' hour' + (Number.parseInt(removeAmount) !== 1 ? 's' : '') + ' have been burned ' +
                    'for task number ' + taskId + '.', 'success');
            }
        } else {
            if (data.requireConfirm) {
                workRemainingElement.attr('title', 'Work Remaining: ' + data.newCurrentHours + ' hour' +
                    (Number.parseInt(data.newCurrentHours) !== 1 ? 's' : '')).text(data.newCurrentHours);
                $.confirm(data.confirmMessage, 'notice').done(function () {
                    removeTaskHours(taskId, workRemainingElement, data.newCurrentHours, removeAmount);
                });
            } else {
                alert(data.message, 'error');
            }
        }
    }).always(function () {
        LoadingScreen.hide();
    });
}

function setupScrumboardRow(trElement) {
    var tr = $(trElement);

    // Make the tasks draggable
    tr.find(".sbi:not(.pbi)").draggable({
        cancel: "a.ui-icon", // clicking an icon won't initiate draggin
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: "document",
        cursor: "ew-resize",
        handle: ".handle",
        axis: "x",
        scope: tr.attr("id"),
        zIndex: 1700
    });

    // Make the appropriate columns droppable
    tr.children("td[data-status]").droppable({ //"td:not(.pbiColumn)"
        accept: ".sbi",
        scope: tr.attr("id"),
        hoverClass: "ui-state-highlight",
        tolerance: "pointer",
        drop: function (event, ui) {
            // 'this': the destination td the task was dropped onto
            if (ui.draggable.parent().index() === $(this).index()) {
                // dropped the task into the same column
                ui.draggable.addClass("returningAfterDrag").animate({ left: "", top: "" }, "fast", function () {
                    $(this).removeClass("returningAfterDrag");
                });
                return;
            }

            // Move the task to the appropriate DOM column
            var oldParent = ui.draggable.parent();
            var tasksWithHigherRelativeValue = $(this).children(".sbi:not(.pbi)").filter(function () {
                return ($(this).data("relativevalue") >= ui.draggable.data("relativevalue"));
            });

            if (tasksWithHigherRelativeValue.length > 0) {
                tasksWithHigherRelativeValue.last().after(ui.draggable);
            } else {
                $(this).prepend(ui.draggable);
            }

            ui.draggable.css("top", "");
            ui.draggable.css("left", "");

            // Update the task in TFS
            var id = ui.draggable.attr("id");
            var status = $(this).data("status");

            if (status !== "To Do" && status !== "In Progress" && status !== "Done") {
                alert("The status '" + status + "' is an invalid destination. Please let somebody know if you have encountered this in error (OldParent Id: " +
                    $(oldParent).attr('id') + ".", "error");
                $(oldParent).append(ui.draggable);
                return;
            }

            $.ajax({
                type: "POST",
                url: "/Task/EditTaskStatus",
                data: { 'id': id, 'status': status }
            }).done(function (data) {
                if (!data) {
                    alert('An error occurred updating the task status');
                    $(oldParent).append(ui.draggable);
                }
            }).fail(function (data) {
                $(oldParent).append(ui.draggable);
            });
        }
    });
}

function refreshScrumboard(refreshParameters) {
    var queryInfo = getCurrentQueryInfo(),
        isAssignedToQuery = queryInfo.isAssignedToQuery,
        isReleaseQuery = queryInfo.isReleaseQuery,
        releaseName = queryInfo.releaseName,
        selectedQueryId = queryInfo.selectedQueryId,
        assignedToMember = queryInfo.assignedToMember,
        showBacklogWithStatus = $("#showBacklogWithStatus").val(),
        taskStatusFilter = $("#taskStatusFilter").val(),
        scrollBack = false;

    LoadingScreen.show();

    if (refreshParameters !== undefined && refreshParameters !== null) {
        if (refreshParameters.isReleaseQuery !== undefined) {
            isReleaseQuery = refreshParameters.isReleaseQuery;
        }

        if (refreshParameters.isAssignedToQuery !== undefined) {
            isAssignedToQuery = refreshParameters.isAssignedToQuery;
        }

        if (refreshParameters.releaseName !== undefined) {
            releaseName = refreshParameters.releaseName;
        }

        if (refreshParameters.assignedToMember !== undefined) {
            assignedToMember = refreshParameters.assignedToMember;
        }

        if (refreshParameters.selectedQueryId !== undefined) {
            selectedQueryId = refreshParameters.selectedQueryId;
        }

        if (refreshParameters.scrollBack !== undefined) {
            scrollBack = refreshParameters.scrollBack;
        }
    }

    var newWindowLocation = resolveUrl("./Scrumboard?");

    if (isAssignedToQuery) {
        newWindowLocation += "assignedTo=" + assignedToMember;
    } else if (isReleaseQuery) {
        newWindowLocation += "release=" + releaseName;
    } else {
        newWindowLocation += "queryId=" + selectedQueryId;
    }

    newWindowLocation += "&showBacklogWithStatus=" + showBacklogWithStatus + "&taskStatusFilter=" + taskStatusFilter;
    if (scrollBack) {
        newWindowLocation += "&scrollBack=" + $(document).scrollTop();
    }

    window.location = newWindowLocation;
}

function closeTaskForm() {
    var taskForm = $('#taskForm');
    taskForm.hide();
}

function sbiContextMenuItemSelected(itemKey, options) {
    var sbi = $(this),
        sbiId = sbi.attr('id'),
        menuPosition = $(options.$menu[0]).position(),
        taskForm,
        index,
        taskId = sbi.closest('dl[class^="sbi"]')[0].id;;

    // Generate Tasks
    //if (itemKey === 'deleteTask') {
    if (confirm('Are you sure you want to delete this task')) {
        $.ajax({
            type: 'POST',
            url: './Task/DeleteTaskFromScrumboard',
            data: {
                id: taskId
            }
        }).done(function (data) {
            location.reload();
        });
    }
    //}
}