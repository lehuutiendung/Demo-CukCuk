<template>
  <div class="container">
    <div class="container__title">
      <p>Danh sách nhân viên</p>
      <div
        class="container__title__button--delete"
        :style="{ display: showButtonDel }"
        @click="deleteMultiple()"
      >
        <i class="fas fa-trash-alt"></i>
        <p id="multiple-delete">Xóa nhân viên</p>
      </div>
      <div class="container__title__button" id="myDiv" @click="showModalBox()">
        <img src="../../assets/icon/add.png" alt="" />
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
      <input
        class="input-search"
        type="text"
        value=""
        placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại"
        @keyup.enter="handleInputSearch($event)"
      />

      <!-- Component ComboBox -->
      <TheCombobox
        :api="'https://localhost:44338/api/Departments'"
        :type="'Department'"
        :mode="1"
        :department="department"
        @DepartmentName="handleDepartment"
      />
      <TheCombobox
        :api="'https://localhost:44338/api/Positions'"
        :type="'Position'"
        :mode="1"
        :position="position"
        @PositionName="handlePosition"
      />
      <div class="import-button"><i class="fas fa-file-import"></i></div>
      <div class="export-button" @click="handleExport(queueSelected)"><i class="fas fa-file-export"></i></div>
      <div class="refresh-button" @click="refreshData()"></div>
    </div>
    <div class="content-table">
      <div class="hidden-scroll"></div>
      <table>
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
            <th>Chức năng</th>
          </tr>
        </thead>
        <colgroup>
          <col span="1" style="width: 1%" />
          <col span="1" style="width: 1%" />
          <col span="1" style="width: 1%" />
          <col span="1" style="width: 1%" />
          <col span="1" style="width: 1%" />
          <col span="1" style="width: 10%" />
          <col span="1" style="width: 10%" />
          <col span="1" style="width: 10%" />
          <col span="1" style="width: 10%" />
          <col span="1" style="width: 10%" />
          <col span="1" style="width: 10%" />
          <col span="1" style="width: 10%" />
        </colgroup>
        <tbody v-show="showTable">
          <!-- Append data here! -->
          <tr
            v-for="(employee, index) in employees"
            :key="employee.EmployeeId"
            @dblclick="dbClickHandle(employee.EmployeeId)"
            @click="clickHandle($event, employee.EmployeeId, employee)"
            delete-employcode=""
            class="table-checkbox--default"
            :class="{'table-checkbox--active' : employee.isSelected}"
          >
            <td class="table-checkbox">
              <input
                class="checkbox"
                type="checkbox"
                v-model="employee.isSelected"
              />
            </td>
            <!-- <input class="checkbox" type="checkbox"> -->
            <td>{{ startIndex + index }}</td>
            <td class="employeeCode">{{ employee.EmployeeCode }}</td>
            <td class="fullName">{{ employee.FullName }}</td>
            <td class="gender">
              <div class="table-gender" :title="employee.GenderName">{{ employee.GenderName }}</div>
            </td>
            <td class="center">
              {{
                employee.DateOfBirth == null
                  ? ""
                  : formatDate(employee.DateOfBirth)
              }}
            </td>
            <td class="">{{ employee.PhoneNumber }}</td>
            <td class="hidden-long-text">
              <div class="table-email" :title="employee.Email">{{ employee.Email }}</div>
            </td>
            <td class="positionName">{{ employee.PositionName }}</td>
            <td class="departmentName">{{ employee.DepartmentName }}</td>
            <td class="salary">
              {{
                employee.Salary == null
                  ? ""
                  : formatSalary(employee.Salary.toString())
              }}
            </td>
            <!-- <td class="work" title=""><div class="work-status">{{ employee.WorkStatus }}</div></td> -->
            <td class="work" title="">
              <div class="column-service" :class="{'show':employee.isSelected}">
                <i class="fas fa-pencil-alt" @click.stop="dbClickHandle(employee.EmployeeId)"></i>
                <i class="fas fa-trash-alt"></i>
              </div>
            </td>

          </tr>
        </tbody>
      </table>
    </div>
    <!-- 
      tableUpdated: cập nhật table khi thêm mới nhân viên => Refresh trở về trang đầu tiên
      tableUpdatedPut: cập nhật table khi sửa nhân viên => Refresh nhưng giữ nguyên trang hiện tại
     -->
    <EmployeeDetail
      :mode="modeForm"
      v-on:changeMode="changeMode()"
      :modalBoxShow="modalBoxShow"
      :employeeId="employeeId"
      :newEmployeeCode="newEmployeeCode"
      :hideLoading="hideLoading"
      v-on:exitModalBox="exitModalBox()"
      v-on:hideModalBox="hideModalBox()"
      v-on:tableUpdated="tableUpdated"
      v-on:tableUpdatedPut="tableUpdatedPut"
      v-on:changeHideLoading="changeHideLoading"
    />
    <PopUp
      :popUpShow="popUpShow"
      v-on:hidePopUp="hidePopUp($event)"
      :mode="modeForm"
      :queueDelete="queueDelete"
      v-on:executeDelete="executeDelete()"
    />
    <TheLoading :hideLoading="hideLoading"/>
    <TheContactMisa
      :contactPopUpShow="contactPopUpShow"
      v-on:hideContactPopUp="hideContactPopUp()"
    />
  </div>
