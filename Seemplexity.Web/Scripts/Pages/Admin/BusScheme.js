$(document).ready(function () {

    var $body = $("body");
    
    $body.on("click", ".js-add-floor", function () {
        BusScheme.AddFloor();
    });

    $body.on("click", ".js-update-bus-scheme", function () {
        BusScheme.UpdateBusScheme($(this));
    });

    $body.on("click", ".js-bus-scheme-item", function () {
        BusScheme.UpdateBusSchemeItem($(this));
    });

});

(function (busScheme, $) {

    busScheme.UpdateBusSchemeItem = function ($button) {
        var floorNumber = $button.data("floor-number");
        var rowNumber = $button.data("row-number");
        var columnNumber = $button.data("column-number");
        var counter = $button.data("counter");

        var $value = $("#FloorDescriptions_" + floorNumber + "_Items_" + counter + "_IsUsable");
        var val = $value.val();
        if (val === "False") {
            $value.val("True");
            $button.removeClass("btn-default");
            $button.addClass("btn-success");
        } else {
            $value.val("False");
            $button.addClass("btn-default");
            $button.removeClass("btn-success");
        }
    };

    busScheme.UpdateBusScheme = function ($button) {
        var floorNumber = $button.data("floor-number");
        var descriptionId = $button.data("description-id");
        var $contentDiv = $("#busSchemeFloor-" + floorNumber);

        var columnsCount = $button.data("column");
        if (columnsCount === undefined) {
            columnsCount = $("#columnsCount-" + floorNumber).val();
        }

        var rowsCount = $button.data("row");
        if (rowsCount === undefined) {
            rowsCount = $("#rowsCount-" + floorNumber).val();
        }

        $.ajax({
            url: "/Admin/CreateBusSchemeFloor?busDescriptionId=" + descriptionId + "&floorNumber=" + floorNumber + "&rowsCount=" + rowsCount + "&columnsCount=" + columnsCount,
            success: function (data) {
                if (data) {

                    $contentDiv.html(data);
                }
            }
        });
    };

    busScheme.AddFloor = function () {
        var $contentDiv = $("#bus-floor-content");
        var descriptionId = $("#busDescriptionId").val();
        var floorNumber = $('.js-floor-number').size() + 1;
        $.ajax({
            url: "/Admin/CreateBusSchemeFloor?busDescriptionId=" + descriptionId + "&floorNumber=" + floorNumber,
            success: function (data) {
                if (data) {
                    
                    //var newFloorNumberDiv = $(data).filter(".floor-number").html(floorNumber);
                    $contentDiv.append("<div id='busSchemeFloor-" + floorNumber + "'>" + data + "</div>");
                    //$contentDiv.children().filter('.floor-number').last().html(floorNumber);
                }
            }
        });
    };

})(window.BusScheme = window.BusScheme || {}, jQuery);