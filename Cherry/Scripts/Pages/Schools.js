import mdcAutoInit from '@material/auto-init';
import { MDCRipple, MDCRippleFoundation, util2 } from '@material/ripple';
import { MDCTextField } from '@material/textfield';
import { Drawer } from '../Global/Elements/Drawer';
import { FullPageSearch } from '../Global/Elements/FullPageSearch';

let drawer = Drawer();
let fpsearch = FullPageSearch($('input[name="__RequestVerificationToken"]').val());


mdcAutoInit.register('MDCTextField', MDCTextField);
mdcAutoInit.register('MDCRipple', MDCRipple);
mdcAutoInit();