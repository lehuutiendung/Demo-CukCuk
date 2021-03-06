/**
 *  Validate cho nhập tiền lương phân cách nhau bởi dấu '.'
 */
formatSalaryModalBox();
function formatSalaryModalBox() {
    var $input = $('.wrap-salary-text');
    $input.on( "keyup", function( event ) {
    
    // When user select text in the document, also abort.
    var selection = window.getSelection().toString();
    if ( selection !== '' ) {
        return;
    }
    
    // When the arrow keys are pressed, abort.
    if ( $.inArray( event.keyCode, [38,40,37,39] ) !== -1 ) {
        return;
    }
    
    var $this = $( this );
    
    // Get the value.
    var input = $this.val();
    
    var input = input.replace(/[\D\s\._\-]+/g, "");
        input = input ? parseInt( input, 10 ) : 0;

        $this.val( function() {
            // String được phân tách nhau bởi dấu '.'
            return ( input === 0 ) ? "" : input.toLocaleString( "id-ID" );
        } );
    } );    
}

/**
 * Chuyển lại định dạng của Salary từ String -> Number để thực hiện POST, PUT
 */
$('#btn-save').click(function (e) { 
    convertSalary();
});
function convertSalary(){
    var $this = $('.wrap-salary-text').val();
    var salary = "";
    for(i = 0; i < $this.split('.').length; i++){
        salary += $this.split('.')[i];
    }
    salary = parseInt(salary, 10);
    return salary;
}

// Validate lại Form khi nhấn "Lưu"
function validateSave(){

    var countRequired = 0;
    $.each($('input[required]'), function (index, item) {
         if($(item).val() == ''){
            $(item).focus();
            $(item).css('border', '1px solid #FF4747');
            $(item).attr('placeholder', "Bạn chưa nhập thông tin này!");
            return false;
         }
         if($(item).val() != ''){
            countRequired++;
         }
    });
    if(countRequired == 5) return true;
}
 validateEmail();
function validateEmail() {
    $('.email').blur(function () {
        //Bắt đầu kí tự thường từ a-z
        //Tiếp theo có thể xuất hiện các kí tự từ A-Z a-z, 0-9
        //Các kí tự trước @ tạo thành chuỗi dài lớn hơn 5 kí tự và ít hơn 32 kí tự
        //Domain dài 2 -> 5 kí tự
        //Subdomain dài 2-> 4 kí tự
        //Top-level domain dài 1-> 2 kí tự (có thể không có)
        var patternEmail  = new RegExp('^[a-z][A-Za-z0-9_\.]*@[a-z0-9]{2,5}(\.[a-z0-9]{2,6}){1,2}$');
        var value = $(this).val();
        if(!patternEmail.test(value)){
            console.log($('.email'));
            $('input.email').css('border', '1px solid #FF4747');
            $('.warning-email').css('display', 'block');
            console.log("Sai dinh dang email");
        }else{
            $('input.email').css('border', '1px solid #bbbbbb');
            $('.warning').css('display', 'none');
        }
    });
  }

