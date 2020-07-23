/*! html5forms - jQuery Plugin; version 1.16.09 (1473350967); Copyright (c) 2016 Analogue Web Design LLC; Dual licensed: MIT/GPL */
(function (document, $) {
    'use strict';

    var build = new Date(1473350967366),

        detect = (function () {
            var attributes = {},
                input = document.createElement('input'),
                textarea = document.createElement('textarea'),
                types = {};

            attributes.maxlength = textarea.maxLength !== undefined;
            try {
                return {
                    attributes: (function (props) {
                        $.each(props.split(' '), function () {
                            attributes[this.toLowerCase()] = input[this] !== undefined;
                        });
                        return attributes;
                    }('step required pattern formNoValidate min max placeholder autofocus')),
                    checkValidity: typeof input.checkValidity === 'function',
                    setCustomValidity: input.setCustomValidity !== undefined,
                    types: (function (props, smile, supported) {
                        $.each(props.split(' '), function (ignore, type) {
                            input.setAttribute('type', type);
                            supported = input.type !== 'text';
                            if (supported) {
                                input.value = smile;
                                if (/^(email|url)$/.test(type)) {
                                    supported = input.checkValidity && input.checkValidity() === false;
                                } else {
                                    supported = input.value !== smile;
                                }
                            }
                            types[type] = !!supported;
                        });
                        return types;
                    }('url tel number email date color', ':)'))
                };
            } finally {
                input = textarea = null;
            }
        }()),

        expando = 'html5forms_' + build.getTime(),

        focusable = function (element) {
            return element && element.offsetWidth > 0 && element.offsetHeight > 0 && element.style.display !== 'none' && element.style.visibility !== 'hidden';
        },

        format = (function (pattern) {
            return function (string) {
                var args = [].slice.call(arguments, 1);

                return string.replace(pattern, function (match, i) {
                    return args[i] !== undefined ? args[i] : match;
                });
            };
        }(/\{(\d+)\}/g)),

        isEmpty = (function (pattern) {
            return function (value) {
                return value === null || value.length === 0 || pattern.test(value);
            };
        }(/^\s+$/)),

        maxlength = (function (nodeValue) {
            return function () {
                var length = parseInt(nodeValue(this.getAttributeNode('maxlength')), 10);

                if (this.value.length > length) {
                    this.value = this.value.slice(0, length);
                }
            };
        }(function (node) {
            return node ? node.nodeValue : '';
        })),

        placeholder = {
            blur: function (event) {
                var element = $(event.target || this),
                    text = element.attr('placeholder'),
                    value = element.val();

                if (isEmpty(value)) {
                    element.val(text).addClass('placeholder');
                }
            },
            focus: function (event) {
                var element = $(event.target || this),
                    text = element.attr('placeholder'),
                    value = element.val();

                if (value === text) {
                    element.val('').removeClass('placeholder');
                }
            }
        },

        placeholders = 'input[placeholder],textarea[placeholder]',

        rules = (function (pattern) {
            var checkDate = function (value) {
                    var date,
                        day,
                        match = pattern.date.exec(value),
                        month,
                        year;

                    if (!match) {
                        return false;
                    }
                    month = parseInt(match[1], 10) || parseInt(match[5], 10);
                    day = parseInt(match[2], 10) || parseInt(match[6], 10);
                    year = parseInt(match[3], 10) || parseInt(match[4], 10);
                    date = new Date(year, month - 1, day);
                    if (date.getDate() !== day || (date.getMonth() + 1) !== month || date.getFullYear() !== year) {
                        return false;
                    }
                    return true;
                },
                checkGroup = function (element, form) {
                    return !!$('input[name="' + element.name + '"]:checked', form);
                };

            return {
                date: {
                    native: false, /*detect.types.date,*/
                    selector: 'input[type=date],input[data-type=date]',
                    validate: function (element) {
                        var value = element.val();

                        if (value && !checkDate(value)) {
                            return format($.fn.html5forms.message.date, value);
                        }
                        return true;
                    }
                },
                email: {
                    native: detect.types.email,
                    selector: 'input[type=email],input[data-type=email]',
                    validate: function (element) {
                        var value = element.val();

                        if (value && !pattern.email.test(value)) {
                            return format($.fn.html5forms.message.email, value);
                        }
                        return true;
                    }
                },
                number: {
                    native: detect.types.number && detect.attributes.max && detect.attributes.min && detect.attributes.step,
                    selector: 'input[type=number],input[data-type=number]',
                    validate: function (element) {
                        var max = element.attr('max'),
                            min = element.attr('min'),
                            step = element.attr('step'),
                            value = element.val();

                        if (!isEmpty(value)) {
                            if (!$.isNumeric(value)) {
                                return format($.fn.html5forms.message.number, value);
                            }
                            min = $.isNumeric(min) ? parseFloat(min) : value;
                            value = parseFloat(value);
                            if (value < min) {
                                return format($.fn.html5forms.message.min, min);
                            }
                            max = $.isNumeric(max) ? parseFloat(max) : value;
                            if (value > max) {
                                return format($.fn.html5forms.message.max, max);
                            }
                            if (step !== 'any') {
                                step = $.isNumeric(step) ? parseFloat(step) : 1;
                                if ((value - min) % step !== 0) {
                                    return format($.fn.html5forms.message.step, step);
                                }
                            }
                        }
                        return true;
                    }
                },
                pattern: {
                    native: detect.attributes.pattern,
                    selector: 'input[pattern]',
                    validate: function (element) {
                        var regex = new RegExp('^(?:' + element.attr('pattern') + ')$'),
                            title = element.attr('title'),
                            value = element.val();

                        if (value && !regex.test(value)) {
                            return format($.fn.html5forms.message.pattern, title);
                        }
                        return true;
                    }
                },
                required: {
                    native: detect.attributes.required,
                    selector: 'input[required],select[required],textarea[required]',
                    validate: function (element) {
                        var index,
                            title = element.attr('title'),
                            value = element.val();

                        switch (element.prop('tagName')) {
                        case 'INPUT':
                        case 'TEXTAREA':
                            switch (element.attr('type')) {
                            case 'checkbox':
                                if (!element.prop('checked')) {
                                    return format($.fn.html5forms.message.checkbox, title);
                                }
                                break;
                            case 'radio':
                                if (!checkGroup(element[0], element.closest('form'))) {
                                    return format($.fn.html5forms.message.radio, title);
                                }
                                break;
                            default:
                                if (isEmpty(value) || element.attr('placeholder') === value) {
                                    return format($.fn.html5forms.message.required, title);
                                }
                            }
                            break;
                        case 'SELECT':
                            index = element[0].selectedIndex;
                            if (index === 0 || isEmpty(element[0].options[index].text)) {
                                return format($.fn.html5forms.message.select, title);
                            }
                            break;
                        }
                        return true;
                    }
                },
                url: {
                    native: detect.types.url,
                    selector: 'input[type=url],input[data-type=url]',
                    validate: function (element) {
                        var value = element.val();

                        if (value && !pattern.url.test(value)) {
                            return format($.fn.html5forms.message.url, value);
                        }
                        return true;
                    }
                }
            };
        }({
            date: /^(\d{1,2})[\s\.\/\-](\d{1,2})[\s\.\/\-](\d{2}|\d{4})$|^(\d{2}|\d{4})[\s\.\/\-](\d{1,2})[\s\.\/\-](\d{1,2})$/,
            email: /^(?:[a-zA-Z0-9.!#$%&'*+\/=?\^_`{|}~\-]+@[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*)$/,
            url: /^(?:(?:http|ftp|https):\/\/[\w\-_]+(?:\.[\w\-_]+)+(?:[\w\-\.,@?\^=%&amp;:\/~\+#]*[\w\-\@?\^=%&amp;\/~\+#])?)/
        })),

        style = '',

        validate = {
            form: function (event) {
                var elements = $('input,select,textarea', this),
                    form = $(this),
                    invalid = false,
                    valid;

                if (form.attr('novalidate')) {
                    return true;
                }
                if (detect.checkValidity) {
                    // Force fire checkValidity() to work around bug in Webkit browsers that support this method, but don’t call it when form is submitted.
                    invalid = !this.checkValidity();
                }
                if (!detect.attributes.placeholder) {
                    $(placeholders, form).each(placeholder.focus);
                }
                elements.each(function () {
                    valid = validate.field(this);
                    if (valid !== true && !invalid) {
                        invalid = true;
                        if (!detect.setCustomValidity) {
                            $('<div/>').addClass('validation-bubble').html('<div class="validation-bubble-arrow">&#9650;</div><div class="validation-bubble-icon"></div><div class="validation-bubble-message">' + valid + '</div>').css({
                                marginLeft: '-' + (this.offsetWidth + 9) + 'px',
                                marginTop: (this.offsetHeight + 9) + 'px'
                            }).insertAfter(this);
                            if (focusable(this)) {
                                this.focus();
                            }
                        }
                    }
                });
                if (!invalid) {
                    form.addClass('invalid').removeClass('valid');
                    return true;
                }
                form.addClass('valid').removeClass('invalid');
                if (!detect.attributes.placeholder) {
                    $(placeholders, form).each(placeholder.blur);
                }
                event.stopImmediatePropagation();
                return false;
            },
            field: function (event) {
                var element = $(event.target || event),
                    rule,
                    valid = true,
                    validationBubble = element.next('div.validation-bubble:first');

                if (validationBubble.length) {
                    validationBubble.remove();
                }
                for (rule in rules) {
                    if (rules.hasOwnProperty(rule) && !rules[rule].native && element.is(rules[rule].selector)) {
                        valid = rules[rule].validate(element);
                        if (valid !== true) {
                            if (detect.setCustomValidity) {
                                element[0].setCustomValidity(valid);
                            } else {
                                element.addClass('invalid').removeClass('valid');
                                if (element.attr('type') === 'radio') {
                                    $('[name="' + element.attr('name') + '"]').addClass('invalid').removeClass('valid');
                                }
                            }
                            return valid;
                        }
                        if (detect.setCustomValidity) {
                            element[0].setCustomValidity('');
                        } else {
                            element.addClass('valid').removeClass('invalid');
                            if (element.attr('type') === 'radio') {
                                $('[name="' + element.attr('name') + '"]').addClass('valid').removeClass('invalid');
                            }
                        }
                    }
                }
                return true;
            }
        },

        versioning = [1, String(build.getFullYear()).slice(2), String((build.getMonth() + 1) / 100).slice(2)];

    function Plugin(form) {
        form = $(form);
        if (form.prop('tagName') !== 'FORM') {
            return;
        }
        if (!detect.attributes.placeholder) {
            form.on('blur', placeholders, placeholder.blur).on('focus', placeholders, placeholder.focus);
            $(placeholders, form).each(placeholder.blur);
        }
        form.on('click', 'button:submit,input:submit', function (event) {
            if ($(event.target).attr('formnovalidate')) {
                form.off('submit', validate.form);
            }
        });
        if (!detect.attributes.maxlength) {
            form.on('keypress keyup', 'textarea[maxlength]', maxlength);
        }
        form.on('submit', validate.form).on('blur change', 'input,select,textarea', validate.field);
        if (!detect.attributes.autofocus) {
            form.find('input[autofocus],textarea[autofocus]').each(function () {
                if (focusable(this)) {
                    this.focus();
                    return false;
                }
            });
        }
    }

    $.fn.html5forms = function () {
        if (this.length === 0 && !$.isReady) {
            $(function () {
                $(this.selector, this.context).html5forms();
            });
            return this;
        }
        return this.each(function () {
            if ($.data(this, expando) === undefined) {
                $.data(this, expando, new Plugin(this));
            }
        });
    };
    $.extend($.fn.html5forms, {
        detect: detect,
        expando: expando,
        message: {
            checkbox: 'Please check this box if you want to proceed.',
            date: 'Please enter a date.',
            email: 'Please enter an email address.',
            max: 'Value must be less than or equal to {0}.',
            min: 'Value must be greater than or equal to {0}.',
            number: 'Please enter a number.',
            pattern: 'Please match the requested format: {0}',
            radio: 'Please select one of these options.',
            required: 'Please fill out this field.',
            select: 'Please select an item in the list.',
            step: 'Invalid value, must be a multiple of {0}.',
            url: 'Please enter a URL.'
        },
        rules: function (rule) {
            switch ($.type(rule)) {
            case 'object':
                $.extend(rules, rule);
                break;
            case 'string':
                return rules[rule] ? rules[rule].validate : $.noop;
            default:
                throw 'Parameter `rule` must be string or object literal';
            }
        },
        version: versioning.join('.') + ' (' + parseInt(build.getTime() / 1e3, 10) + ')'
    });

    if (!detect.attributes.placeholder) {
        style += '.placeholder{color:#a9a9a9!important}';
    }
    if (!detect.setCustomValidity) {
        style += 'div.validation-bubble{background:#fff;border:1px solid #c7c7c7;color:#222;display:inline-block;padding:12px 15px;position:absolute;word-wrap:break-word;z-index:2147483647;}' +
            'div.validation-bubble-arrow{color:#fff;font-size:18px;position:absolute;top:-18px;left:3px}' +
            '* html div.validation-bubble{display:inline;zoom:1}';
    }
    if (style.length) {
        $('<style>' + style + '</style>').appendTo('head');
    }

    $(document).ready(function () {
        $('form').html5forms();
    });
}(this.document, this.jQuery));