<template>
    <div class="container">
        <div class="container__title">
          <p>Danh sách nhân viên</p>
          <div class="container__title__button--delete" :style="{display: showButtonDel}" @click="deleteMultiple()">
            <i class="fas fa-trash-alt"></i>
            <p id="multiple-delete">Xóa nhân viên</p>
          </div>
          <div class="container__title__button" id="myDiv" @click="showModalBox()">
            <img src="../../assets/icon/add.png" alt="">
            <p id="button">Thêm nhân viên</p>
          </div>

          <!-- Toast -->
          <div class="box-toast">
              <div class="toast">
                  <div class="wrap-start-toast">
                      <div class="toast-img toast-error">
                          <i class="fas fa-exclamation-triangle"></i>
                      </div>
                      <div class="toast-content">
                          <p>Lỗi</p>
                          <p>Nội dung đi kèm tiêu đề</p>
                      </div>
                  </div>
                  <div class="toast-close toast-close-error">
                      <i class="fas fa-times"></i>
                  </div>
              </div>
              <div class="toast">
                  <div class="wrap-start-toast">
                      <div class="toast-img toast-warning">
                          <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                      </div>
                      <div class="toast-content">
                          <p>Cảnh báo</p>
                          <p>Nội dung đi kèm tiêu đề</p>
                      </div>
                  </div>
                  <div class="toast-close toast-close-warning">
                      <i class="fas fa-times"></i>
                  </div>
              </div>
              <div class="toast">
                  <div class="wrap-start-toast">
                      <div class="toast-img toast-done">
                          <i class="fa fa-check" aria-hidden="true"></i>
                      </div>
                      <div class="toast-content">
                          <p>Thành công</p>
                          <p>Nội dung đi kèm tiêu đề</p>
                      </div>
                  </div>
                  <div class="toast-close toast-close-done">
                      <i class="fas fa-times"></i>
                  </div>
              </div>
              <div class="toast">
                  <div class="wrap-start-toast">
                      <div class="toast-img toast-notice">
                          <i class="fa fa-info" aria-hidden="true"></i>
                      </div>
                      <div class="toast-content">
                          <p>Thông tin</p>
                          <p>Nội dung đi kèm tiêu đề</p>
                      </div>
                  </div>
                  <div class="toast-close toast-close-notice">
                      <i class="fas fa-times"></i>
                  </div>
              </div>
            </div>
        </div>
        <div class="container__tools">
          <input class="input-search" type="text" value="" placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại" @keyup.enter="handleInputSearch($event)">

          <!-- Component ComboBox -->
          <TheCombobox :api='"http://cukcuk.manhnv.net/api/Department"' :type='"Department"' :mode="1"/>
          <TheCombobox :api='"http://cukcuk.manhnv.net/v1/Positions"' :type='"Position"' :mode="1"/>
          
          <div class="refresh-button" @click="refreshData()">
          </div>
        </div>
        <div class="content-table">
          <div class="hidden-scroll"></div><table>
            <thead>
              <tr>
                <th></th>
                <th>#</th>
                <th>Mã nhân viên</th>
                <th>Họ và tên</th>
                <th>Giới tính</th>
                <th class="center">Ngày sinh</th>
                <th>Điện thoại</th>
                <th>Email</th>
                <th>Chức vụ</th>
                <th>Phòng ban</th>
                <th class="salary">Mức lương cơ bản</th>
                <th>Địa chỉ</th>
              </tr>
              
            </thead>
            <colgroup>
              <col span="1" style="width: 1%;">
              <col span="1" style="width: 1%;">
              <col span="1" style="width: 1%;">
              <col span="1" style="width: 1%;">
              <col span="1" style="width: 1%;">
              <col span="1" style="width: 10%;">
              <col span="1" style="width: 10%;">
              <col span="1" style="width: 10%;">
              <col span="1" style="width: 10%;">
              <col span="1" style="width: 10%;">
              <col span="1" style="width: 10%;">
              <col span="1" style="width: 10%;">
           </colgroup>
            <tbody v-show="showTable">
              <!-- Append data here! -->
                <tr v-for="(employee,index) in employees" :key="employee.EmployeeId" @dblclick="dbClickHandle(employee.EmployeeId)" @click="clickHandle($event, employee.EmployeeId)" delete-employcode="" class="table-checkbox--default">
                    <td class="table-checkbox"><input class="checkbox" type="checkbox" @click="clickCheckBox($event)"></td>
                    <!-- <input class="checkbox" type="checkbox"> -->
                    <td>{{ ++index }}</td>
                    <td class="employeeCode">{{employee.EmployeeCode}}</td>
                    <td class="fullName">{{employee.FullName}}</td>
                    <td class="gender" title="employee.GenderName"><div class="table-gender" title="">{{employee.GenderName}}</div></td>
                    <td class="center">{{ employee.DateOfBirth == null ? '' : formatDate(employee.DateOfBirth)}}</td>
                    <td class="">{{employee.PhoneNumber}}</td>
                    <td class="hidden-long-text"><div class="table-email" title="">{{employee.Email}}</div></td>
                    <td class="positionName">{{employee.PositionName}}</td>
                    <td class="departmentName">{{employee.DepartmentName}}</td>
                    <td class="salary">{{ employee.Salary == null ? '' : formatSalary(employee.Salary.toString())}}</td>
                    <td title="">{{employee.Address}}</td>
                </tr>
            </tbody>
          </table>
        </div>
        <EmployeeDetail :mode="modeForm" :modalBoxShow="modalBoxShow" :employeeId="employeeId" :newEmployeeCode="newEmployeeCode" v-on:exitModalBox="exitModalBox()" v-on:hideModalBox="hideModalBox()" :tableUpdated="tableUpdated()"/>
        <PopUp :popUpShow="popUpShow" v-on:hidePopUp="hidePopUp($event)" :mode="modeForm"/>
    </div>
