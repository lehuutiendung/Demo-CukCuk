/**
 * Xử lý sự kiện ẩn hiển modal-box
 */

// document.getElementById("myDiv").onclick = function () {
//     document.getElementsByClassName("wrap-box-modal")[0].style.display = "flex";
//     document.getElementById("firstField").focus();
//     getNewCode();
// }

// Hiện modal-box và tự động focus, lấy mã nhân viên mới
$('.wrap-box-modal').hide();
$('#myDiv').on('click', function(){
    $('.delete').hide();
    $('.wrap-box-modal').show();
    //Clear data khi click vào lại button thêm mới nhân viên
    $('.input-row > input').val('');
    document.querySelector('.dropdown-title > p').innerText = "Chọn giới tính";
    document.querySelector('.dropdown-title2 > p').innerText = "Chọn vị trí";
    document.querySelector('.dropdown-title3 > p').innerText = "Chọn phòng ban";
    document.querySelector('.dropdown-title4 > p').innerText = "Chọn tình trạng";

    $('#firstField').focus();
    getNewCode();
})

// Ẩn modal-box khi click button exit
document.getElementById("exit-modal").onclick = function (){
    document.getElementsByClassName("wrap-box-modal")[0].style.display = "none";
}

//Search Input
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

//Click button refresh
$(document).ready(function(){
    $('.refresh-button').on('click', function(){
        $('table tbody tr').hide();
        loadData();
    })
})