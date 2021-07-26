/**
 * EVENT HANDLING COMBOBOX "PHÒNG BAN"
 */

/*
var comboboxWrap = document.querySelector('.wrap-combobox');
var comboboxInput = document.querySelector('.combobox');
var comboboxBtn = document.querySelector('.combobox-btn');
var comboboxData = document.querySelector('.combobox-data');
var comboboxIcon = document.querySelector('.combobox-img');
var comboboxDeleteText = document.querySelector('.fa-times-circle');

var stateComboboxData = false; 

//Event for Commbobox Button
comboboxBtn.addEventListener('click', function(){
    if(stateComboboxData){
        hideCombobox();
    }else{
        showCombobox();
    }
})

//Event for Combobox Input
comboboxInput.addEventListener('focus', function(){
    if(!stateComboboxData){
        showCombobox();
    }
})

// Binding Combobox Data
var comboboxDataList = [
    "Tất cả phòng ban",
    "Nhân sự",
    "Kỹ thuật",
    "Marketing",   
];

bindingData();

function showCombobox(){
    comboboxData.style.display = "block";
    stateComboboxData = true;
    comboboxIcon.style.transform = "rotate(180deg)";
    comboboxWrap.style.border = "1px solid #019160";
    comboboxIcon.style.transition = "all 0.3s ease";
}

function hideCombobox(){
    comboboxData.style.display = "none";
    stateComboboxData = false;
    comboboxIcon.style.transform = "rotate(0)";
    comboboxWrap.style.border = "1px solid #bbbbbb";
    comboboxIcon.style.transition = "all 0.3s ease";
}

var currentItem;       //Item choosed
// Function bind data
function bindingData(){
    var comboboxItemHtml = "";
    for(i = 0; i < comboboxDataList.length; i++){
        if(i == currentItem){
            comboboxItemHtml += `<li data-item=${i} class="combobox-data-item item-active" style="color: #ffffff">
                                    <div class="tick-item">
                                        <i class="fas fa-check"></i>
                                    </div>
                                    <span>${comboboxDataList[i]}</span>
                                </li>`;
        }else{
            comboboxItemHtml += `<li data-item=${i} class="combobox-data-item">
                                    <div class="tick-item"></div>
                                    <span>${comboboxDataList[i]}</span>
                                </li>`;
        }   
    }
    comboboxData.innerHTML = comboboxItemHtml;
     
    // Event for choose data item
    var comboboxItems = document.querySelectorAll('.combobox-data-item');
    comboboxItems.forEach(function(item){
        item.addEventListener('click', function(){
            currentItem = item.getAttribute('data-item');
            comboboxInput.value = comboboxDataList[currentItem];
            bindingData();
            hideCombobox();
        })
    })
}

// Event for input --> show button delete-text
comboboxInput.addEventListener('input', function(){
    SearchInComboBox();
    comboboxDeleteText.style.visibility = "visible";
    if(comboboxInput.value == ""){
        comboboxDeleteText.style.visibility = "hidden";
    }
})

// Event click button delete-text{
comboboxDeleteText.addEventListener('click', function(){
    if(comboboxDeleteText.style.visibility == "visible"){
        comboboxInput.value = "";
        comboboxDeleteText.style.visibility = "hidden"; 
    }    
})


// Function handle searching 
function SearchInComboBox(){
    var comboboxItemHtml = "";
    var inputValue = comboboxInput.value.toLowerCase();

    for(i = 0; i < comboboxDataList.length; i++){
        if(comboboxDataList[i].toLowerCase().includes(inputValue)){
            if(i == currentItem){
                comboboxItemHtml += `<li data-item=${i} class="combobox-data-item item-active" style="color: #ffffff">
                                        <div class="tick-item">
                                            <i class="fas fa-check"></i>
                                        </div>
                                        <span>${comboboxDataList[i]}</span>
                                    </li>`;
            }else{
                comboboxItemHtml += `<li data-item=${i} class="combobox-data-item">
                                        <div class="tick-item"></div>
                                        <span>${comboboxDataList[i]}</span>
                                    </li>`;
            }  
        }
    }
    comboboxData.innerHTML = comboboxItemHtml;

    // Event for choose data item
    var comboboxItems = document.querySelectorAll('.combobox-data-item');
    comboboxItems.forEach(function(item){
        item.addEventListener('click', function(){
            currentItem = item.getAttribute('data-item');
            comboboxInput.value = comboboxDataList[currentItem];
            bindingData();
            hideCombobox();
        })
    })
}
*/

/*****************************************************************************************************/
/**
 * EVENT HANDLING COMBOBOX "PHÒNG BAN"
 */

