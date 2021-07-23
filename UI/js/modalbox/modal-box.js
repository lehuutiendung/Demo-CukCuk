var inputGender = document.querySelector('.hidden-gender');
var inputPosition = document.querySelector('.hidden-position');
var inputDepartment = document.querySelector('.hidden-department');
var inputStatus = document.querySelector('.hidden-status');
var inputEmployeeCode = document.querySelector('#firstField');

/**
 * @description Biến lấy employeeId để query khi thao tác PUT Request
 */ 
var employeeId = null; 

var gender = null, position = null, department = null, workStatus = null;

// formMode = 0 (Sửa), formMode = 1 (Thêm mới)
var formMode = 1;

/**
 * @description Xử lý sự kiện khi click Lưu -> Thêm nhân viên mới
 * @author DUNGLHT
 */ 
$('#btn-save').on('click', function () {
    validateSave();
    gender = convertGender(dropDownValue);
    position = convertPosition(dropDownValue2);
    department = convertDepartment(dropDownValue3);
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
        console.log(gender)
        editData(gender, position, department, workStatus, hideAndRefresh);
        multipleDropDown(dropDownValue, dropDownDataList, dropDownData, current);
        multipleDropDown(dropDownValue2, dropDownDataList2, dropDownData2, current2);
        multipleDropDown(dropDownValue3, dropDownDataList3, dropDownData3, current3);
        multipleDropDown(dropDownValue4, dropDownDataList4, dropDownData4, current4);
        console.log("Đã sửa nhân viên", employeeId);
    }
});

/**
 * @description Ẩn modal box và load lại dữ liệu khi lưu 
 * @author DUNGLHT
 */
function hideAndRefresh(){
    $('.wrap-box-modal').hide();
    $('table tbody tr').hide();
    loadData();
}

/**
 * @description Xử lý sự kiện click chọn nhân viên để sửa thông tin
 * @author DUNGLHT
 */
 $('table').on('dblclick', 'tbody tr', function(){
    $('.input-row > input').val('');
    // $('.wrap-salary-text').val('');
    // $('.dropdown-title.gender > p').text('');

    // allEmployees = JSON.parse(localStorage.getItem('allEmployees'));
    // let result = allEmployees.find(obj => {
    //     return obj.EmployeeId == 'b013c013-eaf1-11eb-94eb-42010a8c0002';
    // })
    // console.log(result);

    formMode = 0;
    $('.delete').show();
    $('.wrap-box-modal').show();
    $('#firstField').focus();
    
    employeeId = $(this).attr('employee-id');
    
    $.ajax({
        type: "GET",
        url: `http://cukcuk.manhnv.net/v1/Employees/${$(this).attr('employee-id')}`,
        
    }).done(function(res){
        console.log("res:*********************** ",res);
        
        $('#firstField').val(res.EmployeeCode);
        $('#fullName').val(res.FullName);
        $('.DOB').val(formatDateJSONtoInput(res.DateOfBirth));     
        $('.calendar-format').val(formatDate(res.DateOfBirth));
        // Gán dữ liệu Giới tính
        if(res.Gender != null && res.Gender < 3){
            $('#gender > p').text( res.GenderName);
        }else{
            $('#gender > p').text("Chọn giới tính");
        }
        $('.identityNumber').val(res.IdentityNumber);
        $('.identityDate').val(formatDateJSONtoInput(res.IdentityDate));     
        $('.calendar-identity').val(formatDate(res.IdentityDate));
        $('.identityPlace').val(res.IdentityPlace);
        $('.email').val(res.Email);
        $('.phoneNumber').val(res.PhoneNumber);
        // Gán dữ liệu Vị trí
        if(res.PositionId != null){
            var positionName = getPositionName(res.PositionId);
            document.querySelector('.dropdown-title2 > p').innerText = positionName;
        }else{
            document.querySelector('.dropdown-title2 > p').innerText = "Chọn vị trí";
        }
        // Gán dữ liệu Phòng ban
        if(res.DepartmentId != null){
            var departmentName = getDepartmentName(res.DepartmentId);
            document.querySelector('.dropdown-title3 > p').innerText = departmentName;
        }else{
            document.querySelector('.dropdown-title3 > p').innerText = "Chọn phòng ban";
        }   

        $('.personalTaxCode').val(res.PersonalTaxCode);
        $('.wrap-salary-text').val(res.Salary);

        $('.joinDate').val(formatDateJSONtoInput(res.JoinDate));     
        $('.calendar-joinDate').val(formatDate(res.JoinDate));
        if(res.WorkStatus != null){
            // document.querySelector('.dropdown-title4 > p').innerText = res.WorkStatus;
            multipleDropDown(dropDownValue4, dropDownDataList4, dropDownData4, current4);
        }else{
            document.querySelector('.dropdown-title4 > p').innerText = "Chọn tình trạng";
        }
        
    
    }).fail(function (res) {
        console.log(res);
    });
})

