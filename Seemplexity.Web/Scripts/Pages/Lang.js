$(document).ready(function () {

    $('.js-lang').on("click", function () {
        var lang = this.text.trim();

        $.ajax({
            url: "/api/AccountApi?lang=" + lang,
            success: function () {
                location.reload();
            }
        });

    });

});