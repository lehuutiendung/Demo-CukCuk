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
        comboboxData.style.display = "none";
        stateComboboxData = false;
        comboboxIcon.style.transform = "rotate(0)";
        comboboxWrap.style.border = "1px solid #bbbbbb";
        comboboxIcon.style.transition = "all 0.3s ease";
    }else{
        comboboxData.style.display = "block";
        stateComboboxData = true;
        comboboxIcon.style.transform = "rotate(180deg)";
        comboboxWrap.style.border = "1px solid #019160";
        comboboxIcon.style.transition = "all 0.3s ease";
    }
})

//Event for Combobox Input
comboboxInput.addEventListener('focus', function(){
    if(!stateComboboxData){
        comboboxData.style.display = "block";
        stateComboboxData = true;
        comboboxIcon.style.transform = "rotate(180deg)";
        comboboxWrap.style.border = "1px solid #019160";
        comboboxIcon.style.transition = "all 0.3s ease";
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
            comboboxData.style.display = "none";
            stateComboboxData = false;
            comboboxIcon.style.transform = "rotate(0)";
            comboboxWrap.style.border = "1px solid #bbbbbb";
            comboboxIcon.style.transition = "all 0.3s ease";
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
            comboboxData.style.display = "none";
            stateComboboxData = false;
            comboboxIcon.style.transform = "rotate(0)";
            comboboxWrap.style.border = "1px solid #bbbbbb";
            comboboxIcon.style.transition = "all 0.3s ease";
        })
    })

}