</template>

<script>
import axios from "axios";
import TheCombobox from "../../components/base/BaseCombobox.vue";
import EmployeeDetail from "../../view/employee/EmployeeDetail.vue";
import PopUp from "../../components/base/BasePopup.vue";
import TheLoading from "../../components/base/BaseLoading.vue";
import TheContactMisa from "../../components/base/BaseContactMisa.vue";
import { EventBus } from '../../main';
export default {
  name: "EmployeePage",
  components: {
    TheCombobox,
    EmployeeDetail,
    PopUp,
    TheLoading,
    TheContactMisa
  },

  data() {
    return {
      employees: [],
      employeeId: "",
      modalBoxShow: false,
      popUpShow: false,   
      contactPopUpShow: false,  // Mặc định popup contact misa bị ẩn
      modeForm: 2, // mặc định modeForm = 2, (thêm mới: 0, chỉnh sửa: 1)
      showTable: true,
      newEmployeeCode: "",
      buttonDelShow: false,
      queueDelete: [],
      queueSelected: [],
      activeRow: false,
      checkBox: false,
      update: false,
      department: "",
      departmentId: "",
      position: "",
      positionId: "",
      inputSearch: "",  //Lưu giá trị của input để truyền vào filter search
      pageSize: 10,
      pageNumber: 1,
      totalPage: 0,
      totalRecord: 0,
      maxPages: 10,
      startIndex: 1,
      endIndex: 10,
      pages: [],
      eventBus: '',
      hideLoading: false, // Khi chưa load dữ liệu -> hiển thị loading 
      rowSelected: false,
    };
  },
  
  mounted() {
    var vm = this;

    //Gọi API filter thực hiện phân trang
    axios.get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`)
    .then(res => {
      vm.employees = res.data.Data;
      this.totalPage = res.data.TotalPage;
      this.totalRecord = res.data.TotalRecord;
      let obj = this.paginate(this.totalRecord, this.pageNumber, this.pageSize, this.maxPages, this.totalPage);
      this.startIndex = obj.startIndex + 1;
      this.endIndex = obj.endIndex + 1;
      this.hideLoading = true;
      //Thêm attribute isSelected vào mỗi object employee
      this.employees.forEach(item => {
        item.isSelected = false;
      });
      switch(res.status){
          case 400: this.contactPopUpShow = true;
                    break;
          case 404: this.contactPopUpShow = true;
                    break;
          case 500: this.contactPopUpShow = true;
                    break;
      }
    })
    .then(() => {
        EventBus.$emit('updateIndex', this.startIndex, this.endIndex, this.totalRecord);
    })
    .catch(err => {
        console.error(err); 
      console.error(err); 
        console.error(err); 
        this.contactPopUpShow = true;
        this.hideLoading = true;
    })

    // Cập nhật lại số nhân viên/trang
    EventBus.$on('updatePageSize', (value) => {
      this.pageSize = value;
    })

    // Thay đổi số nhân viên/trang => pageNumber trở về 1 (Trang đầu tiên)
    EventBus.$on('updatePageNumberV2', () => {
      this.pageNumber = 1;
    })

    // Cập nhật lại số trang hiện tại khi thực hiện Click
    EventBus.$on('updateNumberClick', (data) => {
      this.pageNumber = data;
    })

    // Trở về trang đầu tiên
    EventBus.$on('firstPage', () => {
      this.pageNumber = 1;
    })

    //Trở về trước 1 trang
    EventBus.$on('prevPage', () => {
      if(this.pageNumber > 1){
        this.pageNumber--;
      }else{
        this.pageNumber = 1;
      }
    })

    //Next 1 trang
    EventBus.$on('nextPage', () => {
      if(this.pageNumber != this.totalPage){
        this.pageNumber++;
      }else{
        this.pageNumber = this.totalPage;
      }
    })

    // Trở về trang cuối cùng
    EventBus.$on('lastPage', () => {
      this.pageNumber = this.totalPage;
    })
  
  },
  computed: {
    showButtonDel() {
      if (this.buttonDelShow) {
        return "flex";
      } else {
        return "none";
      }
    },
  

  },
  methods: {
    //Thay doi mode form
    changeMode(){
      this.modeForm = 2;
    },

    // Double Click sửa form trong table
    dbClickHandle(employeeId) {
      this.modalBoxShow = !this.modalBoxShow;
      this.employeeId = employeeId;
      this.modeForm = 1;
      this.hideLoading = false;
    },

    changeHideLoading(){
      this.hideLoading = true;
    },

    hideContactPopUp(){
      this.contactPopUpShow = !this.contactPopUpShow;
    },

    clickHandle(e, employeeId, employee) {
      employee.isSelected = !employee.isSelected;
      this.buttonDelShow = true;
      this.rowSelected = !this.rowSelected;
      //Hiển thị icon cột các chức năng khi click vào dòng
      e.currentTarget.children[11].children[0].classList.toggle("show");
      let indexItem = this.queueDelete.indexOf(employeeId);
      if (indexItem == -1) {
        this.queueDelete.push(employeeId);
        this.queueSelected.push(employee);
        console.log(this.queueSelected);
      } else {
        //Tại vị trí indexItem, thực hiện remove 1 phần tử
        this.queueDelete.splice(indexItem, 1);
        this.queueSelected.splice(indexItem, 1);
        console.log(this.queueSelected);
        if (this.queueDelete.length == 0) {
          this.buttonDelShow = false;
        }
      }
    },

    //Mở form thêm mới nhân viên
    showModalBox() {
      this.modalBoxShow = !this.modalBoxShow;
      this.modeForm = 0;
    },

    exitModalBox() {
      this.popUpShow = !this.popUpShow;
    },

    hideModalBox() {
      this.modalBoxShow = !this.modalBoxShow;
    },

    hidePopUp(value) {
      // value = 1: Đóng popup, giữ form (khi thêm mới, chỉnh sửa)
      // value = 0: Đóng popup, đóng form
      // value = 2: Đóng popup, xóa danh sách trong hàng đợi xóa
      if (value == 1) {
        this.popUpShow = !this.popUpShow;
      } else if (value == 0){
        this.popUpShow = !this.popUpShow;
        this.modalBoxShow = !this.modalBoxShow;
        this.modeForm = 2;
      }else{
        this.popUpShow = !this.popUpShow;
      }
    },

    //Cập nhật table khi thêm mới nhân viên => Refresh về trang đầu tiên
    tableUpdated() {
      this.update = !this.update;
      debugger;
      if (this.modalBoxShow == false && this.update == true) {
        let vm = this;
        debugger;
        axios
          .get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${this.pageSize}&pageNumber=1`)
          .then((res) => {
            vm.employees = res.data.Data;
            this.update = !this.update;
          })
          .catch((err) => {
            console.error(err);
          });
      }
    },

    //Cập nhật table khi sửa nhân viên => Refresh và giữ nguyên trang hiện tại
    tableUpdatedPut(){
        this.update = !this.update;
        if (this.modalBoxShow == false && this.update == true) {
          let vm = this;
          axios
            .get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`)
            .then((res) => {
              vm.employees = res.data.Data;
              this.update = !this.update;
            })
            .catch((err) => {
              console.error(err);
            });
        }
    },

    //Format dd/mm/yyyy
    formatDate(date) {
      var rel = "";
      var word = date.split("-");
      for (var i = 0; i < 2; i++) {
        rel += word[2][i];
      }
      return (rel += "/" + word[1] + "/" + word[0]);
    },

    //Format Salary
    formatSalary(salary) {
      var result = "";
      var len = salary.length;
      var charCount = 0;
      for (var i = len - 1; i >= 0; i--) {
        result += salary[i];
        charCount++;
        if (charCount % 3 == 0) {
          if (charCount == len) break;
          result += ".";
        }
      }
      return result.split("").reverse().join("");
    },

    //Xử lý sự kiện refresh data (lấy tất cả bản ghi)
    refreshData() {
      let vm = this;
      vm.department = "Tất cả phòng ban";
      vm.departmentId = "";
      vm.position = "Tất cả vị trí";
      vm.positionId = "";
      // Gọi API lấy tất cả nhân viên
      axios
        .get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`)
        .then((res) => {
          vm.employees = res.data.Data;
          this.totalPage = res.data.TotalPage;
          this.totalRecord = res.data.TotalRecord;
        })
        .catch((err) => {
          console.error(err);
        });
    },

    //Hiện popup xác nhận trước khi xóa
    deleteMultiple() {
      this.popUpShow = !this.popUpShow;
      
    },

    //Thực hiện xóa và ẩn popup
    executeDelete(){
      axios.delete(`https://localhost:44338/api/v1/Employees`, {
        data: this.queueDelete,      
      })
      .then(res => {
          console.log(res)
      }).then(() => {
          //Xóa xong thực hiện refresh table, giữ trang hiện tại.
          axios
          .get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`)
          .then((res) => {
            this.employees = res.data.Data;
          })
          .catch((err) => {
            console.error(err);
          });
      })
      .catch(err => {
        console.error(err); 
      })
      this.popUpShow = !this.popUpShow;
      this.buttonDelShow = false;
    },

    // Tìm kiếm theo mã nhân viên
    handleInputSearch(e) {
      this.inputSearch = e.target.value;
      let vm = this;
      console.log(e.target.value);
      this.inputSearch = e.target.value;
      axios
        .get(
          // `http://cukcuk.manhnv.net/v1/Employees/employeeFilter?pageSize=10&pageNumber=1&employeeFilter=${e.target.value}`
          `https://localhost:44338/api/v1/Employees/Filter?pageSize=${this.pageSize}&pageNumber=${this.pageNumber}&filter=${e.target.value}`
        )
        .then((res) => {
          console.log(res.data);
          vm.employees = res.data.Data;
          console.log(vm.employees);
        })
        .catch((err) => {
          console.error(err);
        });
    },

    // Lấy tên và id của Phòng ban được $emit từ Combobox
    handleDepartment(name, id) {
      this.department = name;
      this.departmentId = id;
    },

    // Lấy tên và id của Vị trí được $emit từ Combobox
    handlePosition(name, id) {
      this.position = name;
      this.positionId = id;
    },

    //Xử lý phân trang
    paginate(totalRecord, pageNumber, pageSize, maxPages, totalPage){
      // tính tổng số trang cần có
      // đảm bảo trang hiện tại không nằm ngoài phạm vi
      if (pageNumber < 1) {
          pageNumber = 1;
      } else if (pageNumber > totalPage) {
          pageNumber = totalPage;
      }

      let startPage, endPage;
      if (totalPage <= maxPages) {
          // tổng số trang ít hơn tối đa để hiển thị tất cả các trang
          startPage = 1;
          endPage = totalPage;
      } else {
          // tổng số trang nhiều hơn tối đa => tính trang bắt đầu và trang kết thúc
          let maxPagesBeforeCurrentPage = Math.floor(maxPages / 2);
          let maxPagesAfterCurrentPage = Math.ceil(maxPages / 2) - 1;
          if (pageNumber <= maxPagesBeforeCurrentPage) {
              // trang hiện tại gần đầu
              startPage = 1;
              endPage = maxPages;
          } else if (pageNumber + maxPagesAfterCurrentPage >= totalPage) {
              // trang hiện tại gần cuối
              startPage = totalPage - maxPages + 1;
              endPage = totalPage;
          } else {
              // trang hiện tại nằm ở vùng giữa
              startPage = pageNumber - maxPagesBeforeCurrentPage;
              endPage = pageNumber + maxPagesAfterCurrentPage;
          }
      }

      // tính chỉ số của bản ghi bắt đầu và kết thúc
      let startIndex = (pageNumber - 1) * pageSize;
      let endIndex = Math.min(startIndex + pageSize - 1, totalRecord - 1);

      // tạo một mảng các trang
      let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);
      var obj = {
          totalRecord: totalRecord,
          pageNumber: pageNumber,
          pageSize: pageSize,
          totalPage: totalPage,
          startPage: startPage,
          endPage: endPage,
          startIndex: startIndex,
          endIndex: endIndex,
          pages: pages
      }
      return obj;
    },

    //Gọi API thực hiện phân trang, và $emit cập nhật số trang đến TheFooter
    callAPIPaging(pageSize, pageNumber){
      //Hiển thị loading đợi API thực hiện thành công
      this.hideLoading = false;
      axios.get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${pageSize}&pageNumber=${pageNumber}`)
      .then(res => {
        this.employees = res.data.Data;
        this.totalPage = res.data.TotalPage;
        this.totalRecord = res.data.TotalRecord;
        let objPage = this.paginate(this.totalRecord, this.pageNumber, this.pageSize, this.maxPages, this.totalPage);
        this.startIndex = objPage.startIndex + 1;
        this.endIndex = objPage.endIndex + 1;
        this.pages = objPage.pages;
        //Ẩn loading
        this.hideLoading = true;
      }).then(()=>{
        //Emit cập nhật số trang
        EventBus.$emit('updatePageNumber', this.pageNumber, this.pages);
        EventBus.$emit('updateIndex', this.startIndex, this.endIndex, this.totalRecord);

      })
      .catch(err => {
        console.error(err); 
      })
    },

    //Gọi API thực hiện tìm kiếm theo (pageSize, pageNumber, inputSearch, departmentId, positionId)
    callAPIFilter(pageSize, pageNumber, inputValue, department, position){
      //Hiển thị loading đợi API thực hiện thành công
      this.hideLoading = false;
      axios.get(`https://localhost:44338/api/v1/Employees/Filter?pageSize=${pageSize}&pageNumber=${pageNumber}&filter=${inputValue}&departmentId=${department}&positionId=${position}`)
      .then(res => {
        console.log(res)
        this.employees = res.data.Data;
        //Ẩn loading
        this.hideLoading = true;
      })
      .catch(err => {
        console.error(err); 
      })
    },

    //Export file excel
    handleExport(exportList){
      axios.post(`https://localhost:44338/api/v1/Customers/Export`, exportList, {
        responseType: 'blob',
      })
      .then(res => {
        console.log(res)
        console.log(res.data.FileGuid);
        let blob = new Blob([res.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' } ), url = window.URL.createObjectURL(blob);
        window.open(url)
      })
      .catch(err => {
        console.error(err); 
      })
    }
  },

  watch:{
    departmentId: function(){
        this.callAPIFilter(this.pageSize, this.pageNumber, this.inputSearch, this.departmentId, this.positionId);   
    },

    positionId: function(){
        this.callAPIFilter(this.pageSize, this.pageNumber, this.inputSearch, this.departmentId, this.positionId);
      
    },
    
    pageNumber: function () {
      this.callAPIPaging(this.pageSize, this.pageNumber);
    },

    pageSize: function (){
      this.callAPIPaging(this.pageSize, this.pageNumber);
    }
  }
};
</script>

<style scoped>
</style>