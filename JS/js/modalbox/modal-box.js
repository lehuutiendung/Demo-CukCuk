var inputGender = document.querySelector('.hidden-gender');
var inputPosition = document.querySelector('.hidden-position');
var inputDepartment = document.querySelector('.hidden-department');
var inputStatus = document.querySelector('.hidden-status');
var inputEmployeeCode = document.querySelector('#firstField');

//Biến lấy employeeId để query khi thao tác PUT Request
var employeeId = null; 
var gender, position, department, workStatus;
// formMode = 0 (Sửa), formMode = 1 (Thêm mới)
var formMode = 1;

/**
 * Xử lý sự kiện khi click Lưu -> Thêm nhân viên mới
 * 
 */ 
$('#btn-save').on('click', function () {
    if(dropDownValue.innerText == "Chọn giới tính"){
        gender = null;
    }else{
        gender = dropDownData.querySelector('.dropdown-data-item.active').getAttribute("curval");
    }
    
    if(dropDownValue2.innerText == "Chọn vị trí"){
        position = null;
    }else{
        position = dropDownData2.querySelector('.dropdown-data-item.active').getAttribute("curval");
    }

    if(dropDownValue3.innerText == "Chọn phòng ban"){
        position = null;
    }else{
        department = dropDownData3.querySelector('.dropdown-data-item.active').getAttribute("curval");
    }

    if(dropDownValue4.innerText == "Chọn tình trạng"){
        workStatus = null;
    }else{
        workStatus = dropDownData4.querySelector('.dropdown-data-item.active').getAttribute("curval");
    }
    if(formMode == 1){
        // Thêm thông tin nhân viên (formMode = 1)
        addData(gender, position, department, workStatus, hideAndRefresh);
        console.log("Đã thêm một nhân viên mới", employeeId);
    }else{
        // Sửa thông tin nhân viên (formMode = 0)
        editData(gender, position, department, workStatus, hideAndRefresh);
        console.log("Đã sửa nhân viên", employeeId);
    }
});

//Ẩn modal box và load lại dữ liệu khi lưu 
function hideAndRefresh(){
    $('.wrap-box-modal').hide();
    $('table tbody tr').hide();
    loadData();
}

/**
 * Xử lý sự kiện click chọn nhân viên để sửa thông tin
 * 
 */
 $('table').on('dblclick', 'tbody tr', function(){
    formMode = 0;
    $('.delete').show();
    $('.wrap-box-modal').show();
    $('#firstField').focus();
    employeeId = $(this).attr('employee-id');
    $.ajax({
        type: "GET",
        url: `http://cukcuk.manhnv.net/v1/Employees/${$(this).attr('employee-id')}`,
        success: function (response) {
            
        },
    }).done(function(res){
        console.log(res);
        $('#firstField').val(res.EmployeeCode);
        $('#fullName').val(res.FullName);
        $('.DOB').val(res.DateOfBirth); //FIXME: Cần chuyển định dạng để binding dữ liệu    
        //FIXME: API trả về dữ liệu của 1 nhân viên bị lỗi, lấy sai department, position
        
        // TODO: Giới tính
        if(res.Gender != null){
            document.querySelector('.dropdown-title > p').innerText = res.GenderName;
        }else{
            document.querySelector('.dropdown-title > p').innerText = "Chọn giới tính";
        }
        
        $('.identityNumber').val(res.IdentityNumber);
        // TODO: Ngày cấp
        $('.identityPlace').val(res.IdentityPlace);
        $('.email').val(res.Email);
        $('.phoneNumber').val(res.PhoneNumber);
        // TODO: Vị trí
        if(res.PositionId != null){
            document.querySelector('.dropdown-title2 > p').innerText = res.PositionName;
        }else{
            document.querySelector('.dropdown-title2 > p').innerText = "Chọn vị trí";
        }
        // TODO: Phòng ban
        if(res.DepartmentId != null){
            document.querySelector('.dropdown-title3 > p').innerText = res.DepartmentName;
        }else{
            document.querySelector('.dropdown-title3 > p').innerText = "Chọn phòng ban";
        }   

        $('.personalTaxCode').val(res.PersonalTaxCode);
        $('.wrap-salary-text').val(res.Salary);
        // TODO: Ngày gia nhập công ty
        // TODO: Tình trạng
        
        if(res.WorkStatus != null){
            // document.querySelector('.dropdown-title4 > p').innerText = res.WorkStatus;
            multipleDropDown(dropDownValue4, dropDownDataList4, dropDownData4, current4);
            console.log(res.WorkStatus)
        }else{
            document.querySelector('.dropdown-title4 > p').innerText = "Chọn tình trạng";
        }
        
    
    }).fail(function (res) {
        console.log(res);
    });
})

// Function chuyển đổi tình trạng công việc text -> id 
function convertStatusWork(workStatus){
    if(workStatus == 0){
        document.querySelector('.dropdown-title4 > p').innerText = "Nhân viên";
    }
}

