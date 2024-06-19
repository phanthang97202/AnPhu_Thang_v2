function IsNullOrEmpty(value) {
    if (value !== undefined && value !== null && value.toString().trim().length > 0) {
        return false;
    }
    return true;
}

function setAdminCurrentTag(parents, parentText) {
    for (var i = 0; i < parents.length; i++) {
        var childLi = "";
        if (i !== 0) {
            childLi += "<li>";
        } else {
            childLi += "<li class=\"breadcrumb-remove-content\">";
        }

        if (parents[i] !== null && parents[i] !== undefined && parents[i].toString().trim().length > 0 && parents[i].toString().trim() !== '#') {
            childLi += "<a href='" + parents[i] + "'\>" + parentText[i] + "</a>";
        } else {
            childLi += "<span style=\"color: ##3b81ab;\">" + parentText[i] + "</span>";
        }
        childLi += "</li>";
        $('ul.breadcrumb').append(childLi);
    }
}

function setAdminCurrentUrl(url) {

    $('.nav-list a[href="' + url + '"]').each(function () {
        $(this).closest('li').addClass('active');
        $(this).closest('li').closest('ul').show();
        $(this).closest('li').closest('ul').closest('li').addClass('active open');

        $(this).closest('li').closest('ul').closest('li').closest('ul').closest('li').addClass('active open');
    });
}