/**
 * @description API xóa nhân viên theo id nhân viên
 * @since 23/07/2021
 * @param {*} employeeId
 * @author DUNGLHT 
 */
function deleteEmployees(employeeId){
    $.ajax({
        type: "DELETE",
        url: `http://cukcuk.manhnv.net/v1/Employees/${employeeId}`,  
        async: false,
    }).done(function(res){
        
    }).fail(function(res){
        console.log(res);
    });
}