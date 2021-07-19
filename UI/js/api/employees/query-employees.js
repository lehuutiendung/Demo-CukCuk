/**
 * Get all employees
 */

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


// CALL API GET ALL EMPLOYEES
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
            var tr = $(`<tr>
                    <td class="table-checkbox"><input type="checkbox"></td>
                    <td>`+ 1 +`</td>
                    <td>`+ item.EmployeeCode +`</td>
                    <td>`+ item.FullName +`</td>
                    <td>`+ item.GenderName +`</td>
                    <td class="center">`+ dateFormated +`</td>
                    <td>`+ item.PhoneNumber +`</td>
                    <td class="hidden-long-text"><div class="table-email">`+ item.Email +`</div></td>
                    <td>`+ item.QualificationName +`</td>
                    <td>`+ item.PositionName +`</td>
                    <td class="salary">`+ salaryFormated +`</td>
                    <td>`+ item.Address +`</td>
                </tr>`);
            
            // Binding data
            $('table tbody').append(tr);
        })
    }).fail(function(res){

    });
}

//CALL API ADD A EMPLOYEE TO DATABASE
function addData() {    
    $.ajax({
        headers: {
            'access-control-allow-origin': '*', 
            'Accept': 'application/json; charset=utf-8', 
        }, 
        type: 'POST',
        url: 'http://cukcuk.manhnv.net/v1/Employees',
        data: JSON.stringify({
            EmployeeCode: $('#firstField').val(),
            FullName: $('#fullName').val(),
            GenderName: 0,
            DateOfBirth: '2021-07-19T02:55:09.681Z',
            PhoneNumber: '0332623437',
            Email: 'tiendungbigcat@gmail.com',
            QualificationName: null,
            PositionName: null,
            Salary: 1,
            Address: 'TK6, Lê Đình Châu',
        }),
        error: function(e) {
            console.log(e);
          },
        dataType: "json",
        contentType: "application/json"
    });
}