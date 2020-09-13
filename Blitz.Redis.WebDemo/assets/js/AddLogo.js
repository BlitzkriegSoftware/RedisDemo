var addLogoDo = function (e) {
    if (e) {
        e.insertAdjacentHTML('afterbegin', '<img src="/assets/images/favicon-32x32.png" class="logo">');
    }
};

var addLogo = function () {
    var e = document.querySelector('.title');
    if (e) {
        addLogoDo(e);
    } else {
        setTimeout(function () { addLogo(); }, 500);
    }
};

function ready(callback) {
    // in case the document is already rendered
    if (document.readyState !== 'loading') callback();
    // modern browsers
    else if (document.addEventListener) document.addEventListener('DOMContentLoaded', callback);
    // IE <= 8
    else document.attachEvent('onreadystatechange', function () {
        if (document.readyState === 'complete') callback();
    });
}

ready(addLogo);