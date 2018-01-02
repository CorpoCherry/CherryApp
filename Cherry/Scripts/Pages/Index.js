import mdcAutoInit from '@material/auto-init';
import { MDCTemporaryDrawer, MDCTemporaryDrawerFoundation, util } from '@material/drawer';
import { MDCRipple, MDCRippleFoundation, util2 } from '@material/ripple';
//const Slideout = require('slideout');

mdcAutoInit.register('MDCRipple', MDCRipple);

let drawer = new MDCTemporaryDrawer(document.querySelector('.mdc-temporary-drawer'));
document.querySelector('.menu').addEventListener('click', () => drawer.open = true);
//var slide = new Slideout({
//    'panel': document.querySelector('.mdc-permanent-drawer'),
//    'menu': document.querySelector('.main'),
//    'padding': 256,
//    'tolerance': 70
//});
//document.querySelector('.menu').addEventListener('click', function () {
//    slide.toggle();
//});

mdcAutoInit();