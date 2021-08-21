<template>
  <div class="wrap-box-modal" :style="{ display: modalBoxState }">
    <div class="box-info">
      <div class="content-scroll">
        <div class="title__employee">
          <p>THÔNG TIN NHÂN VIÊN</p>
          <div class="exit">
            <i id="exit-modal" class="fas fa-times" @click="changePopUp()"></i>
          </div>
        </div>
        <div class="wrap-body">
          <div class="left-menu">
            <div class="wrap-info">
              <div class="avatar__info">
                <input type="file" />
              </div>
            </div>
            <p>(Vui lòng chọn ảnh có định dạng .jpg,.jpeg,.png,.gif.)</p>
          </div>
          <div class="right-menu">
            <p class="title-a">A. THÔNG TIN CHUNG:</p>
            <div class="hr-custom"></div>
            <!-- Div ma nhan vien, ho va ten -->
            <div class="title-row">
              <p class="col-1">Mã nhân viên (<span>*</span>)</p>
              <p>Họ và tên (<span>*</span>)</p>
            </div>
            <div class="input-row">
              <Warning :style="{ display: employee.EmployeeCode?'none':(warnRequired ? 'flex':'none') }" />
              <Input
                :required="true"
                :newEmployeeCode="newEmployeeCode"
                :value="employee.EmployeeCode"
                ref="focusField"
                v-model="employee.EmployeeCode"
                
              />
              <Warning :rightWarning="true" :style="{ display: employee.FullName?'none':(warnRequired ? 'flex':'none')}"/> 
              <Input
                :required="true"
                ref="fullName"
                :value="employee.FullName"
                v-model="employee.FullName"
              />
            </div>
            <!-- Div ngay sinh, gioi tinh -->
            <div class="title-row">
              <p class="col-1">Ngày sinh</p>
              <p>Giới tính</p>
            </div>
            <div class="input-row">
              <Input
                :isDate="true"
                :DOB="true"
                ref="dateOfBirth"
                :value="employee.DateOfBirth"
                v-model="employee.DateOfBirth"
              />
              <DropDown
                :dataValue="dataGender"
                :dataDropdown="dataDropdown"
                :mode="mode"
                type="Gender"
                v-on:gender="getGender"
              />
            </div>
            <!-- Div CMTND, ngay cap -->
            <div class="title-row">
              <p class="col-1">Số CMTND/ Căn cước (<span>*</span>)</p>
              <p>Ngày cấp</p>
            </div>
            <div class="input-row">
              <Warning :style="{ display: employee.IdentityNumber?'none':(warnRequired ? 'flex':'none') }"/>
              <Input
                :required="true"
                ref="identity"
                :value="employee.IdentityNumber"
                v-model="employee.IdentityNumber"
              />
              <Input
                :isDate="true"
                :identity="true"
                :value="employee.IdentityDate"
                v-model="employee.IdentityDate"
              />
            </div>
            <!-- Noi cap -->
            <div class="title-row">
              <p class="col-1">Nơi cấp</p>
            </div>
            <div class="input-row">
              <Input
                :normal="true"
                :value="employee.IdentityPlace"
                v-model="employee.IdentityPlace"
              />
            </div>
            <!-- Email, so dien thoai -->
            <div class="title-row">
              <p class="col-1">Email (<span>*</span>)</p>
              <p>Số điện thoại (<span>*</span>)</p>
            </div>
            <div class="input-row">
              <div class="warning" :style="{ display: showErrorBox }">
                <div class="warning-rotate"></div>
                <div class="warning-box">
                  <p>Email sai định dạng tên miền</p>
                </div>
              </div>
              <Warning :style="{ display: employee.Email?'none':(warnRequired ? 'flex':'none') }"/>
              <Input
                :required="true"
                ref="email"
                :value="employee.Email"
                v-model="employee.Email"
                @blur="validateEmail($event, employee.Email)"
              />
              <Warning :rightWarning="true" :style="{ display: employee.PhoneNumber?'none':(warnRequired ? 'flex':'none') }"/>
              <Input
                :required="true"
                ref="phone"
                :value="employee.PhoneNumber"
                v-model="employee.PhoneNumber"
              />
            </div>
            <p class="title-a">B. THÔNG TIN CÔNG VIỆC:</p>
            <div class="hr-custom"></div>
            <!-- Vi tri, phong ban -->
            <div class="title-row">
              <p class="col-1">Vị trí</p>
              <p>Phòng ban</p>
            </div>
            <div class="input-row">
              <DropDown
                api="https://localhost:44338/api/Positions"
                type="Position"
                :dataDropdown="dataDropdown"
                :callApi="true"
                v-on:position="getPosition"
              />
              <DropDown
                api="https://localhost:44338/api/Departments"
                type="Department"
                :dataDropdown="dataDropdown"
                :callApi="true"
                v-on:department="getDepartment"
              />
            </div>
            <!-- Ma so thue ca nhan, muc luong co ban -->
            <div class="title-row">
              <p class="col-1">Mã số thuê cá nhân</p>
              <p>Mức lương cơ bản</p>
            </div>
            <div class="input-row salary-box">
              <Input
                :normal="true"
                :value="employee.PersonalTaxCode"
                v-model="employee.PersonalTaxCode"
              />
              <div class="wrap-salary">
                <input
                  class="wrap-salary-text"
                  type="text"
                  value=""
                  v-model="employee.Salary"
                  @keyup="inputSalary($event)"
                />
                <span>(VNĐ)</span>
              </div>
            </div>
            <!-- Ngay gia nhap cong ty, tinh trang cong viec -->
            <div class="title-row">
              <p class="col-1">Ngày gia nhập công ty</p>
              <p>Tình trạng công việc</p>
            </div>
            <div class="input-row">
              <Input
                :isDate="true"
                :joinDate="true"
                :value="employee.JoinDate"
                v-model="employee.JoinDate"
              />
              <DropDown 
                :dataValue="dataWorkStatus" 
                :dataDropdown="dataDropdown"
                :mode="mode"
                type="WorkStatus"
                v-on:workStatus="getWorkStatus" 
              />
            </div>
          </div>
        </div>
      </div>
      <div class="footer-modal">
        <!-- <div
          class="button-modal delete"
          v-if="mode == 1"
          id="btn-delete"
          @click="deleteEmployee()"
        >
          <i class="fas fa-trash-alt"></i>
          <p>Xóa</p>
        </div> -->
        <div class="button-modal cancel" id="btn-cancel" @click="changePopUp()">
          <p>Hủy</p>
        </div>
        <div
          class="button-modal save"
          id="btn-save"
          @click="saveEditEmployee()"
        >
          <i class="far fa-save"></i>
          <p>Lưu</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import DropDown from "../../components/base/BaseDropdown.vue";
