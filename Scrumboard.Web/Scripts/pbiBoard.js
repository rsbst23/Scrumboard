function onSbiClicked(e, sbiElement) {
    if (e.button != 0) {
        return;
    }

    if (e.target != null && $(e.target).is("a") || $(sbiElement).hasClass("returningAfterDrag")) {
        return;
    }

    $(sbiElement).toggleClass("selected");
}