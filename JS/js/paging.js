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
var pageSize = null;
var totalItems = localStorage.getItem('totalEmployees');
var currentPage = 1;
var pageSize = 10;
var maxPages = 10;
var pageId = 1;
// Khi load vào trang ngay lần đầu, chạy phân trang với 10 nhân viên/trang
var allEmployees = JSON.parse(localStorage.getItem('allEmployees'));
activePaginate(paginate(totalItems, currentPage, pageSize, maxPages));
changeNumberPage(paginate(totalItems, currentPage, pageSize, maxPages));
changeNumberEmployeeFooter(paginate(totalItems, currentPage, pageSize, maxPages));
setEventClick();

$('.button__dot').first().addClass('focus');
// Xử lý sự kiện khi click chọn phân trang
$('.paging-item').click(function(){
    let val = $(this).text();
    $('.note-number-emmployees > p').text(val);
    $('.paging-data').hide();
    pageId = $(this).attr('page-id');
    if(pageId == 1){
        pageSize = 10;
        $('.number__pages').show();
    }else if(pageId == 2){
        pageSize = 20;
        $('.number__pages').show();
    }else if(pageId == 3){
        pageSize = 40;
        $('.number__pages').show();
    }else{
        loadData();
        $('.number__pages').hide();
    }
    flagShow = false; 
    $('table tbody tr').hide();
    var objPage = paginate(totalItems, currentPage, pageSize, maxPages);
    console.log("Phân trang:", objPage);
    activePaginate(objPage);
    $('.button__dot').remove();
    changeNumberPage(objPage);
    $('.button__dot').first().addClass('focus');
})

/**
 * @description Hàm bắt sự kiện cho các số trang
 * @since 25/07/2021
 * @author DUNGLHT
 */
function setEventClick(){
    $.each($('.number__pages--bind').children(), function (index) { 
        $(this).on('click', function(){
            $('.button__dot').first().removeClass('focus');
            $('.button__dot').removeClass('focus');
            $('.button__dot').css('background-color','');
            $(this).addClass('focus');
            currentPage = parseInt($(this).text());
    
            $('table tbody tr').hide();
            var objClickDot = paginate(totalItems, currentPage, pageSize, maxPages);
            activePaginate(objClickDot);
        
            $('.button__dot').remove();
            changeNumberPage(objClickDot);
            $(".button__dot:contains('" + currentPage +"')").css('background-color','#019160');

            if(currentPage == 1){
                $(".button__dot:contains(10)").css('background-color','#E9EBEE');
            }
            
            // Gán lại số số nhân viên bắt đầu và nhân viên kết thúc trên trang (startIndex - endIndex)
            $('.total__employees--change').remove();
            changeNumberEmployeeFooter(objClickDot);
        })
    });
}

//Bắt sự kiện cho button về trang đầu tiên
$('.button__footer__start').click(function (e) { 
    e.preventDefault();
    currentPage = 1;
    var obj = paginate(totalItems, currentPage, pageSize, maxPages);
    $('table tbody tr').hide();
    activePaginate(obj);
    $('.button__dot').remove();
    changeNumberPage(obj);
    $('.button__dot').first().addClass('focus');
    $('.total__employees--change').remove();
    changeNumberEmployeeFooter(obj);
});

// Bắt sự kiện cho button về trang cuối cùng
$('.button__footer__end').click(function (e) { 
    e.preventDefault();
    currentPage = Math.ceil(parseInt(totalItems,10)/parseInt(pageSize,10));
    var obj = paginate(totalItems, currentPage, pageSize, maxPages);
    $('table tbody tr').hide();
    activePaginate(obj);
    $('.button__dot').remove();
    changeNumberPage(obj);
    $('.button__dot').last().addClass('focus');
    $('.total__employees--change').remove();
    changeNumberEmployeeFooter(obj);
});

// Bắt sự kiện cho button trở về trước 1 trang
$('.button__footer__prev').click(function () { 
    if(currentPage == 1){
        currentPage = 1;
    }else{
        currentPage--;
    }
    var obj = paginate(totalItems, currentPage, pageSize, maxPages);
    $('table tbody tr').hide();
    activePaginate(obj);
    $('.button__dot').remove();
    changeNumberPage(obj);
    $(".button__dot:contains('" + currentPage +"')").css('background-color','#019160');
    if(currentPage == 1){
        $(".button__dot:contains(10)").css('background-color','#E9EBEE');
    }
    $('.total__employees--change').remove();
    changeNumberEmployeeFooter(obj);
});

// Bắt sự kiện cho button tiếp theo 1 trang
$('.button__footer__next').click(function () { 
    var maxNumberPage = Math.ceil(parseInt(totalItems,10)/parseInt(pageSize,10));
    if(currentPage != maxNumberPage){
        currentPage++;
    }else{
        currentPage == maxNumberPage;
    }
    var obj = paginate(totalItems, currentPage, pageSize, maxPages);
    $('table tbody tr').hide();
    activePaginate(obj);
    $('.button__dot').remove();
    changeNumberPage(obj);
    $(".button__dot:contains('" + currentPage +"')").css('background-color','#019160');
    $('.total__employees--change').remove();
    changeNumberEmployeeFooter(obj);
});


/**
 * @description Hàm xử lý phân trang
 * @param {*} totalItems tổng số bản ghi
 * @param {*} currentPage trang đang hoạt động hiện tại
 * @param {*} pageSize kich thước 1 trang
 * @param {*} maxPages trang lớn nhất
 * @returns 
 * @since 24/07/2021
 * @author DUNGLHT
 */