import Input from "../../components/base/BaseInputField.vue";
import Warning from "../../components/base/BaseWarning.vue";

export default {
  name: "EmployeeDetail",
  components: {
    DropDown,
    Input,
    Warning,
  },
  props: {
    modalBoxShow: Boolean,
    employeeId: {
      type: String,
      default: "",
      required: true,
    },
    mode: {
      type: Number,
      default: 0, // 0 - Thêm mới, 1 - Sửa
      required: true,
    },
    newEmployeeCode: {
      type: String,
      default: "",
      required: true,
    },
    update: {
      type: Boolean,
    }
  },
  data() {
    return {
      show: true,
      employee: {},
      dataGender: [
        { GenderName: "Nữ", GenderId: 0 },
        { GenderName: "Nam", GenderId: 1 },
        { GenderName: "Khác", GenderId: 2 },
      ],
      dataWorkStatus: [
        { WorkStatusName: "Đang làm việc", WorkStatusId: 0 },
        { WorkStatusName: "Đang nghỉ phép", WorkStatusId: 1 },
        { WorkStatusName: "Đã nghỉ làm", WorkStatusId: 2 },
      ],
      gender: 0,
      workStatus: 0,
      department: "142cb08f-7c31-21fa-8e90-67245e8b283e",
      position: "30d41e52-5e66-72bc-6c1c-b47866e0b131",
      inputRequired: false,
      dataDropdown: {
        GenderId: this.gender,
        GenderName: "Nữ",
        DepartmentId: this.department,
        DepartmentName: "Phòng Marketting",
        PositionId: this.position,
        PositionName: "Giám đốc",
        WorkStatusId: this.workStatus,
        WorkStatusName: "Đang làm việc"
      },
      showError: false,      //Thay đổi trạng thái của Popup
      validateSave: false,   //Validate khi lưu
      warnRequired: false,   //Ẩn cảnh báo khi click vào input
    };
  },
  mounted() {
      
  },
  computed: {
    modalBoxState() {
      if (this.modalBoxShow) {
        return "flex";
      } else {
        return "none";
      }
    },
    showErrorBox() {
      if (this.showError) {
        return "flex";
      } else {
        return "none";
      }
    },
  },
  methods: {

    // Thay đổi trạng thái ẩn-hiện của popup
    changePopUp() {
      this.$emit("exitModalBox");
      // this.$emit('tableUpdated');
      this.showError = false;
      this.validateSave = false;
      this.warnRequired = false;
    },

    // Thay đổi trạng thái ẩn-hiện của modalbox, và refresh table (Mặc định về trang đầu tiên của table)
    changeState() {
      this.$emit("hideModalBox");
      this.$emit("tableUpdated");
      this.$emit("update");
      this.$emit('changeMode');
    },

    changeStatePut(){
      this.$emit("hideModalBox");
      this.$emit("tableUpdatedPut");
      this.$emit("update");
      this.$emit('changeMode');
    },
    getGender(value, name) {
      console.log("Gender", value);
      this.gender = value;
      this.dataDropdown.GenderName = name;
    },

    getWorkStatus(value, name){
      console.log("WorkStatus", value);
      this.workStatus = value;
      this.dataDropdown.WorkStatusName = name; 
    },

    getDepartment(value, name) {
      console.log("Department", value);
      this.department = value;
      this.dataDropdown.DepartmentName = name;
    },

    getPosition(value, name) {
      console.log("Position", value);
      this.position = value;
      this.dataDropdown.PositionName = name;
    },
    /**
     * @description Lưu dữ liệu1
     * @author DUNGLHT
     * @since 29/07/2021
     */
    saveEditEmployee() {
      let vm = this;
      let codeValue = vm.$refs.focusField.value;
      let nameValue = vm.$refs.fullName.value;
      let identityValue = vm.$refs.identity.value;
      let emailValue = vm.$refs.email.value;
      let phoneValue = vm.$refs.phone.value;
      if( (codeValue == "" || nameValue == "" || identityValue == "" || emailValue == "" || phoneValue == "")){
          vm.validateRequired();
          // vm.validateSave = true;
          // vm.warnRequired = true;
          console.log("Không thể lưu");
      }else{
        if(!this.showError){
          vm.employee.Salary = vm.convertSalary();
          if (vm.mode == 0) {
            
            vm.employee.Gender = vm.gender;
            vm.employee.DepartmentId = vm.department;
            vm.employee.PositionId = vm.position;
            vm.employee.workStatus = vm.workStatus;
            axios
              // .post(`http://cukcuk.manhnv.net/v1/employees`, vm.employee)
              .post(`https://localhost:44338/api/v1/Employees`, vm.employee)
              .then((res) => {
                console.log(res);
                // debugger // eslint-disable-line
                vm.changeState();
              })
              .catch((err) => {
                console.error(err);
              });
          } else {
            vm.employee.Gender = vm.gender;
            vm.employee.DepartmentId = vm.department;
            vm.employee.PositionId = vm.position;
            vm.employee.workStatus = vm.workStatus;
            axios
              .put(
                // `http://cukcuk.manhnv.net/v1/employees/${vm.employeeId}`,
                `https://localhost:44338/api/v1/Employees/${vm.employeeId}`,
                vm.employee
              )
              .then((res) => {
                console.log(res);
                vm.changeStatePut();
              })
              .catch((err) => {
                console.error(err);
              });
          }
        }else{
          console.log("Lỗi email, không thể lưu");
        }
      }
    },

    /**
     * @description Xóa nhân viên trong form chi tiết, tính năng này đã bỏ
     * @author DUNGLHT
     * @since 30/07/2021
     */
    // deleteEmployee() {
    //   let vm = this;
    //   axios
    //     .delete(
    //       `http://cukcuk.manhnv.net/v1/employees/${vm.employeeId}`
    //     )
    //     .then((res) => {
    //       console.log(res);
    //       vm.changeState();
    //     })
    //     .catch((err) => {
    //       console.error(err);
    //     });
    // },

    // Validate cho input
    /**
     * Bắt sự kiện blur, validate cho input
     */
    handleBlur(e) {
      if (e.target.value == "") {
        e.target.style.border = "1px solid #FF4747";
      } else {
        e.target.style.border = "1px solid #bbbbbb";
      }
    },

    // Validate cho input khi click Lưu
    validateRequired(){
      var countRequired = 0;
      /*eslint no-extra-semi: "error"*/
      var list = this.$el.querySelectorAll(`input.required`);
      console.log(list);
      for(let i = 0; i < list.length; i++){
      // this.$el.querySelectorAll(`input.required`).forEach(element => {
        if(list[i].value == ''){
            list[i].focus();
            this.validateSave = true;
            this.warnRequired = true;
            return false;
        }
        if(list[i].value != ''){
            this.validateSave = false;
            this.warnRequired = false;
            
        }
        countRequired++;
      }
      if(countRequired == 5) return true;
    },

    /**
     * Bắt lại sự kiện focus cho input
     */
    handleInput(e) {
      e.target.style.border = "1px solid #019160";
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

    //Validate Email
    validateEmail(e, value) {
      var patternEmail =
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      if (!patternEmail.test(value)) {
        e.target.style.border = "1px solid #FF4747";
        this.showError = true;
        console.log(this.showError);
        return true;
      } else {
        this.showError = false;
        console.log(this.showError);
        console.log("Email OK!");
        return false;
      }
    },

    //Gán mã nhân viên tự sinh cho employee
    // updateCode(){
    //   if(this.mode == 0){
    //     this.employee.EmployeeCode = this.newEmployeeCode;
    //     return this.employee.EmployeeCode;
    //   }
    // },

    // Format Salary khi nhập
    inputSalary(e){
      var selection = window.getSelection().toString();
      if ( selection !== '' ) {
          return;
      }
      let input = e.target.value;
      // eslint-disable-next-line
      input = input.replace(/[\D\s\._\-]+/g, "");
      input = input ? parseInt( input, 10 ) : 0;
      input === 0 ? "" : input.toLocaleString("id-ID");
      this.employee.Salary = input.toLocaleString("id-ID");
      console.log(this.employee.Salary);
      console.log(input.toLocaleString("id-ID"));
    },

    //Format lại Lương khi thực hiện Lưu dữ liệu
    convertSalary(){
      let $this = this.$el.querySelector(`input.wrap-salary-text`).value;
      let salary = "";
      for(let i = 0; i < $this.split('.').length; i++){
        salary += $this.split('.')[i];
      } 
      salary = parseInt(salary, 10);
      return salary;
    },

    //Format Salary
    formatSalary(salary){
      if(salary){
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
      } 
    }
  },
  watch: {
    employee: function () {
      this.$refs.focusField.$el.focus();
    },
    
    mode: function () {
      let vm = this;
      // Gọi API lấy thông tin chi tiết của 1 nhân viên

      // Nếu mode = 1: Form sửa => Gọi API
      if(this.mode == 1){
        axios
        // .get(`http://cukcuk.manhnv.net/v1/employees/${this.employeeId}`)
        .get(`https://localhost:44338/api/v1/Employees/${this.employeeId}`)
        .then((res) => {
          vm.employee = res.data;
          vm.gender = res.data.Gender;
          vm.department = res.data.DepartmentId;
          vm.position = res.data.PositionId;
          vm.dataDropdown.GenderId = res.data.Gender;
          vm.dataDropdown.GenderName = res.data.GenderName;
          vm.dataDropdown.DepartmentId = res.data.DepartmentId;
          vm.dataDropdown.PositionId = res.data.PositionId;
          vm.dataDropdown.WorkStatusId = res.data.WorkStatus;
          vm.employee.Salary = this.formatSalary(res.data.Salary.toString());
        })
        .then(() => {
          axios
            .get(
              // `http://cukcuk.manhnv.net/api/Department/${vm.dataDropdown.DepartmentId}`
              `https://localhost:44338/api/Department/${vm.dataDropdown.DepartmentId}`
            )
            .then((res) => {
              vm.dataDropdown.DepartmentName = res.data.DepartmentName;
            })
            .catch((err) => {
              console.error(err);
            });
        })
        .then(() => {
          axios
            .get(
              // `http://cukcuk.manhnv.net/v1/Positions/${vm.dataDropdown.PositionId}`
              `https://localhost:44338/api/Position/${vm.dataDropdown.PositionId}`
            )
            .then((res) => {
              vm.dataDropdown.PositionName = res.data.PositionName;
            })
            .catch((err) => {
              console.error(err);
            });
        })
        .catch((err) => {
          console.error(err);
        });
      }
      // Nếu mode = 0: Form thêm mới => Clear dữ liệu cũ
      if (this.mode == 0) {
        this.employee = {};
        axios
        // .get("http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode")
        .get("https://localhost:44338/api/v1/Employees/NewCode")
        .then((res) => {
          this.$set(this.employee, "EmployeeCode", res.data.EmployeeCode );
          console.log(this.newEmployeeCode);
        })
        .catch((err) => {
          console.error(err);
        });
      }
    
    },
    
    modalBoxShow: function(){
      this.employee = {};
      console.log(this.$el.querySelectorAll(`input.required`)); 
      this.$el.querySelectorAll(`input.required`).forEach(element => {
        element.style.border = "1px solid #bbbbbb";
      });
      this.$nextTick(() =>  {
        if(this.modalBoxShow == true){
          this.$refs.focusField.$el.children[0].focus();
        }
      })
    },
  },
};
</script>

<style scoped>
@import "../../css/layout/employees/info.css";
</style>