/**
 * @description Function chuyển đổi giới tính text -> id
 * @param {*} dropDownValue: Giá trị text trong dropdown Chọn giới tính
 * @returns Gender Id
 * @author DUNGLHT
 */
function convertGender(dropDownValue){
    if(dropDownValue.innerText == "Chọn giới tính"){
        return null;
    }
    else if(dropDownValue.innerText == "Nữ"){
        return 0;
    }else if(dropDownValue.innerText == "Nam"){
        return 1;
    }
    return 2;
}

/**
 * @description Function chuyển đổi vị trí text -> id
 * @param {*} dropDownValue Giá trị text trong dropdown Chọn vị trí
 * @returns PositionId
 * @author DUNGLHT
 */
function convertPosition(dropDownValue){
    var rel = "";
    var positionData = localStorage.getItem('position');
    JSON.parse(positionData).forEach(function(item){
        if(dropDownValue.innerText == item.PositionName){
            rel = item.PositionId;
            
        }
    })
    return rel;
}

/**
 * @description Function chuyển đổi phòng ban text -> id
 * @param {*} dropDownValue Giá trị text trong dropdown Chọn phòng ban
 * @returns PositionId
 * @author DUNGLHT
 */
function convertDepartment(dropDownValue){
    var rel = "";
    var departmentData = localStorage.getItem('department');
    JSON.parse(departmentData).forEach(function(item){
        if(dropDownValue.innerText == item.DepartmentName){
            rel = item.DepartmentId;

        }
    })
    console.log(rel)
    return rel;
}


/**
 * @description POST ---- Gọi API: Post thêm mới nhân viên vào database
 * @param {*} gender genderId
 * @param {*} position positionId 
 * @param {*} department deparmentId
 * @param {*} workStatus workStatusId
 * @param {*} callback 
 * @author DUNGLHT
 */
function addData(gender,position,department,workStatus, callback) {  
    formMode = 1;  
    console.log($('.DOB').val());
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
            Salary: convertSalary(),
            JoinDate: $('.joinDate').val(),
            WorkStatus: workStatus,
            Address: 'Hà Nội',
            
        }),
        dataType: "json",
        contentType: "application/json",
    }).done(function (res) {
        if(callback) callback();
    }).fail(function(res) {
        console.log(res);
    })
}

/**
 * @description PUT ---- Gọi API sửa thông tin nhân viên
 * @param {*} gender genderId
 * @param {*} position positionId 
 * @param {*} department deparmentId
 * @param {*} workStatus workStatusId
 * @param {*} callback 
 * @author DUNGLHT
 */
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
            Salary: convertSalary(),
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

/**
 * @description DELETE ---- Gọi API xóa nhân viên theo Id
 * @param {*} employeeId 
 * @param {*} callback 
 * @author DUNGLHT
 */
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

/**
 * @description Gọi API trả về mã nhân viên mới
 * @author DUNGLHT
 */
function getNewCode(){
    $.ajax({
        type: "GET",
        url: "http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode",
    }).done(function (res) {
        data = res;
        inputEmployeeCode.value = data;
    }).fail(function (res) {
        console.log(res);
        console.log("Trả về dữ liệu API lỗi! Client sẽ tự sinh mã NV: ");
        inputEmployeeCode.value = "NV"+ Math.floor(Math.random()*100) + 1;
        console.log("Mã NV:", inputEmployeeCode.value = "NV"+ Math.floor(Math.random()*999) + 1);
    });
}

