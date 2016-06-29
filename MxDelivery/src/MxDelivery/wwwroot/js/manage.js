$(function() {
    $("#get-token-button").on("click", function() {
        GetToken();
    });
});

function GetToken() {
    $.ajax({
        type: "GET",
        url: "api/Token",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(result) {
            $("#get-token-button").text(result.token);
            $("#get-token-button").off("click");
        }
    });
}