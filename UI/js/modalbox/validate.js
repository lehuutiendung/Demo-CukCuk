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
    console.log($('input[required]'));
    $.each($('input[required]'), function (index, item) {
         if($(item).val() == ''){
            //  alert("Bạn nhập thiếu trường thông tin bắt buộc!");
            $(item).focus();
            $(item).css('border', '1px solid #F65454');
            $(item).attr('placeholder', "Bạn chưa nhập thông tin này!");
             return false;
         }
         console.log($(item).val());
    });
}

$('.email').blur(function () {
    var patternEmail  = new RegExp('^[A-Za-z0-9]+[A-Za-z0-9]*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)$');
    var value = $(this).val();
    if(!patternEmail.test(value)){
        console.log("Sai dinh dang email");
    }
});

// // Validate email
// function validateEmail(emailValue){
//     const EMAIL_REG = '^[A-Za-z0-9]+[A-Za-z0-9]*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)$';
//     if(!EMAIL_REG.test(emailValue)){
//         alert("Sai dinh dang email");
//     }
// }