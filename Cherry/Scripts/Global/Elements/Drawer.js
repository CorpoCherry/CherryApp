import { MDCTemporaryDrawer, MDCTemporaryDrawerFoundation, util } from '@material/drawer';

export function Drawer() {
    let drawer = new MDCTemporaryDrawer(document.querySelector('.mdc-drawer--temporary'));
    document.querySelector('.menu').addEventListener('click', () => drawer.open = true);

    document.getElementById('slidebutton').addEventListener('click', function () {
        var body = document.getElementsByClassName('slideable')[0];
        if (body.id === 'expanded') {
            body.id = '';
        } else {
            body.id = 'expanded';
        }
    });

    return drawer;
}
