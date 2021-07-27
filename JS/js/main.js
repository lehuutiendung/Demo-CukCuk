/**
 * @description Hiện modal-box và tự động focus, lấy mã nhân viên mới
 * @author DUNGLHT
 */

/**
 * @description Biến formMode xác định thêm mới hay chỉnh sửa (0: Sửa, 1: Thêm mới)
 */
var formMode = 1;
$('.wrap-box-modal').hide();
$('#myDiv').on('click', function(){
    formMode = 1;
    $('.delete').hide();
    // $('.wrap-box-modal').show();
    $('.wrap-box-modal').css('display', 'flex');
    $('.box-info').animate({
        scrollTop: $(".box-info").offset().top - 30
    }, 1000);
    //Clear data khi click vào lại button thêm mới nhân viên
    $('.warning-email').css('display', 'none');
    $('.input-row > input').val('');
    $('.wrap-salary-text').val('');
    document.querySelector('.dropdown-title > p').innerText = "Chọn giới tính";
    document.querySelector('.dropdown-title2 > p').innerText = "Chọn vị trí";
    document.querySelector('.dropdown-title3 > p').innerText = "Chọn phòng ban";
    document.querySelector('.dropdown-title4 > p').innerText = "Chọn tình trạng";
    $('input[required]').css('border', '1px solid #bbbbbb');
    $('#firstField').focus();
    getNewCode();
})


/**
 * @description Search Input: Tìm kiếm theo Mã nhân viên, Tên nhân viên, Số điện thoại
 * @author DUNGLHT
 */
$(document).ready(function() {
    $('.input-search').on('keyup', function(event) {
       event.preventDefault();
       var tukhoa = $(this).val().toLowerCase();
       $('table tbody tr').filter(function() {
        $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa)>-1);
        //   $(this).toggle($(this).find('.employeeCode').text().toLowerCase().indexOf(tukhoa)>-1);
        //   $(this).toggle($(this).find('.fullName').text().toLowerCase().indexOf(tukhoa)>-1);
        //   $(this).toggle($(this).find('.phoneNumber').text().toLowerCase().indexOf(tukhoa)>-1);
       });
    });
 });

/**
 * @description Bộ lọc dữ liệu table với combobox
 * @since 24/07/2021
 * @author DUNGLHT
 */
function filterCombobox(){
    var keyDepartment = $('.combobox[data-id=1]').val().toLowerCase();
    var keyPosition = $('.combobox[data-id=2]').val().toLowerCase();

    if(keyDepartment != '' &&  keyPosition == ''){
        $('table tbody tr').filter(function() {
            $(this).toggle($(this).find('.departmentName').text().toLowerCase().indexOf(keyDepartment)>-1);
        });
    }
    else if(keyPosition != '' &&  keyDepartment == '' ){
        $('table tbody tr').filter(function() {
            $(this).toggle($(this).find('.positionName').text().toLowerCase().indexOf(keyPosition)>-1);
        });
    }else{
        $('table tbody tr').filter(function() {
            //TODO: Xử lý filter 2 combobx đồng thời
        });
    }
}

/**
 * @deprecated Click button refresh
 * @author DUNGLHT
 */
$(document).ready(function(){
    $('.refresh-button').on('click', function(){
        if(pageId == 1 || pageId == 2 || pageId == 3){
            // Refresh các trường input, combobox
            $('.input-search').val('');
            $('.combobox[data-id=1]').val('');
            $('.combobox[data-id=2]').val('');
            loadDataPaging();
            allEmployees = JSON.parse(localStorage.getItem('allEmployees'));
            totalItems = localStorage.getItem('totalEmployees');
            currentPage = 1;
            $('table tbody tr').hide();
            var objPage = paginate(totalItems, currentPage, pageSize, maxPages);
            activePaginate(objPage);
            $('.button__dot').remove();
            changeNumberPage(objPage);
            $('.button__dot').first().addClass('focus');
            $('.total__employees--change').remove();
            changeNumberEmployeeFooter(objPage);
        }else{
            // Refresh các trường input, combobox
            $('.input-search').val('');
            $('.combobox[data-id=1]').val('');
            $('.combobox[data-id=2]').val('');
            $('table tbody tr').detach();
            loadData();
        }
    })
})

/**
 * @description Xử lý chọn nhiều nhân viên
 * @author DUNGLHT
 * @since 23/07/2021
 */ 
var queueDelete = [];
var queueEmployeeCode = [];
$('table').on('click', 'tbody tr', function(){
    $(this).find('input[type=checkbox]').prop("checked", !$(this).find('input[type=checkbox]').prop("checked"));
    $(this).toggleClass('table-checkbox--active');
    $(this).toggleClass('table-checkbox--default');
    $('.container__title__button--delete').css('display', 'flex');
    let employeeId = $(this).attr('delete-id');
    let employeeCodeDelete = $(this).attr('delete-employcode');
    let indexItem = queueDelete.indexOf(employeeId);
    if(indexItem == -1){
        queueDelete.push(employeeId);
        queueEmployeeCode.push(employeeCodeDelete);
    }else{
        //Tại vị trí indexItem, thực hiện remove 1 phần tử
        queueDelete.splice(indexItem, 1);
        queueEmployeeCode.splice(indexItem,1);
        if(queueDelete.length == 0){
            $('.container__title__button--delete').css('display', 'none');
        }
    }
})

/**
 * @description Xử lý click "Xóa nhân viên"
 * @author DUNGLHT
 * @since 23/07/2021
 */
$('.container__title__button--delete').on('click', function () {
    var sizeQueue = queueDelete.length;
    queueDelete.forEach(function(item){
        console.log(item);
        deleteEmployees(item);
        sizeQueue--;
        console.log("Đã xóa nhân viên:" , item);
    })
    if(sizeQueue == 0){
        refreshDeleteMulti();
    }
}) 


/**
 * @description Tải lại dữ liệu bảng sau khi xóa nhiều nhân viên
 * @author DUNGLHT
 * @since 23/7/2021
 */
function refreshDeleteMulti(){
    if(pageId == 1 || pageId == 2 || pageId == 3){
        loadDataPaging();
        allEmployees = JSON.parse(localStorage.getItem('allEmployees'));
        totalItems = localStorage.getItem('totalEmployees');
        currentPage = 1;
        $('table tbody tr').detach();
        var objPage = paginate(totalItems, currentPage, pageSize, maxPages);
        activePaginate(objPage);
        $('.button__dot').remove();
        changeNumberPage(objPage);
        $('.button__dot').first().addClass('focus');
        $('.container__title__button--delete').css('display', 'none');
        queueDelete = [];
        $('.total__employees--change').remove();
        changeNumberEmployeeFooter(objPage);
    }else{
        $('table tbody tr').detach();
        loadData()
        $('.container__title__button--delete').css('display', 'none');
        queueDelete = [];
        loadTotalNumberData();
    }
}

/**
 * @description Css hover cho button Thêm nhân viên
 * @since 23/7/2021
 * @author DUNGLHT
 */
$('#myDiv').hover(function () {
        // over
        $('#myDiv').css('background-color', '#2FBE8E');
    }, function () {
        // out
        $('#myDiv').css('background-color', '#019160');
    }
);


/*************************************************************************************************************************/
/*                                                     EVENT FOOTER                                                      */
/*************************************************************************************************************************/

//Css cho button đang active
$('.button__dot.focus').hover(function () {
        // over
        $('.button__dot.focus').css('background-color', '#019160');
    }, function () {
        // out
    }
);
