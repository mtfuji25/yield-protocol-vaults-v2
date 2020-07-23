/*! Analogue.js; version 1.15.05 (1431137889); Copyright (c) 2015 Analogue Web Design LLC; Dual licensed: MIT/GPL */

var analogue = (function (document, root, window) {
    'use strict';

    var build = new Date(1431137889000),

        classes = [],

        detect = (function () {
            return {
                html5: (function (p) {
                    try {
                        p.innerHTML = '<nav/>';
                        return {
                            elements: p.childNodes.length === 1,
                            style: !!p.firstChild && (p.firstChild.currentStyle || window.getComputedStyle(p.firstChild, null)).display === 'block'
                        };
                    } finally {
                        p = null;
                    }
                }(document.createElement('p'))),
                // https://developer.mozilla.org/en-US/docs/Browser_detection_using_the_user_agent
                mobile: !!window.navigator.userAgent.match(/Mobi/)
            };
        }()),

        head = document.head || document.getElementsByTagName('head')[0] || root,

        shiv = function (element) {
            document.createElement(element);
        },

        splitter = /\w+/g,

        versioning = [1, String(build.getFullYear()).slice(2), String((build.getMonth() + 1) / 100).slice(2)];

    // Remove no-js class if present and apply detection classes to root
    root.className = root.className.replace(/\bno-js\b/, '');
    classes.push('js');
    if (detect.mobile) {
        classes.push('mobile');
    }
    root.className = (root.className.length ? ' ' : '') + classes.join(' ');

    // Shiv the document
    (function (css, elements, p) {
        if (!detect.html5.elements) {
            elements.replace(splitter, shiv);
        }
        if (!detect.html5.style) {
            p = document.createElement('p');
            p.innerHTML = 'x<style>' + css + '</style>';
            try {
                head.insertBefore(p.lastChild, head.firstChild);
            } finally {
                p = null;
            }
        }
    }(
        'article,aside,figcaption,figure,footer,header,hgroup,main,nav,section{display:block}mark{background:#FF0;color:#000}template{display:none}',
        'abbr article aside audio bdi canvas data datalist details dialog figcaption figure footer header hgroup main mark meter nav output picture progress section summary template time video'
    ));

    // Setup queue to safely use .ready() before including jQuery
    window.$ = window.jQuery = (function (queue, setTimeout) {
        var init = function ($) {
                $.each(queue, function (ignore, handler) {
                    $(handler);
                });
            },
            poll = function () {
                return window.jQuery.fn === undefined ? setTimeout(poll, 50) : init(window.jQuery);
            },
            push = {
                ready: function (handler) {
                    queue.push(handler);
                }
            };

        setTimeout(poll, 50);
        return function (handler) {
            if (handler === document || handler === undefined) {
                // Handles $(document).ready(handler) and $().ready(handler)
                return push;
            }
            // Handles $(handler)
            return queue.push(handler);
        };
    }([], window.setTimeout));

    // Avoid `console` errors in browsers that lack a console
    (function (methods, noop, console) {
        console = window.console = window.console || {};
        methods.replace(splitter, function (method) {
            if (console[method] === undefined) {
                console[method] = noop;
            }
        });
    }('assert clear count debug dir dirxml error exception group groupCollapsed groupEnd info log markTimeline profile profileEnd table time timeEnd timeline timelineEnd timeStamp trace warn', function () {
        return;
    }));

    return {
        detect: detect,
        version: versioning.join('.') + ' (' + parseInt(build.getTime() / 1e3, 10) + ')'
    };
}(this.document, this.document.documentElement, this));