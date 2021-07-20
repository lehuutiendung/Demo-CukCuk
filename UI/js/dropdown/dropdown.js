// Dropdown Gioi tinh
var dropDownData = document.querySelector('.dropdown-data');
var dropDownValue = document.querySelector('.dropdown-title');
var current; 
var dropDownDataList = [
    {id: 0, value:'Nữ'},
    {id: 1, value:'Nam'},
    {id: 2, value:'Khác'}
]

// Dropdown Vi tri
var dropDownData2 = document.querySelector('.dropdown-data2');
var dropDownValue2 = document.querySelector('.dropdown-title2');
var current2;
var dropDownDataList2 = [];
//Gọi API trả về danh sách vị trí
$.ajax({
    type: "GET",
    url: "http://cukcuk.manhnv.net/v1/Positions",
    async: false,
}).done(function(res){
    var data = res;
    $.each(data, function (index, item) { 
        var object2 = {};
        object2['id'] = item.PositionId;
        object2['value'] = item.PositionName;
        dropDownDataList2.push(object2)
    });
}).fail(function(res){
    console.log(res);
})

// Dropdown Phong ban
var dropDownData3 = document.querySelector('.dropdown-data3');
var dropDownValue3 = document.querySelector('.dropdown-title3');
var current3;
var dropDownDataList3 = [];

//Gọi API trả về danh sách phòng ban
$.ajax({
    type: "GET",
    url: "http://cukcuk.manhnv.net/api/Department",
    async: false,
}).done(function(res){
    var data = res;
    $.each(data, function (index, item) { 
        var object3 = {};
        object3['id'] = item.DepartmentId;
        object3['value'] = item.DepartmentName;
        dropDownDataList3.push(object3)
    });
}).fail(function(res){
    console.log(res);
})

// Dropdown Tinh trang cong viec
var dropDownData4 = document.querySelector('.dropdown-data4');
var dropDownValue4 = document.querySelector('.dropdown-title4');
var current4; 
var dropDownDataList4 = [
    {id: 0, value: 'Nhân viên'},
    {id: 1, value: 'Thực tập'},
    {id: 2, value: 'Khác'}
]


// Event Toggle Dropdown-Data
var dropDowns = document.querySelectorAll(".wrap-dropdown");
dropDowns.forEach((dropdown) => {
    dropdown.addEventListener('click', function () {
        dropdown.querySelector('.dropdown-data').classList.toggle('show');
    });
});
multipleDropDown(dropDownValue, dropDownDataList, dropDownData, current);
multipleDropDown(dropDownValue2, dropDownDataList2, dropDownData2, current2);
multipleDropDown(dropDownValue3, dropDownDataList3, dropDownData3, current3);
multipleDropDown(dropDownValue4, dropDownDataList4, dropDownData4, current4);

function multipleDropDown(dropDownValue, dropDownDataList, dropDownData, current){
    bindingDropdown();
    function bindingDropdown(){
        var dropDownHTML = '';
        // dropDownValue.innerText = dropDownDataList[current];

        for(i = 0; i < dropDownDataList.length; i++){
            if(i == current){
                dropDownHTML += `<div curVal=${dropDownDataList[i].id} data-item=${i} class="dropdown-data-item active">
                                    <div class="tick-item">
                                        <i class="fas fa-check"></i>
                                    </div>
                                    <span>${dropDownDataList[i].value}</span>
                                </div>`;
            }else{
                dropDownHTML += `<div data-item=${i} class="dropdown-data-item">
                                            <div class="tick-item">
                                            </div>
                                            <span>${dropDownDataList[i].value}</span>
                                        </div>`;
            }   
        }
        dropDownData.innerHTML = dropDownHTML;
        // Event for choose data item
        var dropDownItem = dropDownData.querySelectorAll('.dropdown-data-item');
        dropDownItem.forEach(function(item){
            item.addEventListener('click', function(){
                current = item.getAttribute('data-item');  
                dropDownValue.innerText = dropDownDataList[current].value;  
                bindingDropdown();
            })
        })
    }
}