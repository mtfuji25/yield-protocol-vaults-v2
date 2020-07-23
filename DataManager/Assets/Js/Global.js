(function ($, analogue, document, unsaved, window) {
    'use strict';

    var current = null,

        enable = {

            autocomplete: function () {
                var input = $(this),
                    target = input.data('target'),
                    url = input.data('url');

                if (input.hasClass('disable_go')) {
                    input.closest('fieldset').find('button').attr('disabled', 'disabled');
                }
                input.on('blur change', function () {
                    var input = $(this),
                        target = input.data('target');

                    if (!input.val()) {
                        $(target).val('');
                    }
                }).autocomplete({
                    delay: 900,
                    minLength: 2,
                    select: function (event, ui) {
                        event.preventDefault();
                        input.val(ui.item.name).change();
                        if (target) {
                            $(target).val(ui.item.value).blur();
                        }
                        if (input.hasClass('disable_go')) {
                            input.closest('fieldset').find('button').button('option', 'disabled', false);
                        }
                    },
                    source: function (request, response) {
                        var term = input.val();

                        $.ajax({
                            data: {
                                term: term
                            },
                            dataType: 'json',
                            success: function (data) {
                                response(data);
                            },
                            url: url
                        });
                    }
                });
            }
        },

        error = function (alert, title) {
            $(window).on('keydown', function (event) {
                if (event.which === 13) {
                    event.preventDefault();

                    return false;
                }
            });
            $('<div/>').html(alert.replace('Whoops…', '')).dialog({
                buttons: {
                    'OK': function () {
                        $(this).dialog('close').dialog('destroy').remove();
                        $(window).off('keydown');
                    }
                },
                modal: true,
                title: title ? title : 'Whoops…',
                width: '360px'
            }).dialog('open');
        },

        submitRow = function (row) {
            var element = $(row),
                fields = element.find('input, select, textarea');

            $.ajax({
                cache: false,
                data: fields.serializeArray(),
                method: element.data('method') || 'post',
                url: element.data('action') || document.location.href
            }).done(function (result) {
                if (result === "OK") {
                    unsaved = false;
                } else if (result.indexOf('Whoops') === 0) {
                    error(result);
                } else {
                    $(result).insertAfter(element);
                    unsaved = false;
                }
            });
        };

    $.fx.off = true;

    // Autocomplete
    $('input.autocomplete').each(enable.autocomplete);

    // Auto submit
    $('form.auto-submit').on('change', 'input, select', function () {
        $(this).closest('form').submit();
    });

    $('tr.auto-submit').on('click, keydown', function (event) {
        var element = $(this);

        event.stopPropagation();
        if (current !== element[0]) {
            if (current !== null && unsaved) {
                submitRow(this);
            }
            current = element[0];
            element.find('input,select,textarea').one('change', function () {
                unsaved = true;
                $('[value="Save"]').removeClass('hide');
            });
            unsaved = false;
        }
    });
    $(document).on('click', function () {
        if (current !== null && unsaved) {
            submitRow(current);
        }
        current = null;
    });

    $('a.add-row-via-ajax').on('click keypress', function (event) {
        var row = $(this).closest('tr');

        event.preventDefault();
        if (event.type === 'click' || event.which === 13) {
            submitRow(row);
        }
    });

    // Buttons
    $('main').find('a.button, form button, input.button, label.button, label.file').button().on('click', function () {
        var button = $(this);

        if (button.attr('type') === 'submit') {
            $(this).find('[data-fa-i2svg]').removeClass('fa-save fa-trash-alt fa-plus').addClass('fa-spinner fa-spin');
        }
    });

    // Column Chooser
    $('#column-chooser button[type="reset"]').on('click', function () {
        $('#column-chooser').hide();
    });
    $('#column-chooser input[type="checkbox"]').on('click blur', function () {
        var element = $(this),
            index = element.data('index');

        if (element.is(':checked')) {
            $('table.list td:nth-child(' + index + ')').show();
            $('table.list th:nth-child(' + index + ')').show();
        } else {
            $('table.list td:nth-child(' + index + ')').hide();
            $('table.list th:nth-child(' + index + ')').hide();
        }
        element.off('blur');
    }).blur();
    $('a.show-column-chooser').on('click', function (event) {
        event.preventDefault();
        $('#column-chooser').show();
    });
    $.fn.datajax.callbacks({
        restoreIcon: function () {
            $('#column-chooser [data-fa-i2svg]').each(function () {
                var icon = $(this);

                if (icon.hasClass('fa-spin')) {
                    icon.removeClass('fa-spinner fa-spin').addClass('fa-save');
                }
            });
        }
    });

    // Check all/none
    $('input.check-all').on('click', function () {
        var checked = $(this).is(':checked');

        if (checked) {
            $(this).closest('div.checkboxes').find(':checkbox').prop('checked', true);
        } else {
            $(this).closest('div.checkboxes').find(':checkbox').prop('checked', false);
        }
    });

    // Clickable table rows
    $('table.list').on('click', 'tr[data-href]', function () {
        document.location = $(this).addClass('clicked').attr('data-href');
    });

    // Date Picker
    $(document).on('click', 'input.date', function () {
        var input = $(this);

        if (!input.hasClass('hasDatepicker')) {
            input.datepicker({
                onSelect: function (date) {
                    var element = $(this),
                        parsed,
                        target = element.attr('data-target');

                    if (target) {
                        $(target).val(date);
                    }
                    element.change();
                    if (element.hasClass('setDate')) {
                        parsed = $.datepicker.parseDate('mm/dd/yy', date);
                        parsed.setFullYear(parsed.getFullYear() + 1);
                        $('#' + element.attr('data-set-date')).datepicker('setDate', parsed);
                    }
                },
                showAnim: ''
            }).datepicker("show");
        }
    });

    // Date Week
    $('input.week').each(function () {
        $(this).datepicker({
            beforeShowDay: function (date) {
                return [date.getDay() === 1, ''];
            },
            firstDay: 1,
            onSelect: function (date) {
                var element = $(this),
                    parsed,
                    target = element.attr('data-target');

                if (target) {
                    $(target).val(date);
                }
                element.change();
                if (element.hasClass('setDate')) {
                    parsed = $.datepicker.parseDate('mm/dd/yy', date);
                    parsed.setFullYear(parsed.getFullYear() + 1);
                    $('#' + element.attr('data-set-date')).datepicker('setDate', parsed);
                }
            }
        }).each(function () {
            $(this).after('<label class="fa fa-calendar fa-fw" for="' + this.id + '"></label>');
        });
    });

    // Date range
    $('#range, select.range').on('blur change', function () {
        if ($(this).val() === 'custom') {
            $('span.date_range').show();
        } else {
            $('span.date_range').hide();
        }
    }).blur();

    // Delete/Remove
    $('[value="Delete"], [value="Remove"], a.delete, a.remove').on('click', function (event) {
        if (!window.confirm('Are you sure you want to delete this item?\nOnce complete, this operation cannot be undone.')) {
            event.preventDefault();
        }
        $(this).find('[required]').each(function () {
            $(this).removeAttr('required');
        });
    });

    $('a.delete-row-via-ajax').on('click keypress', function (event) {
        var anchor = $(this),
            element = anchor.closest('tr'),
            fields = element.find('input, select, textarea');

        event.preventDefault();
        if (event.type === 'click' || event.which === 13) {
            $.ajax({
                cache: false,
                data: fields.serializeArray(),
                method: element.data('method') || 'post',
                url: anchor.attr('href') || element.data('action') || document.location.href
            }).done(function (result) {
                if (result.indexOf('Whoops') === 0) {
                    error(result);
                } else {
                    element.remove();
                }
            });
        }
    });

    // Dialog
    $(document).on('click', 'a.dialog, tr.dialog', function (event) {
        var cancel = function () {
                $(this).dialog('close').dialog('destroy').remove();
            },
            div = $('<div/>'),
            element = $(this),
            process = function (action) {
                return function () {
                    var form = $(this).find('form:first'),
                        target = $(element.data('target') || element);

                    if (action === 'Delete' && !window.confirm('Are you sure you want to delete this item?\nOnce complete, this operation cannot be undone.')) {
                        return;
                    }
                    form.find('input[name="Action"]').val(action);
                    $.ajax({
                        data: form.serialize(),
                        type: form.attr('method') || 'POST',
                        url: form.attr('action')
                    }).done(function (data) {
                        if (data.indexOf('Whoops') === 0) {
                            error(data);
                        } else {
                            switch (action) {
                            case 'Add':
                                if (element.data('after')) {
                                    $(element.data('after')).after(data);
                                } else if (element.data('append')) {
                                    $(element.data('append')).append(data);
                                } else if (element.data('before')) {
                                    $(element.data('before')).before(data);
                                } else if (element.data('prepend')) {
                                    $(element.data('prepend')).prepend(data);
                                } else {
                                    document.location.reload();
                                }
                                break;
                            case 'Delete':
                                target.remove();
                                break;
                            case 'Update':
                                target.replaceWith(data);
                                break;
                            default:
                                error('No action specified for returned data!', 'Missing Action');
                            }
                            div.dialog('close').dialog('destroy').remove();
                        }
                    });
                };
            },
            url = element.attr('href') || element.data('action');

        event.preventDefault();
        $.ajax({
            cache: false,
            url: url
        }).done(function (response) {
            if (response.indexOf('Whoops') === 0) {
                error(response);
            } else {
                div.html(response).dialog({
                    /* eslint-disable sort-keys */
                    buttons: (/\/add/i).test(url) ? {
                        'Add': process('Add'),
                        'Cancel': cancel
                    } : {
                        'Update': process('Update'),
                        'Delete': process('Delete'),
                        'Cancel': cancel
                    },
                    /* eslint-enable sort-keys */
                    draggable: false,
                    modal: true,
                    open: function () {
                        $(this).find('input.autocomplete').each(enable.autocomplete);
                    },
                    resizable: false,
                    title: element.attr('title') || element.data('title') || element.text(),
                    width: element.data('width')
                });
            }
        });
    });

    // Display Name
    $('#DisplayName').each(function () {
        var company = $('#Company'),
            display = $(this),
            first = $('#FirstName'),
            last = $('#LastName');

        $('#FirstName, #LastName, #Company').on('change', function () {
            var name;

            if (last.val() && first.val()) {
                name = last.val() + ', ' + first.val();
            } else if (last.val()) {
                name = last.val();
            } else if (first.val()) {
                name = first.val();
            }
            display.empty();
            display.append($('<option></option>').attr('value', company.val()).text(company.val()));
            display.append($('<option></option>').attr('value', name).text(name));
        });
    });

    // Expandable
    $('.expandable').on('click', function () {
        var span = $(this).find('span'),
            target = $(this).data('target');

        if (span.text() === '+') {
            $(target).show();
            span.text('-');
        } else {
            $(target).hide();
            span.text('+');
        }
    });

    // Masked Inputs
    $('input[type="tel"]').mask('999-999-9999? x99999');
    $('#SSN').mask('999-99-9999');

    // Redirect Buttons
    $('[name="redirect"]').on('click', function (event) {
        event.preventDefault();
        document.location = $(this).data('href');
    });

    // Tabs
    $('ul.tabs a').on('click', function (event) {
        var element = $(this),
            target = element.attr('href');

        event.preventDefault();
        $('tbody.tab').hide();
        $('ul.tabs li').removeClass('active');
        element.closest('li').addClass('active');
        $(target).show();
    });

    // Time
    $(document).on('click', 'input.time', function () {
        var input = $(this);

        if (!input.hasClass('ui-timepicker-input')) {
            input.timepicker({
                step: 1,
                timeFormat: 'h:i A'
            }).timepicker('show');
        }
    });

    // Time Clock
    $('ol.keypad li').on('click', function () {
        var element = $(this),
            key = element.text(),
            target = $('#EmployeeNumber'),
            value = target.val();

        if (element.hasClass('clear')) {
            target.val('');
        } else if (element.hasClass('back')) {
            target.val(value.slice(0, -1));
        } else if (value.length < 4) {
            target.val(value + key);
        }

        if (target.val().length === 4) {
            element.closest('form').submit();
        }
    });

    // Toggles
    $('.toggle').on('click', function (e) {
        var target = $(this).attr('href');

        e.preventDefault();
        $(target).toggle();
    });

    // Track Changes
    $('form.track').find('input,select,textarea').one('change', function () {
        unsaved = true;
        $('[value="Save"]').removeClass('hide');
    });
    $('[value="Save"]').on('click', function () {
        unsaved = false;
    });
    window.onbeforeunload = function () {
        $(window).on('keydown', function (event) {
            if (event.which === 13) {
                event.preventDefault();

                return false;
            }
        });
        if (unsaved) {
            return 'You have unsaved changes on this page. Do you want to leave this page and discard your changes or stay on this page?';
        }
    };

}(this.jQuery, this.analogue, this.document, false, this));
