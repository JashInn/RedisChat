$(document).ready(function () {
    var a = "redischat";

    $("#btnSubmit").click(function () {
        $.ajax({
            url: '/Listener/Index_sub',
            type: 'POST',
            data: 'message=' + a,
            success: function (data) {
                setInterval(function () {

                    $.ajax({
                        type: "POST",
                        url: "Listener/Index_GetMessages",
                        success: function (data) {
                            //$("#lblResult").empty();
                            $("#lblResult").append(data[0]);

                        }
                    });
                }, 3000);
            },
      
        });
    });
    $("#btnUnsub").click(function () {
        $.ajax({
            url: '/Listener/Index_Unsub',
            type: 'POST',
            data: 'message=' + a,
            success: function (data) {

            },
        });
    });

    

});