/**
 * Call API: Return Department Name
 */
 $(document).ready(function () {
    loadDepartment();
});

var listDataRoom = new Array();

function loadDepartment(){
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/api/Department",
        async: false,
    }).done(function(res){
        var data = res;
        $.each(data, function (index, item) { 
            listDataRoom.push(item.DepartmentName);
        });
    }).fail(function(res){
        console.log(res);
    })
}