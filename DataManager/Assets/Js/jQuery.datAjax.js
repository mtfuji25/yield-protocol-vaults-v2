/*! datAjax - jQuery Plugin; version 1.16.03 (1458514322); Copyright (c) 2016 Analogue Web Design LLC; Dual licensed: MIT/GPL */
(function (document, window, $) {
    'use strict';

    var build = new Date(1458514322000),

        callbacks = (function (registry) {
            return {
                get: function (name) {
                    return $.isFunction(registry[name]) ? registry[name] : $.noop;
                },
                set: function (object) {
                    $.extend(registry, object);
                }
            };
        }({})),

        confirm = $.ui && $.fn.dialog ? function (message, ok, settings) {
            var buttons = {},
                options = $.extend({}, $.fn.datajax.defaults.confirm, settings || {});

            buttons[options.cancel] = function () {
                $(this).dialog('close').dialog('destroy').remove();
            };
            buttons[options.ok] = function () {
                if ($.isFunction(ok)) {
                    ok();
                }
                $(this).dialog('close').dialog('destroy').remove();
            };
            $('<div class="dialog confirm">' + message + '</div>').dialog({
                buttons: buttons,
                dialogClass: 'ui-confirm',
                modal: true,
                open: function () {
                    $(this).closest('.ui-dialog').find('.ui-dialog-buttonpane button:eq(1)').focus();
                },
                resizable: false,
                title: options.title
            });
        } : function (message, ok) {
            if (window.confirm(message)) {
                ok();
            }
        },

        expando = 'datajax_' + build.getTime(),

        lower = function (string) {
            return (string || '').toLowerCase();
        },

        request = function (event, bypass) {
            var data = [],
                element,
                options = {},
                settings;

            event.preventDefault();
            element = $(this);
            if (element.data('ajax-confirm') && !bypass) {
                return confirm(element.data('ajax-confirm'), $.proxy(function () {
                    request.call(this, event, true);
                }, this));
            }
            $.each(element.data(), function (key, value) {
                if (/^ajax[A-Z]+/.test(key)) {
                    options[key.match(/^ajax([a-zA-Z0-9]*)/)[1].replace(/^[A-Z]/, lower)] = value;
                }
            });
            options.action = options.action || $.fn.datajax.defaults.action;
            if (options.loading) {
                options.loading = $(options.loading);
                options.loadingDuration = options.loadingDuration || 0;
            }
            options.target = options.target || this;
            if (event.type === 'submit') {
                data = element.serializeArray();
            }
            settings = {
                beforeSend: function (jqXHR, settings) {
                    var result = callbacks.get(options.beforeSend).apply(this, arguments);

                    if (options.beforeSend) {
                        settings.data = $.param(settings.data, false);
                    }
                    if (result !== false && options.loading) {
                        options.loading.show(options.loadingDuration);
                    }
                    return result;
                },
                cache: false,
                data: data,
                method: options.method || options.type || element.attr('method') || $.fn.datajax.defaults.method,
                processData: options.beforeSend ? false : true,
                url: options.url || element.attr('action') || element.attr('href')
            };
            $.ajax(settings).done(function (data, textStatus, jqXHR) {
                var contentType = jqXHR.getResponseHeader('Content-Type') || 'text/html';

                if (contentType.indexOf('application/x-javascript') === -1) {
                    switch (options.action.toLowerCase()) {
                    case 'append':
                        $(options.target).append(data);
                        break;
                    case 'after':
                        $(options.target).after(data);
                        break;
                    case 'before':
                        $(options.target).before(data);
                        break;
                    case 'hide':
                        $(options.target).hide();
                        break;
                    case 'prepend':
                        $(options.target).prepend(data);
                        break;
                    case 'remove':
                        $(options.target).remove();
                        break;
                    case 'remove-closest':
                        element.closest(options.target).remove();
                        break;
                    case 'replace':
                        $(options.target).empty().html(data);
                        break;
                    case 'replace-closest':
                        element.closest(options.target).empty().html(data);
                        break;
                    case 'update':
                        options.target = $(options.target);
                        if (options.target.is(':input')) {
                            options.target.val(data);
                        }
                        break;
                    }
                }
                callbacks.get(options.success || options.done).apply(this, arguments);
            }).fail(function () {
                callbacks.get(options.error || options.fail).apply(this, arguments);
            }).always(function () {
                if (options.loading) {
                    options.loading.hide(options.loadingDuration);
                }
                callbacks.get(options.complete || options.always).apply(this, arguments);
            });
        },

        versioning = [1, String(build.getFullYear()).slice(2), String((build.getMonth() + 1) / 100).slice(2)];

    function Plugin(element) {
        $(element).on('click', 'a.ajax', request).on('submit', 'form.ajax', request);
    }

    $.fn.datajax = function () {
        if (this.length === 0 && !$.isReady) {
            $(function () {
                $(this.selector, this.context).datajax();
            });
            return this;
        }
        return this.each(function () {
            if ($.data(this, expando) === undefined) {
                $.data(this, expando, new Plugin(this));
            }
        });
    };
    $.extend($.fn.datajax, {
        callbacks: function (callback) {
            switch ($.type(callback)) {
            case 'object':
                callbacks.set(callback);
                break;
            case 'string':
                return callbacks.get(callback);
            default:
                throw 'Parameter `callback` must be string or object literal';
            }
        },
        confirm: confirm,
        defaults: {
            action: 'none',
            confirm: {
                cancel: 'Cancel',
                ok: 'OK',
                title: 'Please Confirm'
            },
            method: 'GET'
        },
        expando: expando,
        version: versioning.join('.') + ' (' + parseInt(build.getTime() / 1e3, 10) + ')'
    });

    $(document).ready(function () {
        $(document).datajax();
    });
}(this.document, this, this.jQuery));