// Write your JavaScript code.

$(function () {
    var tags = [];

    var labels = $("#labels").val().split(" ");
    for (i in labels) {
        var txt = labels[i];
        if (txt.length > 0) {
            $("#tags").prepend("<span>" + txt + "</span>");
            tags.push(txt);
        }
    }

    $("#tags input").on({
        focusout: function () {
            var txt = this.value.replace(/[^a-z0-9\+\-\.\#]/ig, "").toLowerCase();
            if (txt.length > 0) {
                if (tags.indexOf(txt) == -1) {
                    $("#tags input").before("<span>" + txt + "</span>");
                    $("#labels").val($("#labels").val() + " " + txt);
                    tags.push(txt);
                }
            }
            this.value = "";
        },
        keyup: function (ev) {
            if (/(188|13|32)/.test(ev.which)) $(this).focusout();
        }
    });
    $("#tags").on("click", "span", function () {
        var txt = $(this)[0].innerHTML.trim();
        $(this).remove();
        $("#labels").val((" " + $("#labels").val() + " ").replace(" " + txt + " ", " ").replace("  ", " ").trim());
        delete tags[tags.indexOf(txt)];
    });

});