//POST ---- Gọi API: Post thêm mới nhân viên vào database
function addData(gender,position,department,workStatus, callback) {  
    formMode = 1;  
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
            DateOfBirth: $('.DOB').val(),
            Gender: gender,
            IdentityNumber: $('.identityNumber').val(),
            IdentityDate: $('.identityDate').val(),
            IdentityPlace: $('.identityPlace').val(),
            Email: $('.email').val(),
            PhoneNumber: $('.phoneNumber').val(),
            PositionId: position,
            DepartmentId: department,
            PersonalTaxCode:  $('.personalTaxCode').val(),
            Salary: $('.wrap-salary-text').val(),
            JoinDate: $('.joinDate').val(),
            WorkStatus: workStatus,
            Address: 'Hà Nội',
            
        }),
        error: function(e) {
            console.log(e);
          },
        dataType: "json",
        contentType: "application/json",
    }).done(function (res) {
        if(callback) callback();
    }).fail(function(res) {
        console.log(res);
    })
}

//PUT ---- Gọi API sửa thông tin nhân viên
function editData(gender, position, department, workStatus, callback){
    formMode = 0;  
    $.ajax({
        headers: {
            'access-control-allow-origin': '*', 
            'Accept': 'application/json; charset=utf-8', 
        }, 
        type: 'PUT',
        url: `http://cukcuk.manhnv.net/v1/Employees/${employeeId}`,
        data: JSON.stringify({
            EmployeeCode: $('#firstField').val(),
            FullName: $('#fullName').val(),
            DateOfBirth: $('.DOB').val(),
            Gender: gender,
            IdentityNumber: $('.identityNumber').val(),
            IdentityDate: $('.identityDate').val(),
            IdentityPlace: $('.identityPlace').val(),
            Email: $('.email').val(),
            PhoneNumber: $('.phoneNumber').val(),
            PositionId: position,
            DepartmentId: department,
            PersonalTaxCode:  $('.personalTaxCode').val(),
            Salary: $('.wrap-salary-text').val(),
            JoinDate: $('.joinDate').val(),
            WorkStatus: workStatus,
            Address: 'Hà Nội',
        }),
        error: function(e) {
            console.log(e);
          },
        dataType: "json",
        contentType: "application/json"
    }).done(function (res) {
        if(callback) callback();
    }).fail(function(res) {
        console.log(res);
    })
}

// DELETE ---- Gọi API xóa nhân viên theo Id
function deleteData(employeeId, callback){
    $.ajax({
        type: "DELETE",
        url: `http://cukcuk.manhnv.net/v1/Employees/${employeeId}`,  
    }).done(function(res){
        if(callback) callback();
    }).fail(function(res){
        console.log(res);
    });
}


//Gọi API trả về mã nhân viên mới
function getNewCode(){
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode",
    }).done(function (res) {
        data = res;
        inputEmployeeCode.value = data;
    }).fail(function (res) {
        console.log(res);
    });
}

//Bắt sự kiện blur khi bỏ trống các trường input bắt buộc
var inputRequired = document.querySelectorAll('.input-row input[required]');
inputRequired.forEach(function(item){
    item.addEventListener('blur', function(){
        let value = item.value;
        if(value == ''){
            item.style.border = "1px solid #F65454";
            item.setAttribute('title', "Thông tin này bắt buộc nhập!");
        }else{
            item.style.border = "1px solid #bbbbbb";
            item.removeAttribute('title');
        }
    })
    item.addEventListener('focus', function(){
        item.style.border = "1px solid #019160";
    })
})

//Xử lý sự kiện khi click button Xóa trong modal-box chỉnh sửa nhân viên
$('#btn-delete').on('click', function(){
    $('.background-popup-delete').show();
})

//Sự kiện "Xóa" trong popup chỉnh sửa nhân viên (Xóa nhân viên)
$('.btn-delete-popup').on('click', function () {
    deleteData(employeeId, hideAndRefresh);
    $('.background-popup-delete').hide();
    console.log("Đã xóa nhân viên", employeeId);
})

//Sự kiện "Hủy" trong popup chỉnh sửa nhân viên
$('.btn-close-popup').on('click', function(){
    $('.background-popup-delete').hide();
})

// Xử lý sự kiện cho button Hủy modal-box
$('#btn-cancel').click(function(){
    $('.background-popup').show();
})

//Sự kiện "Tiếp tục nhập" Popup
$('.btn-continue').click(function () {
    $('.background-popup').hide();
})

//Sự kiện "Đóng" Popup
$('.btn-close').click(function () {
    $('.background-popup').hide();
    $('.wrap-box-modal').hide();
})


// FORMAT DATE IN MODAL
// $('.calendar').on('change', function(){
//     this.setAttribute(
//         "data-date",
//         moment(this.value, "YYYY-MM-DD")
//         .format( this.getAttribute("data-date-format") )
//     )
// }).trigger('change');


