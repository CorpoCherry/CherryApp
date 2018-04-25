var searchlist = [];
var searchfield;
var searchfield_inner;
var timer;
var resize_timer;
var verification;
var pagesbuttons;
var currentpage = 0;
var availablepages = 0;

function addToList(item) {
    var node = $("<a class='mdc-list-item'>" + item.officialName + "</a>");
    node.appendTo($("#search_panel_inner"));
    searchlist[searchlist.length] = {
        item: item,
        node: node
    };
}

export function getFromListByTag(tag) {
    return searchlist.find(x => x.item.tag === tag);
}
export function getFromListByNode(node) {
    return searchlist.find(x => x.node === node);
}
export function removeFromList(item) {
    searchlist.removeChild(item);
}
export function clearList() {
    for (var i = 0; i < searchlist.length; i++) {
        searchlist[i].node.remove();
    }
    searchlist = [];
    
}

export function FullPageSearch(token) {
    verification = token;
    searchfield = $("#search_textfield");
    searchfield_inner = $(searchfield.children("input"));
    pagesbuttons = $('#search_pagesbuttons');

    searchfield_inner.on("keyup", function () {
        showLoader();
        clearTimeout(timer);
        timer = setTimeout(function () {
            var fieldval = searchfield_inner.val();
            clearPages();
            if (fieldval === "") {
                getSearchData(calculateSpace, 0, "", verification);
            }
            else {
                getSearchData(calculateSpace, 0, fieldval, verification);
            }
        }, 250);
    });

    searchfield_inner.on('input', function () {
        var searchfield_label = $(searchfield.children("#search_textfield_label"));
        if (searchfield_inner.val() === "") {
            searchfield_label.show(100);
        }
        else {
            searchfield_label.hide(100);
        }
    });
    $(window).resize(function () {
        DisablePagesCounter();
        showLoader();
        clearTimeout(resize_timer);
        resize_timer = setTimeout(function () {
            var fieldval = searchfield_inner.val();
            if (fieldval === "") {
                getSearchData(calculateSpace, 0, "", verification);
            }
            else {
                getSearchData(calculateSpace, 0, fieldval, verification);
            }
        }, 250);
    });

    $("#search_pagesbuttons_back").click(function () { BackPage() });
    $("#search_pagesbuttons_next").click(function () { NextPage() });

    getSearchData(calculateSpace, 0, "", verification);
}

export function getSearchData(count, page, value, token) {
    $.ajax({
        url: window.location.pathname + "?handler=GetName",
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            count: count,
            page: page,
            name: value.trim()
        },
        success: function (result) {
            clearList();
            displayData(result);
        }
    });
}

function displayData(response) {
    if (response !== null) {
        for (var i = 0; i < response.schools.length; i++) {
            addToList(response.schools[i]);
        }
        CalculatePages(response.allin);
        displayPagesCounter();
    }
    else {
        //TODO: Infrom User about no connection
        console.warn("No Respones at:" + Date.now());
    }
    hideLoader();

}

function isBackAvailable() {
    if (currentpage === 0) return false;
    return true;
}

function isNextAvailable() {
    if (currentpage === availablepages) return false;
    return true
}

function NextPage() {
    if (isNextAvailable()) {
        currentpage = currentpage + 1;
        displayPagesCounter();
        getSearchData(calculateSpace, currentpage, searchfield_inner.val(), verification);
    } 
}
function BackPage() {
    if (isBackAvailable()) {
        currentpage = currentpage - 1;
        displayPagesCounter();
        getSearchData(calculateSpace, currentpage, searchfield_inner.val(), verification);
    }  
}

function clearPages() {
    currentpage = 0;
    displayPagesCounter();
}
function displayPagesCounter() {
    EnablePagesCounter();
    ShowHideNextBackButtons();
    $(pagesbuttons.children("#search_pagesbuttons_number")).html((1 + currentpage) + "/" + (1 + availablepages));
}

function ShowHideNextBackButtons() {
    if (isBackAvailable()) {
        $(pagesbuttons.children("#search_pagesbuttons_back")).css("visibility", "visible");
    }
    else {
        $(pagesbuttons.children("#search_pagesbuttons_back")).css("visibility", "hidden");
    }
    if (isNextAvailable()) {
        $(pagesbuttons.children("#search_pagesbuttons_next")).css("visibility", "visible");
    }
    else {
        $(pagesbuttons.children("#search_pagesbuttons_next")).css("visibility", "hidden");
    }
}

function DisablePagesCounter() {
    ShowHideNextBackButtons();
    currentpage = 0;
    availablepages = 0;
    $(pagesbuttons.hide());
}

function EnablePagesCounter() {
    $(pagesbuttons.show());
}
function CalculatePages(itemscount) {
    availablepages = Math.floor(itemscount / calculateSpace());
}

function showLoader() {
    $("#search_loading").show(150);
    $("#search_panel").hide(150);
}
function hideLoader() {
    $("#search_panel").show(150);
    $("#search_loading").hide(150);

}

function calculateSpace() {
    var spaceAval = $(window).height() - 128;
    return Math.floor(Math.ceil(spaceAval / 48)*1.5);
}
