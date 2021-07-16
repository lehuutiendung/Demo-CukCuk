var state = document.getElementsByClassName('combobox-data')[0].style.display;
document.getElementsByClassName('wrap-combobox')[0].addEventListener('click', () => {
    if(state == "none"){
        document.getElementsByClassName('combobox-data')[0].style.display = "block";
        state = "block";
    }else if (state == "block"){
        document.getElementsByClassName('combobox-data')[0].style.display = "none";
        state = "none";
    }else{
        document.getElementsByClassName('combobox-data')[0].style.display = "block";
        state = "block";
    }  
})