var comboboxWrap = document.querySelector('.wrap-combobox[data-id="1"]');
var comboboxInput = document.querySelector('.combobox[data-id="1"]')
var comboboxBtn = document.querySelector('.combobox-btn[data-id="1"]');
var comboboxData = document.querySelector('.combobox-data[data-id="1"]');
var comboboxIcon = document.querySelector('.combobox-img[data-id="1"]');
var comboboxDeleteText = document.querySelector('.fa-times-circle[data-id="1"]');
var stateComboboxData = false; 

// listDataRoom(Return API Data)
let comboboxDataList = listDataRoom;

// var comboboxWrap = document.querySelectorAll('.wrap-combobox');
// var comboboxInput = document.querySelectorAll('.combobox');
// var comboboxBtn = document.querySelectorAll('.combobox-btn');
// var comboboxData = document.querySelectorAll('.combobox-data');
// var comboboxIcon = document.querySelectorAll('.combobox-img');
// var comboboxDeleteText = document.querySelectorAll('.fa-times-circle');

//Event for Commbobox Button
comboboxBtn.addEventListener('click', function(){
    bindingData();
    if(stateComboboxData){
        hideCombobox();
    }else{
        showCombobox();
    }
})

//Event for Combobox Input
comboboxInput.addEventListener('focus', function(){
    bindingData();
    if(!stateComboboxData){
        showCombobox();
    }
})

function showCombobox(){
    comboboxData.style.display = "block";
    stateComboboxData = true;
    comboboxIcon.style.transform = "rotate(180deg)";
    comboboxWrap.style.border = "1px solid #019160";
    comboboxIcon.style.transition = "all 0.3s ease";
}

function hideCombobox(){
    comboboxData.style.display = "none";
    stateComboboxData = false;
    comboboxIcon.style.transform = "rotate(0)";
    comboboxWrap.style.border = "1px solid #bbbbbb";
    comboboxIcon.style.transition = "all 0.3s ease";
}

//Item choosed
var currentItem;     

// Function bind data
function bindingData(){
    var comboboxItemHtml = "";
    for(i = 0; i < comboboxDataList.length; i++){
        if(i == currentItem){
            comboboxItemHtml += `<li data-item=${i} class="combobox-data-item item-active" data-id="1" style="color: #ffffff">
                                    <div class="tick-item">
                                        <i class="fas fa-check"></i>
                                    </div>
                                    <span>${comboboxDataList[i]}</span>
                                </li>`;
        }else{
            comboboxItemHtml += `<li data-item=${i} class="combobox-data-item" data-id="1">
                                    <div class="tick-item"></div>
                                    <span>${comboboxDataList[i]}</span>
                                </li>`;
        }   
    }
    comboboxData.innerHTML = comboboxItemHtml;
     
    // Event for choose data item
    var comboboxItems = document.querySelectorAll('.combobox-data-item[data-id="1"]');
    comboboxItems.forEach(function(item){
        item.addEventListener('click', function(){
            currentItem = item.getAttribute('data-item');
            comboboxInput.value = comboboxDataList[currentItem];
            bindingData();
            hideCombobox();
            filterCombobox();
        })
    })
}

// Event for input --> show button delete-text
comboboxInput.addEventListener('input', function(){
    SearchInComboBox();
    comboboxDeleteText.style.visibility = "visible";
    if(comboboxInput.value == ""){
        comboboxDeleteText.style.visibility = "hidden";
    }
})

// Event click button delete-text{
comboboxDeleteText.addEventListener('click', function(){
    if(comboboxDeleteText.style.visibility == "visible"){
        comboboxInput.value = "";
        comboboxDeleteText.style.visibility = "hidden"; 
    }    
})


// Function handle searching 
function SearchInComboBox(){
    var comboboxItemHtml = "";
    var inputValue = comboboxInput.value.toLowerCase();

    for(i = 0; i < comboboxDataList.length; i++){
        if(comboboxDataList[i].toLowerCase().includes(inputValue)){
            if(i == currentItem){
                comboboxItemHtml += `<li data-item=${i} class="combobox-data-item item-active" data-id="1" style="color: #ffffff">
                                        <div class="tick-item">
                                            <i class="fas fa-check"></i>
                                        </div>
                                        <span>${comboboxDataList[i]}</span>
                                    </li>`;
            }else{
                comboboxItemHtml += `<li data-item=${i} class="combobox-data-item" data-id="1">
                                        <div class="tick-item"></div>
                                        <span>${comboboxDataList[i]}</span>
                                    </li>`;
            }  
        }
    }
    comboboxData.innerHTML = comboboxItemHtml;

    // Event for choose data item
    var comboboxItems = document.querySelectorAll('.combobox-data-item[data-id="1"]');
    comboboxItems.forEach(function(item){
        item.addEventListener('click', function(){
            currentItem = item.getAttribute('data-item');
            comboboxInput.value = comboboxDataList[currentItem];
            bindingData();
            hideCombobox();
            filterCombobox();
        })
    })
}

/*****************************************************************************************************/
/**
 * EVENT HANDLING COMBOBOX "VỊ TRÍ"
 */
