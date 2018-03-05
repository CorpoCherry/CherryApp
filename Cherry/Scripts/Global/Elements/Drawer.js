import { MDCTemporaryDrawer, MDCTemporaryDrawerFoundation, util } from '@material/drawer';

export function Drawer() {
    let drawer = new MDCTemporaryDrawer(document.querySelector('.mdc-drawer--temporary'));
    document.querySelector('.menu').addEventListener('click', () => drawer.open = true);

    $.each($('.slidemenu'), function(index , slidablemenu)
    {
        var slidebutton = $(slidablemenu).children('.slidebutton');
        slidebutton.click(function () {
            let slidearea = $(this).parent().children('.slidearea');
            if (slidearea.css("height") == "0px") {
                $.each($('.slidearea'), function (indexx, area)
                {
                    $(area).height("0px");
                });
                slidearea.height(slidearea.children("a").length * 48 + 2 + 'px');
            } else {
                slidearea.height("0px");
            }

        });
    });

    return drawer;
}
