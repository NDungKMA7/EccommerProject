$(document).ready(function () {
    $("#key").keyup(function () {
    
        var key = $("#key").val();
        if (key != "") { 
            $(".sub-search-form").attr("style", "display:block;");        
            $.get("/Search/Ajax?key=" + key, function (result) {              
                $(".sub-search-form ul").empty();          
                $(".sub-search-form ul").append(result);
            });
        } else {
            $(".sub-search-form").attr("style", "display:none;");
        }
    });
});