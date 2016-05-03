$(document).ready(function () {
    var a = "redischat";

    setInterval(function () {

        $.ajax({
            type: "POST",
            url: "Home/MessageListener",
            success: function (data) {
                //alert(data);
                $("#lblResult").empty();
                $.each(data,
                    function (i, val) {
                        //alert(data[i]); alert(data.data[i]);
                        $("#lblResult").append(data[i]+"</br>");
                    }
                    );


            }
        });
    }, 5000);



});