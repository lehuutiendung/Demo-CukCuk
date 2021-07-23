/**
 * @description Hiện modal-box và tự động focus, lấy mã nhân viên mới
 * @author DUNGLHT
 */
$('.wrap-box-modal').hide();
$('#myDiv').on('click', function(){
    $('.delete').hide();
    $('.wrap-box-modal').show();
    //Clear data khi click vào lại button thêm mới nhân viên
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
 * @description Search Input
 * @author DUNGLHT
 */
$(document).ready(function() {
    $('.input-search').on('keyup', function(event) {
       event.preventDefault();
       /* Act on the event */
       var tukhoa = $(this).val().toLowerCase();
       $('table tbody tr').filter(function() {
          $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa)>-1);
       });
    });
 });


/**
 * @deprecated Click button refresh
 * @author DUNGLHT
 */
$(document).ready(function(){
    $('.refresh-button').on('click', function(){
        $('table tbody tr').detach();
        loadData();
    })
})

/**
 * @description Xử lý click "Xóa nhân viên"
 * @author DUNGLHT
 * @since 23/07/2021
 */ 
var queueDelete = [];
$('table').on('click', 'tbody tr td .checkbox', function(){
    $('.container__title__button--delete').css('display', 'flex');
    let employeeId = $(this).attr('delete-id');
    let indexItem = queueDelete.indexOf(employeeId);
    if(indexItem == -1){
        queueDelete.push(employeeId);
    }else{
        //Tại vị trí indexItem, thực hiện remove 1 phần tử
        queueDelete.splice(indexItem, 1);
    }
    console.log(queueDelete);
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
    $('table tbody tr').detach();
    loadData()
    $('.container__title__button--delete').css('display', 'none');
    queueDelete = [];
    loadTotalNumberData();
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

//Xử lý sự kiện click hiển thị danh sách các lựa chọn số nhân viên/trang
var flagShow = false;
$('.note').click(function() { 
    if(!flagShow){
        $('.paging-data').show();
        flagShow = true;
    }else{
        $('.paging-data').hide();
        flagShow = false;
    }
});

// Xử lý sự kiện chọn item số nhân viên/trang
var numEmployeeInPage = null;
var numTotal = localStorage.getItem('totalEmployees');
$('.paging-item').click(function(){
    let val = $(this).text();
    let temp = '';
    temp = $('.note-number-emmployees > p').text();
    $('.note-number-emmployees > p').text(val);
    $(this).text(temp);
    $('.paging-data').hide();
    flagShow = false;
    //Lấy số nhân viên/trang
    numEmployeeInPage = $('.note-number-emmployees > p').text();
    console.log(numTotal);
    let numPaging = Math.floor(parseInt(numTotal,10)/parseInt(numEmployeeInPage,10) + 1);
    console.log(numPaging);
})
