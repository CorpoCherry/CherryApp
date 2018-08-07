import mdcAutoInit from '@material/auto-init';
import { MDCRipple, MDCRippleFoundation, util2 } from '@material/ripple';
import { MDCTextField } from '@material/textfield';
import { Drawer } from '../Global/Elements/Drawer';
import { FullPageSearch } from '../Global/Elements/FullPageSearch';
import { MDCDialog } from '@material/dialog';

var ItemSelection = function (data) {
    $("#search_selection_name").html(data.officialName);
    $("#search_selection_tag").html(data.tag);
    $("#search_selection_first").css("display", "none");
    $("#search_selection").css("display", "flex");
}

let drawer = Drawer();
let fpsearch = FullPageSearch($('input[name="__RequestVerificationToken"]').val(), MDCRipple, ItemSelection);

const dialog = new MDCDialog(document.querySelector('#delete-dialog'));
const editdialog = new MDCDialog(document.querySelector('#edit-dialog'));
dialog.listen('MDCDialog:accept', function () {
    console.log('school is being removed');
})

document.querySelector('#search_selection_operations_delete').addEventListener('click', function (evt) {
    dialog.lastFocusedTarget = evt.target;
    dialog.show();
})

document.querySelector('#search_selection_operations_edit').addEventListener('click', function (evt) {
    editdialog.lastFocusedTarget = evt.target;
    editdialog.show();
})




mdcAutoInit.register('MDCTextField', MDCTextField);
mdcAutoInit.register('MDCRipple', MDCRipple);
mdcAutoInit();