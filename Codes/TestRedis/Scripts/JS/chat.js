$(document).ready(function () {
    var a = "redischat";

    setInterval(function () {

        $.ajax({
            type: "POST",
            url: "Home/MessageListener",
            success: function (data) {
                $("#lblResult").empty();
                
                $("#lblResult").append(data[0]);

            }
        });
    }, 10000);

    

});