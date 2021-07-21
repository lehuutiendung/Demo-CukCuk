// CALL API WITH FETCH 
// function employeeData(){
//     fetch('http://cukcuk.manhnv.net/v1/Employees').then(response => {
//         return response.json();
//     }).then(data => {
//         console.log(data);
//     }).catch(error => {
//         console.log(error);
//     });
// }
// employeeData();

/**
 * Gọi API Employees trả về danh sách nhân viên, render dữ liệu ra HTML
 */
 $(document).ready(function(){
    loadData();
})

function loadData(){
    // Load data
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees",
    }).done(function(res){
        var data = res;
        $.each(data, function(index, item){
            if(item.Salary == null){
                salaryFormated = "null";
            }else{
                salaryFormated = formatSalary(item.Salary.toString());
            }
            
            if(item.DateOfBirth == null){
                dateFormated = "null";
            }else{
                dateFormated = formatDate(item.DateOfBirth.toString());
            }
            var tr = $(`<tr employee-id="${item.EmployeeId}">
                    <td class="table-checkbox"><input type="checkbox"></td>
                    <td>`+ 1 +`</td>
                    <td>`+ item.EmployeeCode +`</td>
                    <td>`+ item.FullName +`</td>
                    <td>`+ item.GenderName +`</td>
                    <td class="center">`+ dateFormated +`</td>
                    <td>`+ item.PhoneNumber +`</td>
                    <td class="hidden-long-text"><div class="table-email" title=>`+ item.Email +`</div></td>
                    <td>`+ item.PositionName +`</td>
                    <td>`+ item.DepartmentName +`</td>
                    <td class="salary">`+ salaryFormated +`</td>
                    <td title=>`+ item.Address +`</td>
                </tr>`);
            // Binding data
            $('table tbody').append(tr);
            
        })
    }).fail(function(res){
        console.log(res);
    });
}


