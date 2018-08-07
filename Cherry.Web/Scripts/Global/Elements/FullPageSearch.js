var sensorx = require('../../../node_modules/resize-sensor/src/ResizeSensor');
var searchlist = [];
var searchfield;
var searchfield_inner;
var timer;
var resize_timer;
var verification;
var pagesbuttons;
var currentpage = 0;
var availablepages = 0;
var MDCRipple;
var OnSelection;

//TODO: Disable selection same item again

function addToList(item) {
    var node = $("<a class='mdc-list-item search_item'>" + item.officialName + "</a>");
    node.click(function () { getItem(item.tag); });
    node.appendTo($("#search_panel_inner"));
    item.ripple = new MDCRipple(node[0]);
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
        searchlist[i].item.ripple.destroy();
        searchlist[i].node.remove();
    }
    searchlist = [];
    
}
export function FullPageSearch(token, ripple, selectionfunction) {
    MDCRipple = ripple;
    verification = token;
    OnSelection = selectionfunction;
    searchfield = $("#search_textfield");
    searchfield_inner = $(searchfield.children("input"));
    pagesbuttons = $('#search_pagesbuttons');

    searchfield_inner.on("keyup", function () {
        DisablePagesCounter();
        showLoader();
        clearTimeout(timer);
        timer = setTimeout(function () {
            var fieldval = searchfield_inner.val();
            clearPages();
            if (fieldval === "") {
                getSearchData(calculateSpace, 0, "");
            }
            else {
                getSearchData(calculateSpace, 0, fieldval);
            }
        }, 250);
    });

    searchfield_inner.on('input', function () {
        var searchfield_label = $(searchfield.children("#search_textfield_label"));
        if (searchfield_inner.val() === "") {
            searchfield_label.show(100);
            getSearchData(calculateSpace, 0, "");
        }
        else {
            searchfield_label.hide(100);
        }
    });
    var xss = new sensorx(document.getElementById('search_side'), function () {
        DisablePagesCounter();
        showLoader();
        clearTimeout(resize_timer);
        resize_timer = setTimeout(function () {
            var fieldval = searchfield_inner.val();
            if (fieldval === "") {
                getSearchData(calculateSpace, 0, "");
            }
            else {
                getSearchData(calculateSpace, 0, fieldval);
            }
        }, 250);
    });

    $("#search_pagesbuttons_back").click(function () { BackPage() });
    $("#search_pagesbuttons_next").click(function () { NextPage() });

    getSearchData(calculateSpace, 0, "");
}

export function getSearchData(count, page, value) {
    $.ajax({
        url: window.location.pathname + "?handler=GetName",
        type: 'POST',
        data: {
            __RequestVerificationToken: verification,
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

export function getItem(tag) {
    $.ajax({
        url: window.location.pathname + "?handler=GetItem",
        type: 'POST',
        data: {
            __RequestVerificationToken: verification,
            tag: tag
        },
        success: function (result) {
            OnSelection(result);
        }
    });
}

function showInfo(info) {
    var info_search = $("#search_info");
    info_search.html(info);
    info_search.css('display', 'flex');
    info_search.stop().animate(
        {
            opacity: '1'
        }, 50, 'swing');
}
function hideInfo() {
    $("#search_info").stop().animate(
        {
            opacity: '0'
        }, 50, 'swing');
    $("#search_info").css('display', 'none');

}

function displayData(response) {
    if (response !== null) {
        if (response.schools.length === 0) {
            showInfo("Brak wyników...<br/>Spróbuj użyć innego wyrażenia");    
        }
        for (var i = 0; i < response.schools.length; i++) {
            addToList(response.schools[i]);
        }
        CalculatePages(response.allin);
        displayPagesCounter();
    }
    else {
        showInfo("Błąd połączenia...<br/>Spróbuj ponownie za jakiś czas");
        console.warn("No Respone at:" + Date.now());
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
        getSearchData(calculateSpace, currentpage, searchfield_inner.val());
    } 
}
function BackPage() {
    if (isBackAvailable()) {
        currentpage = currentpage - 1;
        displayPagesCounter();
        getSearchData(calculateSpace, currentpage, searchfield_inner.val());
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
    $(pagesbuttons.stop().animate(
        {
            marginBottom: '-70px'
        }, 50, 'swing'));
}

function EnablePagesCounter() {
    $(pagesbuttons.stop().animate(
        {
            marginBottom: '0px'
        }, 50, 'swing'));
}
function CalculatePages(itemscount) {
    availablepages = Math.floor(itemscount / calculateSpace());
}

function showLoader() {
    hideInfo();
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