function paginate(totalItems, currentPage, pageSize, maxPages){
    // tính tổng số trang cần có
    let totalPages = Math.ceil(parseInt(totalItems,10)/parseInt(pageSize,10));
    // đảm bảo trang hiện tại không nằm ngoài phạm vi
    if (currentPage < 1) {
        currentPage = 1;
    } else if (currentPage > totalPages) {
        currentPage = totalPages;
    }

    let startPage, endPage;
    if (totalPages <= maxPages) {
        // tổng số trang ít hơn tối đa để hiển thị tất cả các trang
        startPage = 1;
        endPage = totalPages;
    } else {
        // tổng số trang nhiều hơn tối đa => tính trang bắt đầu và trang kết thúc
        let maxPagesBeforeCurrentPage = Math.floor(maxPages / 2);
        let maxPagesAfterCurrentPage = Math.ceil(maxPages / 2) - 1;
        if (currentPage <= maxPagesBeforeCurrentPage) {
            // trang hiện tại gần đầu
            startPage = 1;
            endPage = maxPages;
        } else if (currentPage + maxPagesAfterCurrentPage >= totalPages) {
            // trang hiện tại gần cuối
            startPage = totalPages - maxPages + 1;
            endPage = totalPages;
        } else {
            // trang hiện tại nằm ở vùng giữa
            startPage = currentPage - maxPagesBeforeCurrentPage;
            endPage = currentPage + maxPagesAfterCurrentPage;
        }
    }

    // tính chỉ số của bản ghi bắt đầu và kết thúc
    let startIndex = (currentPage - 1) * pageSize;
    let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

    // tạo một mảng các trang
    let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);
    var obj = {
        totalItems: parseInt(totalItems,10),
        currentPage: currentPage,
        pageSize: pageSize,
        totalPages: totalPages,
        startPage: startPage,
        endPage: endPage,
        startIndex: startIndex,
        endIndex: endIndex,
        pages: pages
    }
    return obj;
}

/**
 * @description Hàm kiểm tra dữ liệu và gắn dữ liệu lên bảng khi phân thực hiện phân trang
 * @param {*} objPage 
 * @since 25/07/2021
 * @author DUNGLHT
 */ 
function activePaginate(objPage){
    for(var i = objPage.startIndex; i <= objPage.endIndex; i++){
        if(allEmployees[i].Salary == null){
            salaryFormated = "";
        }else{
            salaryFormated = formatSalary(allEmployees[i].Salary.toString());
        }
        
        if(allEmployees[i].DateOfBirth == null){
            dateFormated = "";
        }else{
            dateFormated = formatDate(allEmployees[i].DateOfBirth.toString());
        }

        if(allEmployees[i].GenderName == null){
            allEmployees[i].GenderName = '';
        }

        if(allEmployees[i].PositionId == null){
            allEmployees[i].PositionName = '';
        }
        if(allEmployees[i].DepartmentId == null){
            allEmployees[i].DepartmentName = '';
        }
        if(allEmployees[i].Address == null){
            allEmployees[i].Address = '';
        }
        var tr = $(`<tr employee-id="${allEmployees[i].EmployeeId}" delete-id="${allEmployees[i].EmployeeId}" delete-employcode="${allEmployees[i].EmployeeCode}" class="table-checkbox--default">
                    <td class="table-checkbox"><input class="checkbox" type="checkbox" delete-id="${allEmployees[i].EmployeeId}"></td>
                    <td>`+ 1 +`</td>
                    <td class="employeeCode">`+ allEmployees[i].EmployeeCode +`</td>
                    <td class="fullName">`+ allEmployees[i].FullName +`</td>
                    <td class="gender"><div class="table-gender" title='${allEmployees[i].GenderName}'>`+ allEmployees[i].GenderName +`</div></td>
                    <td class="center">`+ dateFormated +`</td>
                    <td class="">`+ allEmployees[i].PhoneNumber +`</td>
                    <td class="hidden-long-text"><div class="table-email" title='${allEmployees[i].Email}'>`+ allEmployees[i].Email +`</div></td>
                    <td class="positionName">`+ allEmployees[i].PositionName +`</td>
                    <td class="departmentName">`+ allEmployees[i].DepartmentName +`</td>
                    <td class="salary">`+ salaryFormated +`</td>
                    <td title=>`+ allEmployees[i].Address +`</td>
                </tr>`);
        // Binding data
        $('table tbody').append(tr);

        
    }
}

/**
 * @description Hàm render ra HTML danh sách số trang
 * @param {*} objPage 
 * @since 25/07/2021
 * @author DUNGLHT
 */
function changeNumberPage(objPage){
    for(var j = objPage.startPage; j <= objPage.endPage; j++){
        var numberPage = $(`<div class="button__dot" number-page="${j}">
                                <p>${j}</p>
                            </div>`);
        $('.number__pages--bind').append(numberPage);
    }
    setEventClick();
}

function changeNumberEmployeeFooter(objPage){
    var totalEmployeesHTML = $(`<p class="total__employees--change" style="color: #5D5C5C;">Hiển thị <span style="color: #000000; font-weight: bold;">${objPage.startIndex+1}-${objPage.endIndex+1}/${objPage.totalItems}</span> nhân viên</p>`);
    $('.total__employees').append(totalEmployeesHTML);
}