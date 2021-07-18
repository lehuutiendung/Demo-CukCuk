var state = false;
var listDropdown = document.querySelectorAll('[data-dropdown]');
var x;
listDropdown.forEach(function(dropdown){
    dropdown.addEventListener('click', function(){
        state = !state;
        x = dropdown.getAttribute('data-dropdown');
        localStorage.setItem('lastDropDown', x);
        if(state){
            document.querySelector(`[data-dropdown-value = "${x}"]`).style.display = "none"; 
        }else{
            document.querySelector(`[data-dropdown-value = "${x}"]`).style.display = "block"; 
        }
    })
})

//Need fix

var listItem = document.querySelectorAll('div[data-dropdown-item]');
listItem.forEach(function(item){
    item.addEventListener('click', function(){
        // document.querySelector(`div[data-dropdown-item = "${}"]`).classList.remove("active");
        lastItemID = localStorage.getItem('lastItem');
        lastDropDown = localStorage.getItem('lastDropDown');
        y = item.getAttribute('data-dropdown-item');
        if(lastItemID == null){
            curValue = document.querySelector(`[data-dropdown-value = "${lastDropDown}"] span[data-dropdown-item = "${y}"]`).textContent;
            document.querySelector(`p[data-dropdown-item = "${lastDropDown}"]`).innerHTML = curValue;
            document.querySelector(`[data-dropdown-value = "${lastDropDown}"] div[data-dropdown-item = "${y}"]`).classList.add("active");
            document.querySelector(`[data-dropdown-value = "${lastDropDown}"] i[data-dropdown-item = "${y}"]`).style.display = "block";
        }
        else if(lastItemID != y){
            document.querySelector(`[data-dropdown-value = "${lastDropDown}"] div[data-dropdown-item = "${lastItemID}"]`).classList.remove("active");
            document.querySelector(`[data-dropdown-value = "${lastDropDown}"] i[data-dropdown-item = "${lastItemID}"]`).style.display = "none";
        }
        localStorage.setItem('lastItem', y);
        
        // Lay gia tri text cua span khi click
        curValue = document.querySelector(`[data-dropdown-value = "${lastDropDown}"] span[data-dropdown-item = "${y}"]`).textContent;
        document.querySelector(`p[data-dropdown-item = "${lastDropDown}"]`).innerHTML = curValue;
        document.querySelector(`[data-dropdown-value = "${lastDropDown}"] div[data-dropdown-item = "${y}"]`).classList.add("active");
        document.querySelector(`[data-dropdown-value = "${lastDropDown}"] i[data-dropdown-item = "${y}"]`).style.display = "block";
    })
})