/**
 * @description Function lấy DepartmentId trả về DepartmentName dùng localStorage
 * @param {*} DepartmentId 
 * @returns 
 * @author DUNGLHT
 */
function getDepartmentName(DepartmentId){
    var rel = "";
    var saveData = localStorage.getItem('department');
    JSON.parse(saveData).forEach(function(item){
        if(DepartmentId === item.DepartmentId){
            rel = item.DepartmentName;
            
        }
    })
    return rel;
}


/**
 * @description Function lấy PositionId trả về PositionName dùng localStorage
 * @param {*} PositionId 
 * @returns 
 * @author DUNGLHT
 */
function getPositionName(PositionId){
    var rel = "";
    var saveData = localStorage.getItem('position');
    JSON.parse(saveData).forEach(function(item){
        if(PositionId === item.PositionId){
            rel = item.PositionName;
            
        }
    })
    return rel;
}

/**
 * @description Bắt sự kiện blur khi bỏ trống các trường input bắt buộc
 * @since 22/07/2021
 * @author DUNGLHT
 */
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

/**
 * @description Xử lý sự kiện khi click button Xóa trong modal-box chỉnh sửa nhân viên
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('#btn-delete').on('click', function(){
    $('.background-popup-delete').show();
})

/**
 * @description Sự kiện "Xóa" trong popup chỉnh sửa nhân viên (Xóa nhân viên)
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('.btn-delete-popup').on('click', function () {
    deleteData(employeeId, hideAndRefresh);
    $('.background-popup-delete').hide();
    console.log("Đã xóa nhân viên", employeeId);
})

/**
 * @description Sự kiện "Hủy" trong popup chỉnh sửa nhân viên
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('.btn-close-popup').on('click', function(){
    $('.background-popup-delete').hide();
})

/**
 * @description Xử lý sự kiện cho button Hủy modal-box
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('#btn-cancel').click(function(){
    $('.background-popup').show();
})

/**
 * @description Xử lý sự kiện cho nút X modal-box
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('#exit-modal').click(function(){
    $('.background-popup').show();
})


/**
 * @description Sự kiện "Tiếp tục nhập" Popup
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('.btn-continue').click(function () {
    $('.background-popup').hide();
})

/**
 * @description Sự kiện "Đóng" Popup
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('.btn-close').click(function () {
    $('.background-popup').hide();
    $('.wrap-box-modal').hide();
})

/**
 * @description Xử lý sự kiện cho nút X Popup
 * @since 21/07/2021
 * @author DUNGLHT
 */
$('.exit-popup').click(function () {
    $('.background-popup').hide();
    $('.background-popup-delete').hide();
})

/**
 * @description Custom cho lịch; custom border cho lịch, dropdown, lương
 * @author DUNGLHT
 */
// Custom lịch ngày sinh
$('.DOB').change(function(){
    let date = $('.DOB').val();
    $('.calendar-format').val(formatDate(date));
})
// Custom lịch ngày cấp căn cước
$('.identityDate').change(function(){
    let date = $('.identityDate').val();
    $('.calendar-identity').val(formatDate(date));
})
// Custom lịch ngày gia nhập công ty
$('.joinDate').change(function(){
    let date = $('.joinDate').val();
    $('.calendar-joinDate').val(formatDate(date));
})


/**
 * @description Format date khi doubleclick, gắn data lên input date
 * @param {*} date 
 * @returns 
 * @since 22/07/2021
 * @author DUNGLHT
 */
function formatDateJSONtoInput(date){
    //2021-07-15T00:00:00
    // var curDate = date.toString();
    try {
        var rel = '';
    for(i = 0; i < 10; i++){
        rel += date[i];
    }
    return rel;
    } catch (error) {
        console.log("Không có dữ liệu ngày để hiển thị!")
    }
}

