// var state = document.getElementsByClassName('dropdown-data')[0].style.display;
// document.getElementsByClassName('dropdown')[0].addEventListener('click', () => {
//     if(state == "none"){
//         document.getElementsByClassName('dropdown-data')[0].style.display = "block";
//         state = "block";
//     }else if (state == "block"){
//         document.getElementsByClassName('dropdown-data')[0].style.display = "none";
//         state = "none";
//     }else{
//         document.getElementsByClassName('dropdown-data')[0].style.display = "block";
//         state = "block";
//     }  
// })
var state = false;
var cur = document.querySelectorAll('[data-dropdown]');
cur.forEach(function(item){
    item.addEventListener('click', function(){
        state = !state;
        if(state){
            
        }
    })
})