</template>

<script>
import axios from "axios";
import TheCombobox from '../../components/base/BaseCombobox.vue'
import EmployeeDetail from '../../view/employee/EmployeeDetail.vue'
import PopUp from '../../components/base/BasePopup.vue'
export default {
    name: 'EmployeePage',
    components: {
        TheCombobox,
        EmployeeDetail,
        PopUp
    },
    data() {
        return {
            employees: [],
            employeeId: '',
            modalBoxShow: false,
            popUpShow: false,
            modeForm: 0, 
            showTable: true,
            newEmployeeCode: '',
            buttonDelShow: false,
            queueDelete: [],
            activeRow: false,
            checkBox: false,
        }
    },
    mounted() {
        var vm = this;
        // Gọi API lấy tất cả nhân viên
        axios.get('http://cukcuk.manhnv.net/v1/employees')
        .then(res => {
            vm.employees = res.data
        })
        .catch(err => {
            console.error(err); 
        })
    },
    computed: {
        showButtonDel(){
            if(this.buttonDelShow){
                return 'flex';
            }else{
                return 'none';
            }
        },
        activeRowTable(){
            if(this.activeRow){
                return true;
            }else{
                return false;
            }
        },
    },
    methods: {
        // Double Click sửa form trong table
        dbClickHandle(employeeId){
            this.modalBoxShow = !this.modalBoxShow;
            this.employeeId = employeeId;
            this.modeForm = 1;
        },

        clickHandle(e,employeeId){
            this.buttonDelShow = true;
            e.currentTarget.classList.toggle("table-checkbox--default");
            e.currentTarget.classList.toggle("table-checkbox--active");
            let indexItem = this.queueDelete.indexOf(employeeId);
            console.log("Click tr");
            if(e.currentTarget.children[0].children[0].checked){
                console.log(e.currentTarget.children[0].children[0].checked);
                e.currentTarget.children[0].children[0].checked = false;

            }else{
                console.log(e.currentTarget.children[0].children[0].checked);
                e.currentTarget.children[0].children[0].checked = true;
            }
            if(indexItem == -1){
                this.queueDelete.push(employeeId);
            }else{
                //Tại vị trí indexItem, thực hiện remove 1 phần tử
                this.queueDelete.splice(indexItem, 1);
                if(this.queueDelete.length == 0){
                    this.buttonDelShow = false;
                }
            }
        },

        clickCheckBox(e){
            console.log(e.currentTarget.checked);
            console.log("click checkbox");
        },

        //Mở form thêm mới nhân viên
        showModalBox(){
            this.modalBoxShow = !this.modalBoxShow;
            this.modeForm = 0;
            axios.get("http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode")
            .then(res => {
                this.newEmployeeCode = res.data;
                console.log(this.newEmployeeCode);
            })
            .catch(err => {
                console.error(err); 
            })
        },

        exitModalBox(){
            this.popUpShow = !this.popUpShow;
        },

        hideModalBox(){
            this.modalBoxShow = !this.modalBoxShow;
        },

        hidePopUp(value){
            // value = 1: Đóng popup, giữ form
            // value = 0: Đóng popup, đóng form
            if(value == 1){
                this.popUpShow = !this.popUpShow;
            }else{
                this.popUpShow = !this.popUpShow;
                this.modalBoxShow = !this.modalBoxShow;
            }
            
        },

        tableUpdated(){
            if(this.modalBoxShow == false){
                let vm = this;
                // Gọi API lấy tất cả nhân viên
                axios.get('http://cukcuk.manhnv.net/v1/employees')
                .then(res => {
                    vm.employees = res.data;
                })
                .catch(err => {
                    console.error(err); 
                })
            }
        },

        //Format dd/mm/yyyy
        formatDate(date){
            var rel = "";
            var word = date.split('-');
            for(var i = 0; i < 2;  i++){
                rel += word[2][i];
            }
            return rel+= '/' + word[1] + '/' + word[0];
        },
        
        //Format Salary
        formatSalary(salary){
            var result = "";
            var len = salary.length;
            var charCount = 0;
            for(var i = len - 1; i >= 0; i--){
                result += salary[i];
                charCount++;
                if(charCount%3==0){
                    if(charCount == len) break;
                    result +=".";
                }
            }
            return result.split("").reverse().join("");
        },
        
        //Xử lý sự kiện refresh data
        refreshData(){
            this.showTable = !this.showTable;
            console.log("click refresh");
            let vm = this;
            // Gọi API lấy tất cả nhân viên
            axios.get('http://cukcuk.manhnv.net/v1/employees')
            .then(res => {
                vm.employees = res.data;
                this.showTable = !this.showTable;
            })
            .catch(err => {
                console.error(err); 
            })
        },

        //Xóa nhiều nhân viên
        deleteMultiple(){
            this.queueDelete.forEach(item => {
                axios
                .delete(`http://cukcuk.manhnv.net/v1/employees/${item}`)
                .then((res) => {
                    console.log(res);
                })
                .catch((err) => {
                    console.error(err);
                });
            });
        },

        //TODO: Bind data lên table
        handleInputSearch(e){
            debugger
            console.log(e.target.value);
            this.employees = [];
            axios.get(`http://cukcuk.manhnv.net/v1/Employees/Filter?pageSize=1&pageNumber=10&employeeCode=${e.target.value}`)
            .then(res => {
                console.log(res);
                debugger
                this.employees = res.data;
                debugger
                console.log(this.employees);
            })
            .catch(err => {
                console.error(err); 
            })
        }
    },
}
</script>

<style scoped>

</style>