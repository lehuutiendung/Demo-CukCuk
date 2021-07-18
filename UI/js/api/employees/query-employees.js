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


// CALL API WITH AJAX
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
            // var tr = $(`<tr>
            //             <th>`+ item.EmployeeCode +`</th>
            //             <th>`+ item.FullName +`</th>
            //             <th>`+ item.GenderName +`</th>
            //             <th class="center">`+ item.DateOfBirth +`</th>
            //             <th>`+ item.PhoneNumber +`</th>
            //             <th class="hidden-long-text">`+ item.Email +`</th>
            //             <th>`+ item.QualificationName +`</th>
            //             <th>`+ item.PositionName +`</th>
            //             <th class="salary">`+ item.Salary +`</th>
            //             <th>`+ item.Address +`</th>
            //         </tr>`);
            
                    var tr = $(`<tr>
                    <td class="table-checkbox"><input type="checkbox"></td>
                    <td>`+ 1 +`</td>
                    <td>`+ item.EmployeeCode +`</td>
                    <td>`+ item.FullName +`</td>
                    <td>`+ item.GenderName +`</td>
                    <td class="center">`+ item.DateOfBirth +`</td>
                    <td>`+ item.PhoneNumber +`</td>
                    <td class="hidden-long-text"><div class="table-email">`+ item.Email +`</div></td>
                    <td>`+ item.QualificationName +`</td>
                    <td>`+ item.PositionName +`</td>
                    <td class="salary">`+ item.Salary +`</td>
                    <td>`+ item.Address +`</td>
                </tr>`);

            // var tr = $(`<tr>
            //             <td>NV-532408</td>
            //             <td>Hồ Văn Long</td>
            //             <td>Nam</td>
            //             <td class="center">01/02/2000</td>
            //             <td>0332623437</td>
            //             <td>holong@gmail.com</td>
            //             <td>Thu ngân</td>
            //             <td>Phòng Công nghệ</td>
            //             <td class="salary">10.000.000</td>
            //             <td>Đang thử việc</td>
            //         </tr>`);

            
            $('table tbody').append(tr);
        })
        
    }).fail(function(res){

    });
    // Binding data
}