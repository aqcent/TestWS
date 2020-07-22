$(document).ready(
    function () {

        var reportTypeSelector = $('.js-report-selector')[0];

        if (reportTypeSelector === null || reportTypeSelector === undefined) {
            return;
        }

        $(reportTypeSelector).on('change', () => {
            getReportForm(reportTypeSelector.value);
        });


        function getReportForm(reportType) {
            $.ajax({
                url: '/ticketsAdmin/GetReportForm',
                type: 'GET',
                dataType: 'html',
                contentType: 'application/json;charset=utf-8',
                data: {
                    type: reportType
                }
            }).done(function (resoultHTML) {
               
                $('.js-report-form-container').html(resoultHTML);
                $('#DateFrom').datetimepicker({format: 'DD.MM.YYYY HH:mm'});
                $('#DateTo').datetimepicker({format: 'DD.MM.YYYY HH:mm'});

            }).fail(function () {
                alert('Report selection request processing failed.');
            });
        }
    });