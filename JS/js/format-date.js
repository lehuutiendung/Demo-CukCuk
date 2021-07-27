/**
 * 
 * @param {*} date 
 * @returns Chuỗi theo định dạng ngày: dd/mm/yy
 * Auth: LHTDung(18/7/2021)
 */

// Function format date
function formatDate(date){
    try {
        var rel = "";
        word = date.split('-');
        for(i = 0; i < 2;  i++){
            rel += word[2][i];
        }
        return rel+= '/' + word[1] + '/' + word[0];
    } catch (error) {
        
    }
}

//Function format string to date
function formatDateString(stringDate){
    var stringSplit = stringDate.split('/');
    var rel = '';
    for(i = stringSplit.length - 1 ; i >= 0; i--){
        rel += stringSplit[i];
        if(i != 0){
            rel += '-';
        }
    }
    rel = new Date (rel);
    // rel = rel.toISOString();
    rel = JSON.stringify(rel)
    console.log(rel);
    return rel;
}
// formatDateString("2000-03-11");
// formatDateString("09/07/2000");
// formatDate("2000-03-11T00:00:00");