/**
 * 
 * @param {*} salary 
 * @returns Chuỗi theo định dạng: 1.000.000
 * Auth: LHTDung(18/7/2021)
 */

// Function format salary (1.000.000)
function formatSalary(salary){
    var result = "";
    var len = salary.length;
    var charCount = 0;
    for(i = len - 1; i >= 0; i--){
        result += salary[i];
        charCount++;
        if(charCount%3==0){
            if(charCount == len) break;
            result +=".";
        }
    }
    return reverseString(result);
}

// Function reverse string
function reverseString(str){
    return str.split("").reverse().join("");
}