var comboboxWrap2 = document.querySelector('.wrap-combobox[data-id="2"]');
var comboboxInput2 = document.querySelector('.combobox[data-id="2"]')
var comboboxBtn2 = document.querySelector('.combobox-btn[data-id="2"]');
var comboboxData2 = document.querySelector('.combobox-data[data-id="2"]');
var comboboxIcon2 = document.querySelector('.combobox-img[data-id="2"]');
var comboboxDeleteText2 = document.querySelector('.fa-times-circle[data-id="2"]');
var stateComboboxData2 = false; 

// listDataPosition (Return API Data)
var comboboxDataList2 = listDataPosition;

//Event for Commbobox Button
comboboxBtn2.addEventListener('click', function(){
    bindingData2();
    if(stateComboboxData2){
        hideCombobox2();
    }else{
        showCombobox2();
    }
})

//Event for Combobox Input
comboboxInput2.addEventListener('focus', function(){
    bindingData2();
    if(!stateComboboxData2){
        showCombobox2();
    }
})

function showCombobox2(){
    comboboxData2.style.display = "block";
    stateComboboxData2 = true;
    comboboxIcon2.style.transform = "rotate(180deg)";
    comboboxWrap2.style.border = "1px solid #019160";
    comboboxIcon2.style.transition = "all 0.3s ease";
}

function hideCombobox2(){
    comboboxData2.style.display = "none";
    stateComboboxData2 = false;
    comboboxIcon2.style.transform = "rotate(0)";
    comboboxWrap2.style.border = "1px solid #bbbbbb";
    comboboxIcon2.style.transition = "all 0.3s ease";
}

//Item choosed
var currentItem2;
       
// Function bind data
function bindingData2(){
    var comboboxItemHtml2 = "";
    for(i = 0; i < comboboxDataList2.length; i++){
        if(i == currentItem2){
            comboboxItemHtml2 += `<li data-item=${i} class="combobox-data-item item-active" data-id="2" style="color: #ffffff">
                                    <div class="tick-item">
                                        <i class="fas fa-check"></i>
                                    </div>
                                    <span>${comboboxDataList2[i]}</span>
                                </li>`;
        }else{
            comboboxItemHtml2 += `<li data-item=${i} class="combobox-data-item" data-id="2">
                                    <div class="tick-item"></div>
                                    <span>${comboboxDataList2[i]}</span>
                                </li>`;
        }   
    }
    comboboxData2.innerHTML = comboboxItemHtml2;
     
    // Event for choose data item
    var comboboxItems2 = document.querySelectorAll('.combobox-data-item[data-id="2"]');
    comboboxItems2.forEach(function(item){
        item.addEventListener('click', function(){
            currentItem2 = item.getAttribute('data-item');
            comboboxInput2.value = comboboxDataList2[currentItem2];
            bindingData2();
            hideCombobox2();
            filterCombobox();
        })
    })
}

// Event for input --> show button delete-text
comboboxInput2.addEventListener('input', function(){
    SearchInComboBox2();
    comboboxDeleteText2.style.visibility = "visible";
    if(comboboxInput2.value == ""){
        comboboxDeleteText2.style.visibility = "hidden";
    }
})

// Event click button delete-text{
comboboxDeleteText2.addEventListener('click', function(){
    if(comboboxDeleteText2.style.visibility == "visible"){
        comboboxInput2.value = "";
        comboboxDeleteText2.style.visibility = "hidden"; 
    }    
})


// Function handle searching 
function SearchInComboBox2(){
    var comboboxItemHtml2 = "";
    var inputValue2 = comboboxInput2.value.toLowerCase();

    for(i = 0; i < comboboxDataList2.length; i++){
        if(comboboxDataList2[i].toLowerCase().includes(inputValue2)){
            if(i == currentItem2){
                comboboxItemHtml2 += `<li data-item=${i} class="combobox-data-item item-active" data-id="2" style="color: #ffffff">
                                        <div class="tick-item">
                                            <i class="fas fa-check"></i>
                                        </div>
                                        <span>${comboboxDataList2[i]}</span>
                                    </li>`;
            }else{
                comboboxItemHtml2 += `<li data-item=${i} class="combobox-data-item" data-id="2">
                                        <div class="tick-item"></div>
                                        <span>${comboboxDataList2[i]}</span>
                                    </li>`;
            }  
        }
    }
    comboboxData2.innerHTML = comboboxItemHtml2;

    // Event for choose data item
    var comboboxItems2 = document.querySelectorAll('.combobox-data-item[data-id="2"]');
    comboboxItems2.forEach(function(item){
        item.addEventListener('click', function(){
            currentItem2 = item.getAttribute('data-item');
            comboboxInput2.value = comboboxDataList2[currentItem2];
            bindingData2();
            hideCombobox2();
            filterCombobox();
        })
    })
}

