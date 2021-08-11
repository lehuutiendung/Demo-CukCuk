<template>
  <div class="footer">
    <div class="total__employees">
      <!-- <p style="color: #5D5C5C;">Hiển thị <span style="color: #000000; font-weight: bold;">1-10/1000</span> nhân viên</p> -->
      <p class="total__employees--change" style="color: #5d5c5c">
        Hiển thị
        <span style="color: #000000; font-weight: bold">{{ startIndex }}-{{ endIndex }}/{{ totalRecord }}</span> nhân
        viên
      </p>
    </div>
    <div class="number__pages">
      <div
        class="button__hide button__footer__start"
        @click="handleFirstPage()"
      >
        <img src="../../assets/icon/btn-firstpage.svg" alt="" />
      </div>
      <div class="button__hide button__footer__prev" @click="handlePrevPage()">
        <img src="../../assets/icon/btn-prev-page.svg" alt="" />
      </div>
      <div class="number__pages--bind">
        <div
          class="button__dot"
          :class="{ focus: numberPage == page ? true : false }"
          v-for="page in pages"
          :key="page"
          @click="getValuePageNumber(page)"
        >
          <p>{{ page }}</p>
        </div>
      </div>

      <div class="button__hide button__footer__next" @click="handleNextPage()">
        <img src="../../assets/icon/btn-next-page.svg" alt="" />
      </div>
      <div class="button__hide button__footer__end" @click="handleLastPage()">
        <img src="../../assets/icon/btn-lastpage.svg" alt="" />
      </div>
    </div>
    <div class="note" @click="clickChoosePaging()">
      <div class="note-number-emmployees">
        <p style="color: #5d5c5c">
          <span style="color: #000000; font-weight: bold" v-html="myHTMLPaging"></span> nhân
          viên/trang
        </p>
      </div>
      <div class="up-down-number">
        <i class="fas fa-chevron-up"></i>
        <i class="fas fa-chevron-down"></i>
      </div>
    </div>
    <div class="paging-data" :style="{ display: showPaging }">
      <!-- <div class="paging-item" page-id="4"><span>Tất cả</span> nhân viên</div>
            <div class="paging-item" page-id="3"><span>40</span> nhân viên/trang</div>
            <div class="paging-item" page-id="2"><span>20</span> nhân viên/trang</div>
            <div class="paging-item" page-id="1"><span>10</span> nhân viên/trang</div> -->
      <div
        class="paging-item"
        page-id="3"
        v-for="(item, index) in employeeInPage"
        :key="index"
        @click="getValuePageSize($event)"
      >
        <span>{{ item }}</span> nhân viên/trang
      </div>
    </div>
  </div>
</template>

<script>
import { EventBus } from "../../main";
export default {
  name: "TheFooter",
  data() {
    return {
      numberPage: 1,
      pages: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      numberClicked: 1,
      showPagingData: false,
      employeeInPage: [40, 20, 10],
      myHTMLPaging: '10',
      startIndex: 1,
      endIndex: 10,
      totalRecord: 0,
    };
  },

  mounted() {
    // Update số trang hiện tại và danh sách trang hiển thị (Được $emit từ EmployeeList)
    EventBus.$on("updatePageNumber", (data, pages) => {
      this.numberPage = data;
      this.pages = pages;
    });

    // Update số bắt đầu và kết thúc trong trang ()
    EventBus.$on('updateIndex', (start, end, total) => {
      this.startIndex = start;
      this.endIndex = end;
      this.totalRecord = total;
    })
  },
  computed: {
    showPaging() {
      if (this.showPagingData) {
        return "block";
      } else {
        return "none";
      }
    },
  },

  methods: {
    //Xử lý click trở về trang đầu tiên => Emit tới EmployeeList để phân trang
    handleFirstPage() {
      EventBus.$emit("firstPage");
    },

    //Xử lý click trở về trước 1 trang => Emit tới EmployeeList để phân trang
    handlePrevPage() {
      EventBus.$emit("prevPage");
    },

    //Xử lý click next 1 trang => Emit tới EmployeeList để phân trang
    handleNextPage() {
      EventBus.$emit("nextPage");
    },

    //Xử lý click trở về trang cuối cùng => Emit tới EmployeeList để phân trang
    handleLastPage() {
      EventBus.$emit("lastPage");
    },

    // Click chọn số trang và thực hiện $emit để đồng bố số trang hiện tại giữa TheFooter và EmployeeList
    getValuePageNumber(current) {
      this.numberPage = current;
      EventBus.$emit("updateNumberClick", current);
    },

    //Thay đổi biến ẩn hiện data số nhân viên/trang
    clickChoosePaging() {
      this.showPagingData = !this.showPagingData;
    },

    //Lấy giá trị số nhân viên/trang khi Click
    getValuePageSize(e){
      let value = e.target.innerText;
      let finalValue = "";
      for(let i = 0; i < 2; i++){
         finalValue += value[i];
      }
      finalValue = parseInt(finalValue, 10);
      this.showPagingData = !this.showPagingData;

      //Emit cap nhat lai pageSize
      EventBus.$emit("updatePageSize", finalValue);
      //Convert myHTMLPaging thanh string de binding len HTML
      this.myHTMLPaging = finalValue.toString(); 
      // Emit cap nhat lai pageNumber khi thay doi so nhan vien/trang
      EventBus.$emit("updatePageNumberV2", 1);
    }
  },

  watch: {},
};
</script>

<style scoped>
</style>