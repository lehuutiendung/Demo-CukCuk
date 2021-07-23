/**
 * Call API: Return Position Name
 */
$(document).ready(function () {
    loadPosition();
});

var listDataPosition = new Array();
function loadPosition(){
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Positions",
    }).done(function(res){
        var data = res;
        localStorage.setItem('position', JSON.stringify(data));
        $.each(data, function (index, item) { 
            listDataPosition.push(item.PositionName);
        });
    }).fail(function(res){
        console.log(res);
    })
}
