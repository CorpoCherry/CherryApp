import mdcAutoInit from '@material/auto-init';
import { MDCTemporaryDrawer, MDCTemporaryDrawerFoundation, util } from '@material/drawer';
import { MDCRipple, MDCRippleFoundation, util2 } from '@material/ripple';
import { Drawer } from '../Global/Elements/Drawer';
let drawer = Drawer();

mdcAutoInit.register('MDCRipple', MDCRipple);
mdcAutoInit();