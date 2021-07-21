/**
 * 
 * @param {*} date 
 * @returns Chuỗi theo định dạng ngày: dd/mm/yy
 * Auth: LHTDung(18/7/2021)
 */

// Function format date
function formatDate(date){
    var rel = "";
    word = date.split('-');
    for(i = 0; i < 2;  i++){
        rel += word[2][i];
    }
    return rel+= '/' + word[1] + '/' + word[0];
}

// formatDate("2000-03-11T00:00:00");