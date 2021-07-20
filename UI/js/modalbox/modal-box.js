var inputGender = document.querySelector('.hidden-gender');
var inputPosition = document.querySelector('.hidden-position');
var inputDepartment = document.querySelector('.hidden-department');
var inputStatus = document.querySelector('.hidden-status');
var inputEmployeeCode = document.querySelector('#firstField');

// Xử lý sự kiện khi click Lưu -> Thêm nhân viên mới
$('#btn-save').on('click', function () {
    let gender, position, department, workStatus;
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

    addData(gender,position,department,workStatus);
    console.log("Added employee");
});

//Gọi API: Post thêm mới nhân viên vào database
function addData(gender,position,department,workStatus) {    
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
        contentType: "application/json"
    }).fail(function(res) {
        console.log(res);
    })
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


TODO: 
//*Bắt sự kiện blur cho các trường input bắt buộc
$('input[required]').blur(function () { 
    let value = $(this).val();
    if(value == ''){
        $(this).addClass('redError');
        $(this).attr('title', 'Trường thông tin này bắt buộc nhập!');
    }else{
        $(this).removeClass('redError');
        $(this).removeAttr('title');
    }
});

// FORMAT DATE IN MODAL
// $('.calendar').on('change', function(){
//     this.setAttribute(
//         "data-date",
//         moment(this.value, "YYYY-MM-DD")
//         .format( this.getAttribute("data-date-format") )
//     )
// }).trigger('change');


