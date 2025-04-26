// Datepicker configuration
$(document).ready(function() {
    // Set datepicker defaults
    $.datepicker.setDefaults({
        monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        monthNamesShort: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        yearRange: '1900:2100',
        changeYear: true,
        
        _generateMonthYearHeader: function(inst, drawMonth, drawYear, minDate, maxDate, secondary, monthNames, monthNamesShort) {
            var originalFunction = $.datepicker._generateMonthYearHeader;
            var result = originalFunction.apply(this, arguments);
            return result;
        },
        
        beforeShow: function(input, inst) {
            // Add custom CSS for alignment and styling
            if (!$('#datepickerAlignmentFix').length) {
                $('head').append(`
                    <style id="datepickerAlignmentFix">
                        .ui-datepicker .ui-datepicker-title {
                            display: flex !important;
                            justify-content: center !important;
                            align-items: center !important;
                            position: relative !important;
                            width: 100% !important;
                            padding: 0 !important;
                            margin: 0 auto !important;
                        }
                        
                        .ui-datepicker select.ui-datepicker-month {
                            text-align-last: right !important;
                            padding-right: 5px !important;
                            width: 40% !important;
                            min-width: unset !important;
                            margin-right: 5px !important;
                            flex: 0 0 auto !important;
                        }
                        
                        select.ui-datepicker-month option {
                            direction: ltr !important;
                            text-align: left !important;
                            padding-left: 5px !important;
                        }
                        
                        @media screen and (-webkit-min-device-pixel-ratio:0) {
                            .ui-datepicker select.ui-datepicker-month {
                                text-align: right !important;
                                direction: rtl !important;
                            }
                        }
                        
                        @-moz-document url-prefix() {
                            .ui-datepicker select.ui-datepicker-month {
                                text-align: right !important;
                                direction: rtl !important;
                            }
                        }
                        
                        .year-input-container {
                            position: relative !important;
                            display: inline-flex !important;
                            width: 40% !important;
                            margin-left: 5px !important;
                            flex: 0 0 auto !important;
                        }
                        
                        .year-input-spinner {
                            right: 0 !important;
                            top: 0 !important;
                            bottom: 0 !important;
                            width: 16px !important;
                            display: flex !important;
                            flex-direction: column !important;
                        }
                        
                        .year-spinner-up,
                        .year-spinner-down {
                            flex: 1 !important;
                            background: transparent !important;
                            border: none !important;
                            padding: 0 !important;
                            cursor: pointer !important;
                            display: flex !important;
                            align-items: center !important;
                            justify-content: center !important;
                            opacity: 0.6 !important;
                        }
                        
                        .year-spinner-up:hover,
                        .year-spinner-down:hover {
                            opacity: 1 !important;
                        }
                        
                        .year-spinner-up svg,
                        .year-spinner-down svg {
                            width: 10px !important;
                            height: 5px !important;
                        }
                        
                        .year-spinner-up svg path,
                        .year-spinner-down svg path {
                            fill: #555 !important;
                        }
                        
                        .year-spinner-up:hover svg path,
                        .year-spinner-down:hover svg path {
                            fill: #0d6efd !important;
                        }
                        
                        .ui-datepicker-year-number {
                            padding-right: 18px !important;
                            text-align: left !important;
                            width: 100% !important;
                        }
                    </style>
                `);
            }
            
            inst.settings.yearRange = '1900:2100';
            
            setTimeout(function() {
                convertYearToNumberInput($(input));
            }, 10);
        },
        
        onChangeMonthYear: function(year, month, inst) {
            setTimeout(function() {
                convertYearToNumberInput($(inst.input));
            }, 10);
        }
    });
    
    // Convert year input when datepicker is opened
    $(document).on('click', '.hasDatepicker', function() {
        setTimeout(function() {
            convertYearToNumberInput($(this));
        }.bind(this), 10);
    });
    
    // Function to convert year dropdown to a number input
    function convertYearToNumberInput(clickedDatepicker) {
        var yearSelect = $('.ui-datepicker-year');
        if (yearSelect.length === 0 || yearSelect.prop('tagName') === 'INPUT') return;
        
        var currentYear = parseInt(yearSelect.val());
        var minYear = 1900;
        var maxYear = 2100;
        
        // Create input container and element
        var yearInputContainer = $('<div class="year-input-container"></div>');
        
        var yearInput = $('<input>')
            .attr({
                'type': 'number',
                'min': minYear,
                'max': maxYear,
                'value': currentYear,
                'class': 'ui-datepicker-year ui-datepicker-year-number',
                'step': '1',
                'maxlength': '4',
                'oninput': 'javascript: if(this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);'
            })
            .css({
                'width': '60px',
                'border': 'none',
                'background': 'transparent',
                'font-weight': 'bold',
                'font-size': '1.1em',
                'color': '#333',
                'padding': '0',
                'margin': '0',
                '-webkit-appearance': 'none',
                '-moz-appearance': 'textfield'
            });
        
        // Create spinner controls
        var spinnerDiv = $('<div class="year-input-spinner"></div>');
        
        var upButton = $('<button type="button" class="year-spinner-up" aria-label="Increment year">' +
            '<svg viewBox="0 0 10 5" xmlns="http://www.w3.org/2000/svg">' +
            '<path d="M0 5 L5 0 L10 5 Z"/>' +
            '</svg></button>');
            
        var downButton = $('<button type="button" class="year-spinner-down" aria-label="Decrement year">' +
            '<svg viewBox="0 0 10 5" xmlns="http://www.w3.org/2000/svg">' +
            '<path d="M0 0 L5 5 L10 0 Z"/>' +
            '</svg></button>');
            
        spinnerDiv.append(upButton).append(downButton);
        yearInputContainer.append(yearInput).append(spinnerDiv);
        
        // Replace select with custom input
        yearSelect.replaceWith(yearInputContainer);
        
        // Store reference to associated datepicker
        if (clickedDatepicker && clickedDatepicker.length) {
            yearInput.data('associated-datepicker', clickedDatepicker);
        }
        
        // Limit to 4 digits
        yearInput.on('input', function() {
            if (this.value.length > 4) {
                this.value = this.value.slice(0, 4);
            }
        });
        
        // Handle Enter key
        yearInput.on('keydown', function(e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                $(this).trigger('change');
                return false;
            }
        });
        
        // Handle year change
        yearInput.on('change', function() {
            var year = parseInt($(this).val());
            
            if (isNaN(year)) {
                $(this).val(currentYear);
                return;
            }
            
            // Apply range limits
            var correctedYear = year;
            
            if (year < minYear) {
                correctedYear = minYear;
                $(this).val(correctedYear);
            }
            
            if (year > maxYear) {
                correctedYear = maxYear;
                $(this).val(correctedYear);
            }
            
            var monthSelect = $('.ui-datepicker-month');
            var month = parseInt(monthSelect.val());
            var specificDatepicker = $(this).data('associated-datepicker');
            
            if (specificDatepicker && specificDatepicker.length) {
                try {
                    var currentDate = specificDatepicker.datepicker('getDate') || new Date();
                    var day = currentDate.getDate();
                    var newDate = new Date(correctedYear, month, day);
                    
                    // Handle invalid dates (e.g., Feb 29 in non-leap years)
                    if (newDate.getMonth() != month) {
                        newDate = new Date(correctedYear, month + 1, 0);
                    }
                    
                    specificDatepicker.datepicker('setDate', newDate);
                    
                    setTimeout(function() {
                        if ($('.ui-datepicker-year').prop('tagName') !== 'INPUT') {
                            convertYearToNumberInput(specificDatepicker);
                        }
                    }, 0);
                    
                    return false;
                } catch (e) {
                    console.error("Error updating datepicker:", e);
                }
            }
        });
        
        // Up button click handler
        upButton.on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            var input = $(this).closest('.year-input-container').find('input');
            var currentValue = parseInt(input.val());
            var maxValue = parseInt(input.attr('max'));
            
            if (currentValue < maxValue) {
                input.val(currentValue + 1).change();
            } else {
                input.val(maxValue).change();
            }
            
            setTimeout(function() {
                var yearElem = $('.ui-datepicker-year');
                if (yearElem.prop('tagName') !== 'INPUT') {
                    var clickedDatepicker = input.data('associated-datepicker');
                    convertYearToNumberInput(clickedDatepicker);
                }
            }, 10);
        });
        
        // Down button click handler
        downButton.on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            var input = $(this).closest('.year-input-container').find('input');
            var currentValue = parseInt(input.val());
            var minValue = parseInt(input.attr('min'));
            
            if (currentValue > minValue) {
                input.val(currentValue - 1).change();
            } else {
                input.val(minValue).change();
            }
            
            setTimeout(function() {
                var yearElem = $('.ui-datepicker-year');
                if (yearElem.prop('tagName') !== 'INPUT') {
                    var clickedDatepicker = input.data('associated-datepicker');
                    convertYearToNumberInput(clickedDatepicker);
                }
            }, 10);
        });
    }
    
    // Global event listener to maintain number input after UI interactions
    $(document).on('mousedown mouseup click', '.ui-datepicker', function(e) {
        setTimeout(function() {
            var yearElem = $('.ui-datepicker-year');
            if (yearElem.length && yearElem.prop('tagName') !== 'INPUT') {
                convertYearToNumberInput($('.hasDatepicker:focus'));
            }
        }, 10);
    });
});
