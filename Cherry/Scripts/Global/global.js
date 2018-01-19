const PATH = '/sw.js';
const iosfix = require("./Fixes/ios_fix_links");

let isServiceWorkersSupport = ('serviceWorker' in navigator);

if (isServiceWorkersSupport) {
    console.log('Will service worker register?');
    navigator.serviceWorker.register(PATH).then(function () {
        console.log("Yes it did.");
    }).catch(function (err) {
        console.log("No it didn't. This happened: ", err)
    });
}

document.getElementById('slidebutton').addEventListener('click', function () {
    var body = document.getElementsByClassName('slideable')[0];
    if (body.id == 'expanded') {
        body.id = '';
    } else {
        body.id = 'expanded';
    };
});