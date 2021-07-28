<template>
    <div class="container">
        <div class="container__title">
          <p>Danh sách nhân viên</p>
          <div class="container__title__button--delete">
            <i class="fas fa-trash-alt"></i>
            <p id="multiple-delete">Xóa nhân viên</p>
          </div>
          <div class="container__title__button" id="myDiv" @click="showModalBox">
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
          <input class="input-search" type="text" value="" placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại">

          <!-- Component ComboBox -->
          <TheCombobox :api='"http://cukcuk.manhnv.net/api/Department"' :type='"Department"' :mode="1"/>
          <TheCombobox :api='"http://cukcuk.manhnv.net/v1/Positions"' :type='"Position"' :mode="1"/>
          
          <div class="refresh-button">
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
            <tbody>
              <!-- Append data here! -->
                <tr v-for="employee in employees" :key="employee.EmployeeId" employee-id="" delete-id="" delete-employcode="" class="table-checkbox--default">
                    <td class="table-checkbox"><input class="checkbox" type="checkbox" delete-id=""></td>
                    <td>1</td>
                    <td class="employeeCode">{{employee.EmployeeCode}}</td>
                    <td class="fullName">{{employee.FullName}}</td>
                    <td class="gender"><div class="table-gender" title="">{{employee.GenderName}}</div></td>
                    <td class="center">{{formatDate(employee.DateOfBirth)}}</td>
                    <td class="">{{employee.PhoneNumber}}</td>
                    <td class="hidden-long-text"><div class="table-email" title="">{{employee.Email}}</div></td>
                    <td class="positionName">{{employee.PositionName}}</td>
                    <td class="departmentName">{{employee.DepartmentName}}</td>
                    <td class="salary">{{formatSalary(employee.Salary.toString())}}</td>
                    <td title="">{{employee.Address}}</td>
                </tr>
            </tbody>
          </table>
        </div>
      </div>
</template>

<script>
import axios from "axios";
import TheCombobox from '../base/BaseCombobox.vue'
export default {
    name: 'TheContent',
    components: {
        TheCombobox,
    },
    created() {
        // Gọi API lấy tất cả nhân viên
        axios.get('http://cukcuk.manhnv.net/v1/employees')
        .then(res => {
            this.employees = res.data;
        })
        .catch(err => {
            console.error(err); 
        })
    },
    data() {
        return {
            employees: [],
        }
    },
    methods: {
        showModalBox(){
            this.$parent.$parent.$emit('modalBoxUpdated')
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
        
    
    }

}
</script>

<style scoped>

</style>