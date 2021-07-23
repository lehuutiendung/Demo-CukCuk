$(document).ready(function(){
    loadData();
})

/**
 * @description Gọi API Employees trả về danh sách nhân viên, render dữ liệu ra HTML
 * @author DUNGLHT
 */
function loadData(){
    // Load data
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees",
    }).done(function(res){
        var data = res;
        // Tổng số nhân viên
        localStorage.setItem('totalEmployees', data.length);
        console.log("Tổng số NV:", localStorage.getItem('totalEmployees'))

        // Lưu tất cả nhân viên vào localStorage
        localStorage.setItem('allEmployees', JSON.stringify(data));
        // console.log(JSON.parse(localStorage.getItem('allEmployees')));
    
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
                    <td class="table-checkbox"><input class="checkbox" type="checkbox" delete-id="${item.EmployeeId}"></td>
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
    }).done(function(res) {
    
    }).fail(function(res){
        console.log(res);
    });
}

// TODO: Cập nhật tổng số nhân viên
// var totalEmployeesHTML = $(`<p style="color: #5D5C5C;">Hiển thị <span style="color: #000000; font-weight: bold;">1-10/${localStorage.getItem('totalEmployees')}</span> nhân viên</p>`);
//             // Binding data
//             $('.total__employees').append(totalEmployeesHTML);
           
