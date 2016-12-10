$(document).ready(function () {
    //$("#date").datepicker();
    //var datePicker = $('#date');

    //datePicker.datepicker({autoclose: true,language: "ru",format: "dd.mm.yyyy",startDate: datePicker.val()});

    var $countryTo = $("#countryTo");
    var $cityFrom = $("#cityFrom");
    var $cityTo = $("#cityTo");
    var $datePicker = $('#date');

    BusFilter.LoadCountriesTo($countryTo);

    $countryTo.on("change", function () {
        BusFilter.LoadCitiesFrom($countryTo.val(), $cityFrom);
    });

    $cityFrom.on("change", function () {
        BusFilter.LoadCitiesTo($countryTo.val(), $cityFrom.val(), $cityTo);
    });

    $cityTo.on("change", function () {
        BusFilter.LoadDates($countryTo.val(), $cityFrom.val(), $cityTo.val(), $datePicker);
    });

    $(".js-modal-transport-scheme").on("click", function () {
        var $that = $(this);
        BusFilter.OpenTransportScheme($that);
    });

    $(".js-modal-bus-description").on("click", function () {
        var $that = $(this);
        BusFilter.OpenBusDescription($that);
    });
});

(function (busFilter, $) {

    busFilter.Booking = function () {

        var $form = $("#TransportScheme");
        $form.submit();

        //$.ajax({
        //    type: "POST",
        //    url: $form.attr('action'),
        //    data: $form.serialize(),
        //    success: function (msg) {
        //        busFilter.$CurrentModal.modal('hide');
        //    },
        //    error: function () {
        //        alert("Ошибка при попытке бронирования");
        //    }
        //});
    };

    busFilter.UpdateBookingButton = function () {
        var selectedPlaces = new Array();
        $(".js-transport-scheme-table").find(".btn-primary").each(function (i) {
            selectedPlaces.push($(this).text());
        });
        selectedPlaces.sort();
        $("#SelectedPlacesInput").val(selectedPlaces.join(","));

        if (selectedPlaces.length > 0) {
            $(".js-booking-scheme-button").prop('disabled', false);
        } else {
            $(".js-booking-scheme-button").prop('disabled', 'disabled');
        }
    };

    busFilter.SchemeButtonClick = function ($element) {
        if ($element.hasClass('btn-success')) {
            $element.removeClass('btn-success');
            $element.addClass('btn-primary');
        }
        else if ($element.hasClass('btn-primary')) {
            $element.removeClass('btn-primary');
            $element.addClass('btn-success');
        }
    };

    busFilter.OpenBusDescription = function ($element) {
        var counter = $element.data("row-counter");
        var $dataElement = $("#row_" + counter);

        var serviceListKey = $dataElement.data("service-list-key");
        var partnerKey = $dataElement.data("partner-key");
        var date = $dataElement.data("date");

        var id = $dataElement.data("modal-div-large");
        var $div = $('#' + id);

        $.ajax({
            url: "/Bus/BusDescription?serviceListKey=" + serviceListKey + "&date=" + date + "&partnerKey=" + partnerKey,
            success: function (data) {
                if (data) {
                    $div.html(data);
                    $div.modal('show');

                    busFilter.$CurrentModal = $div;

                    //$(".js-modal-transport-scheme-button").on("click", function () {
                    //    var $that = $(this);
                    //    BusFilter.SchemeButtonClick($that);
                    //    BusFilter.UpdateBookingButton();
                    //});

                    //$(".js-booking-scheme-button").on("click", function () {
                    //    BusFilter.Booking();
                    //});
                }
            }
        });
    }

    busFilter.OpenTransportScheme = function ($element) {
        var counter = $element.data("row-counter");
        var $dataElement = $("#row_" + counter);

        var serviceListKey = $dataElement.data("service-list-key");
        var partnerKey = $dataElement.data("partner-key");
        var serviceListName = $dataElement.data("service-list-name");
        var id = $dataElement.data("modal-div");
        var date = $dataElement.data("date");
        var cityFrom = $("#cityFrom").val();
        var cityTo = $("#cityTo").val();

        var $div = $('#' + id);

        $.ajax({
            url: "/Bus/TransportScheme?serviceListKey=" + serviceListKey + "&date=" + date + "&cityFrom=" + cityFrom + "&cityTo=" + cityTo + "&serviceListName=" + serviceListName + "&partnerKey=" + partnerKey,
            success: function (data) {
                if (data) {
                    $div.html(data);
                    $div.modal('show');

                    busFilter.$CurrentModal = $div;

                    $(".js-modal-transport-scheme-button").on("click", function () {
                        var $that = $(this);
                        BusFilter.SchemeButtonClick($that);
                        BusFilter.UpdateBookingButton();
                    });

                    $(".js-booking-scheme-button").on("click", function () {
                        BusFilter.Booking();
                    });
                }
            }
        });
    }

    busFilter.LoadCountriesTo = function ($element) {

        var lang = $("#lang").val();

        $.ajax({
            dataType: "json",
            url: "/api/BusFilter?lang=" + lang + "&countryKey=" + $element.val(),
            success: function (data) {
                if (data) {
                    $element.empty();

                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        if (!item.Selected)
                            $element.append($("<option></option>").attr("value", item.Value).text(item.Text));
                        else
                            $element.append($("<option></option>").attr("value", item.Value).attr("selected", true).text(item.Text));
                    }

                    $element.change();
                }
            }
        });
    };

    busFilter.LoadCitiesFrom = function (selectedCountryTo, $element) {

        var lang = $("#lang").val();

        $.ajax({
            dataType: "json",
            url: "/api/BusFilter?lang=" + lang + "&countryKey=" + selectedCountryTo + "&cityKeyFrom=" + $element.val(),
            success: function (data) {
                if (data) {
                    $element.empty();

                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        if (!item.Selected)
                            $element.append($("<option></option>").attr("value", item.Value).text(item.Text));
                        else
                            $element.append($("<option></option>").attr("value", item.Value).attr("selected", true).text(item.Text));
                    }

                    $element.change();
                }
            }
        });
    };

    busFilter.LoadCitiesTo = function (selectedCountryTo, selectedCityFromKey, $element) {

        var lang = $("#lang").val();

        $.ajax({
            dataType: "json",
            url: "/api/BusFilter?lang=" + lang + "&countryKey=" + selectedCountryTo + "&cityKeyFrom=" + selectedCityFromKey + "&cityKeyTo=" + $element.val(),
            success: function (data) {
                if (data) {
                    $element.empty();

                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        if (!item.Selected)
                            $element.append($("<option></option>").attr("value", item.Value).text(item.Text));
                        else
                            $element.append($("<option></option>").attr("value", item.Value).attr("selected", true).text(item.Text));
                    }

                    $element.change();
                }
            }
        });
    };

    busFilter.LoadDates = function (selectedCountryTo, selectedCityFromKey, selectedCityToKey, $element) {
        var datePickerId = "#" + $element.attr("id") + "";

        $.ajax({
            dataType: "json",
            url: "/api/BusFilter?countryKey=" + selectedCountryTo + "&cityKeyFrom=" + selectedCityFromKey + "&cityKeyTo=" + selectedCityToKey + "&date=" + $element.val(),
            success: function (data) {
                if (data) {

                    $(datePickerId + ' > :first-child').datepicker('remove');
                    $(datePickerId).html('<input type="text" class="form-control">');
                    var js = "$('" + datePickerId + "').datepicker({autoclose: true, language: 'ru', format: 'dd.mm.yyyy',startDate: '" + data.MinDate + "'"
                        + ", endDate: '" + data.MaxDate + "'";
                    if (data.DisabledDates.length > 0)
                        js = js + ", datesDisabled: " + "['" + data.DisabledDates.join("','") + "']";
                    js = js + "});";
                    eval(js);

                    $(datePickerId).val(data.SelectedDate);
                }
            }
        });
    };

})(window.BusFilter = window.BusFilter || {}, jQuery);