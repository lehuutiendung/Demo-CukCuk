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
              <input
                class="col-1 autofocus"
                required=""
                v-on:blur="handleBlur($event)"
                v-on:focus="handleInput($event)"
                type="text"
                id="firstField"
                autocomplete=""
                title="Trường bắt buộc phải nhập!"
                v-model="employee.EmployeeCode"
              />
              <input
                class="autofocus"
                type="text"
                required=""
                v-on:blur="handleBlur($event)"
                v-on:focus="handleInput($event)"
                value=""
                id="fullName"
                autocomplete=""
                title="Trường bắt buộc phải nhập!"
                v-model="employee.FullName"
              />
            </div>
            <!-- Div ngay sinh, gioi tinh -->
            <div class="title-row">
              <p class="col-1">Ngày sinh</p>
              <p>Giới tính</p>
            </div>
            <div class="input-row">
              <input
                class="calendar DOB"
                type="date"
                date-date=""
                data-date-format="DD MMMM YYYY"
                value=""
                title="Nhập đúng định dạng ngày/tháng/năm"
                v-model="employee.DateOfBirth"
              />
              <input
                class="calendar-format calendar-salary"
                v-on:keyup="formatSalary()"
                type="text"
                placeholder="_ _/_ _/_ _ _ _"
                title="Nhập đúng định dạng ngày/tháng/năm"
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
              <input
                class="col-1 identityNumber autofocus"
                required=""
                v-on:blur="handleBlur($event)"
                v-on:focus="handleInput($event)"
                type="text"
                value=""
                autocomplete="on"
                title="Trường bắt buộc phải nhập!"
                v-model="employee.IdentityNumber"
              />
              <input
                class="calendar identityDate"
                type="date"
                value=""
                title="Nhập đúng định dạng ngày/tháng/năm"
                v-model="employee.IdentityDate"
              />
              <input
                class="calendar-identity"
                type="text"
                placeholder="_ _/_ _/_ _ _ _"
                title="Nhập đúng định dạng ngày/tháng/năm"
                v-model="employee.IdentityDate"
              />
            </div>
            <!-- Noi cap -->
            <div class="title-row">
              <p class="col-1">Nơi cấp</p>
            </div>
            <div class="input-row">
              <input
                class="col-1 identityPlace autofocus"
                type="text"
                value=""
                autocomplete="on"
                v-model="employee.IdentityPlace"
              />
            </div>
            <!-- Email, so dien thoai -->
            <div class="title-row">
              <p class="col-1">Email (<span>*</span>)</p>
              <p>Số điện thoại (<span>*</span>)</p>
            </div>
            <div class="input-row">
              <div class="warning-email">
                <div class="warning-rotate"></div>
                <div class="warning-box">
                  <p>Email sai định dạng tên miền</p>
                </div>
              </div>
              <input
                class="col-1 email autofocus"
                required=""
                v-on:blur="handleBlur($event)"
                v-on:focus="handleInput($event)"
                type="text"
                value=""
                autocomplete="on"
                title="Trường bắt buộc phải nhập!"
                v-model="employee.Email"
              />
              <input
                class="col-1 phoneNumber autofocus"
                required=""
                v-on:blur="handleBlur($event)"
                v-on:focus="handleInput($event)"
                type="text"
                value=""
                title="Trường bắt buộc phải nhập!"
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
                api="http://cukcuk.manhnv.net/v1/Positions"
                type="Position"
                :dataDropdown="dataDropdown"
                :callApi="true"
                v-on:position="getPosition"
              />
              <DropDown
                api="http://cukcuk.manhnv.net/api/Department"
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
              <input
                class="col-1 personalTaxCode autofocus"
                type="text"
                value=""
                v-model="employee.PersonalTaxCode"
              />
              <div class="wrap-salary">
                <input
                  class="wrap-salary-text"
                  type="text"
                  value=""
                  v-model.number="employee.Salary"
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
              <input
                class="calendar joinDate"
                type="date"
                value=""
                title="Nhập đúng định dạng ngày/tháng/năm"
                v-model="employee.JoinDate"
              />
              <input
                class="calendar-format calendar-joinDate"
                type="text"
                placeholder="_ _/_ _/_ _ _ _"
                title="Nhập đúng định dạng ngày/tháng/năm"
                v-model="employee.JoinDate"
              />
              <DropDown :dataValue="dataWorkStatus" type="WorkStatus" />
            </div>
          </div>
        </div>
      </div>
      <div class="footer-modal">
        <div
          class="button-modal delete"
          v-if="mode == 1"
          id="btn-delete"
          @click="deleteEmployee()"
        >
          <i class="fas fa-trash-alt"></i>
          <p>Xóa</p>
        </div>
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
export default {
  name: "EmployeeDetail",
  components: {
    DropDown,
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
      default: '',
      required: true,
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
        { WorkStatusName: "Đang làm việc" },
        { WorkStatusName: "Đang nghỉ phép" },
        { WorkStatusName: "Đã nghỉ làm" },
      ],
      gender: "",
      department: "",
      position: "",
      inputRequired: false,
      dataDropdown: {
        GenderId: this.gender,
        GenderName: '',
        DepartmentId: this.department,
        DepartmentName: '',
        PositionId: this.position,
        PositionName: '',
      },
    };
  },
  computed: {
    modalBoxState() {
      if (this.modalBoxShow) {
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
    },

    // Thay đổi trạng thái ẩn-hiện của modalbox
    changeState() {
      this.$emit("hideModalBox");
      this.$emit("tableUpdated");
    },

    getGender(value) {
      console.log("Gender", value);
      this.gender = value;
    },

    getDepartment(value) {
      console.log("Department", value);
      this.department = value;
    },

    getPosition(value) {
      console.log("Position", value);
      this.position = value;
    },
    /**
     * @description Lưu dữ liệu
     * @author DUNGLHT
     * @since 29/07/2021
     */
    saveEditEmployee() {
      let vm = this;
      if (vm.mode == 0) {
        vm.employee.Gender = vm.gender;
        vm.employee.DepartmentId = vm.department;
        vm.employee.PositionId = vm.position;
        axios
          .post(`http://cukcuk.manhnv.net/v1/employees`, vm.employee)
          .then((res) => {
            console.log(res);
            vm.changeState();
          })
          .catch((err) => {
            console.error(err);
          });
      } else {
        vm.employee.Gender = vm.gender;
        vm.employee.DepartmentId = vm.department;
        vm.employee.PositionId = vm.position;
        axios
          .put(
            `http://cukcuk.manhnv.net/v1/employees/${vm.employeeId}`,
            vm.employee
          )
          .then((res) => {
            console.log(res);
            vm.changeState();
          })
          .catch((err) => {
            console.error(err);
          });
      }
    },

    /**
     * @description Xóa nhân viên trong form chi tiết
     * @author DUNGLHT
     * @since 30/07/2021
     */
    deleteEmployee() {
      let vm = this;
      axios
        .delete(
          `http://cukcuk.manhnv.net/v1/employees/${vm.employeeId}`,
          vm.employee
        )
        .then((res) => {
          console.log(res);
          vm.changeState();
        })
        .catch((err) => {
          console.error(err);
        });
    },

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

    /**
     * Bắt lại sự kiện focus cho input
     */
    handleInput(e) {
      e.target.style.border = "1px solid #019160";
    },

    /**
     * Format Salary khi nhập
     */
    formatSalary() {
      
    },
  },
  watch: {
    employee: function () {
    
    },
    employeeId: function (value) {
      let vm = this;
      // Gọi API lấy thông tin chi tiết của 1 nhân viên
      axios
        .get(`http://cukcuk.manhnv.net/v1/employees/${value}`)
        .then((res) => {
          vm.employee = res.data;
          vm.gender = res.data.Gender;
          vm.department = res.data.DepartmentId;
          vm.position = res.data.PositionId;
          vm.dataDropdown.GenderId = res.data.Gender;
          vm.dataDropdown.GenderName = res.data.GenderName;
          vm.dataDropdown.DepartmentId = res.data.DepartmentId;
          vm.dataDropdown.PositionId = res.data.PositionId;
        })
        .then(() => {
            axios.get(`http://cukcuk.manhnv.net/api/Department/${vm.dataDropdown.DepartmentId}`)
            .then(res => {
                vm.dataDropdown.DepartmentName = res.data.DepartmentName;
            })
            .catch(err => {
              console.error(err); 
            })
        })
        .then(() => {
            axios.get(`http://cukcuk.manhnv.net/v1/Positions/${vm.dataDropdown.PositionId}`)
            .then(res => {
                vm.dataDropdown.PositionName = res.data.PositionName;
            })
            .catch(err => {
              console.error(err); 
            })
        })
        .catch((err) => {
          console.error(err);
        });
    },
    mode: function () {
      if (this.mode == 0) {
        this.employee = {};
      }
    },
  },
};
</script>

<style scoped>
@import "../../css/layout/employees/info.css";
</style>