$(document).ready(function(){
    // loadData();
    loadDataPaging();
})

function loadDataPaging(){
    // Load data
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees",
        async: false,
    }).done(function(res){
        var data = res;
        // Lưu tổng số nhân viên vào localStorage
        localStorage.setItem('totalEmployees', data.length);
        // Lưu tất cả nhân viên vào localStorage
        localStorage.setItem('allEmployees', JSON.stringify(data));
    }).done(function(res) {
       
    }).fail(function(res){
        console.log(res);
    });
}

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
        // localStorage.setItem('totalEmployees', data.length);
        console.log("Tổng số NV:", localStorage.getItem('totalEmployees'))

        // Lưu tất cả nhân viên vào localStorage
        // localStorage.setItem('allEmployees', JSON.stringify(data));
        // console.log(JSON.parse(localStorage.getItem('allEmployees')));
    
        $.each(data, function(index, item){
            if(item.Salary == null){
                salaryFormated = "";
            }else{
                salaryFormated = formatSalary(item.Salary.toString());
            }
            
            if(item.DateOfBirth == null){
                dateFormated = "";
            }else{
                dateFormated = formatDate(item.DateOfBirth.toString());
            }

            if(item.GenderName == null){
                item.GenderName = '';
            }
            if(item.PositionId == null){
                item.PositionName = '';
            }
            if(item.DepartmentId == null){
                item.DepartmentName = '';
            }
            if(item.Address == null){
                item.Address = '';
            }
            
            var tr = $(`<tr employee-id="${item.EmployeeId}" delete-id="${item.EmployeeId}" delete-employcode="${item.EmployeeCode}" class="table-checkbox--default">
                    <td class="table-checkbox"><input class="checkbox" type="checkbox" delete-id="${item.EmployeeId}"></td>
                    <td>`+ 1 +`</td>
                    <td class="employeeCode">`+ item.EmployeeCode +`</td>
                    <td class="fullName">`+ item.FullName +`</td>
                    <td class="gender"><div class="table-gender" title='${item.GenderName}'>`+ item.GenderName +`</div></td>
                    <td class="center">`+ dateFormated +`</td>
                    <td class="">`+ item.PhoneNumber +`</td>
                    <td class="hidden-long-text"><div class="table-email" title='${item.Email}'>`+ item.Email +`</div></td>
                    <td class="positionName">`+ item.PositionName +`</td>
                    <td class="departmentName">`+ item.DepartmentName +`</td>
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

// loadTotalNumberData();
/**
 * @description Cập nhật tổng số nhân viên
 * @since 23/07/2021
 * @author DUNGLHT
 */
function loadTotalNumberData(){
    // Load data
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees",
    }).done(function(res){
        $('.total__employees > p').detach();
        var totalEmployeesHTML = $(`<p style="color: #5D5C5C;">Hiển thị <span style="color: #000000; font-weight: bold;">1-10/${res.length}</span> nhân viên</p>`);
        $('.total__employees').append(totalEmployeesHTML);
    }).fail(function(res){
        console.log(res);
    });
}