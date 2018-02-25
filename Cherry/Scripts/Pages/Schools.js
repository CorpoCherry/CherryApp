import mdcAutoInit from '@material/auto-init';
import { MDCRipple, MDCRippleFoundation, util2 } from '@material/ripple';
import { MDCTextField } from '@material/textfield';
import { Drawer } from '../Global/Elements/Drawer';
let drawer = Drawer();
var token = $('input[name="__RequestVerificationToken"]').val();
var school;


$(document).ready(function () {
    var search = new MDCTextField(document.querySelector("#search"));
    var schoolList = document.querySelector("#schoolList");
    var timer;

    document.querySelector("#search_field").addEventListener("keyup", function () {
        showLoader();
        clearList();
        clearTimeout(timer);
        timer = setTimeout(function () {
            if (search.value === "") {
                loadByPage(0, token);
                
            }
            else {
                loadByName(search.value, token);
            }
        }, 250);
    });

    $('#search_field').on('input', function () {
        if (search.value === "") {
            $("#search_label").show(100);
            loadByPage(0);
        }
        else {
            $("#search_label").hide(100);
        }
    });

    loadByPage(0);
});

function loadByPage(page) {
    $.ajax({
        url: window.location.pathname + "?handler=GetPage",
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            id: 0
        },
        success: function (result) {
            clearList();
            displayData(result);
        }
    });
}
function loadByName(name) {
    $.ajax({
        url: window.location.pathname + "?handler=GetByName",
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            name: name.trim()
        },
        success: function (result) {
            clearList();
            displayData(result);
        }
    });
}
var va = function () {
    //TODO: Disallow clicking once again on same button
    loadSchool($(this).attr("tag"));
}

function displayData(response) {
    if (response !== null) {

        for (var i = 0; i < response.length; i++) {
            var item = "<a class='mdc-list-item' href='#' tag='" + response[i].tag + "'>" + response[i].officialName + "</a>";
            var tag = '[tag=' + response[i].tag + ']';
            $('body').on('click.loadschool', tag, va);
            $("#schoolList").append(item);

            //TODO: Add Ripples
            //var a = document.querySelector(tag);
            //var x = new MDCRipple(a);
            //var b = document.querySelector(tag);
        }
    }
    hideLoader();    
}
function loadSchool(searchable_tag) {
    $.ajax({
        url: window.location.pathname + "?handler=GetSchool",
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            tag: searchable_tag
        },
        success: function (result) {

            $(".noselection").css('visibility', 'collapse');
            $(".selection").css('visibility', 'visible');
            $(".selection").html(JSON.stringify(result).replace(/,/g, '<br>'));
            school = result;
        }
    });
}
function clearList() {
    while (schoolList.firstChild) {
        var x = $(schoolList.firstChild).attr("tag");
        var tag = '[tag=' + x + ']';
        $('body').off('click.loadschool', tag, va);
        try {
            var a = document.querySelector(tag);
            a.MDCRipple.destroy;
        }
        catch(err){}
        schoolList.removeChild(schoolList.firstChild);
    }
}

function showLoader() {
    $(".loading_panel").show(150);
    $(".panel-inside").hide(150);
}
function hideLoader() {
    $(".panel-inside").show(150);
    $(".loading_panel").hide(150);
    
}
mdcAutoInit.register('MDCTextField', MDCTextField);
mdcAutoInit.register('MDCRipple', MDCRipple);
mdcAutoInit();
