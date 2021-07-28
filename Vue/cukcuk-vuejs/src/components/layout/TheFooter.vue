<template>
    <div class="footer">
        <div class="total__employees">
          <!-- <p style="color: #5D5C5C;">Hiển thị <span style="color: #000000; font-weight: bold;">1-10/1000</span> nhân viên</p> -->
        <p class="total__employees--change" style="color: #5D5C5C;">Hiển thị <span style="color: #000000; font-weight: bold;">1-10/400</span> nhân viên</p></div>
        <div class="number__pages">
          <div class="button__hide button__footer__start">
            <img src="../../assets/icon/btn-firstpage.svg" alt="">
          </div>
          <div class="button__hide button__footer__prev">
            <img src="../../assets/icon/btn-prev-page.svg" alt="">
          </div>
          <div class="number__pages--bind"> 
              <!-- <div class="button__dot focus">
                <p style="color: #ffffff;">1</p>
              </div>
              <div class="button__dot">
                <p>2</p>
              </div>
              <div class="button__dot">
                <p>3</p>
              </div>
              <div class="button__dot">
                <p>4</p>
              </div> -->
          <div class="button__dot focus" number-page="1">
                                <p>1</p>
                            </div><div class="button__dot" number-page="2">
                                <p>2</p>
                            </div><div class="button__dot" number-page="3">
                                <p>3</p>
                            </div><div class="button__dot" number-page="4">
                                <p>4</p>
                            </div><div class="button__dot" number-page="5">
                                <p>5</p>
                            </div><div class="button__dot" number-page="6">
                                <p>6</p>
                            </div><div class="button__dot" number-page="7">
                                <p>7</p>
                            </div><div class="button__dot" number-page="8">
                                <p>8</p>
                            </div><div class="button__dot" number-page="9">
                                <p>9</p>
                            </div><div class="button__dot" number-page="10">
                                <p>10</p>
                            </div></div>
          
          <div class="button__hide button__footer__next">
            <img src="../../assets/icon/btn-next-page.svg" alt="">
          </div>
          <div class="button__hide button__footer__end">
            <img src="../../assets/icon/btn-lastpage.svg" alt="">
          </div>
          
          
        </div>
        <div class="note">
          <div class="note-number-emmployees">
            <p style="color: #5D5C5C;"><span style="color: #000000; font-weight: bold;">10</span> nhân viên/trang</p>
          </div>
          <div class="up-down-number">
            <i class="fas fa-chevron-up"></i>
            <i class="fas fa-chevron-down"></i>
          </div>
        </div>
        <div class="paging-data">
            <div class="paging-item" page-id="4"><span>Tất cả</span> nhân viên</div>
            <div class="paging-item" page-id="3"><span>40</span> nhân viên/trang</div>
            <div class="paging-item" page-id="2"><span>20</span> nhân viên/trang</div>
            <div class="paging-item" page-id="1"><span>10</span> nhân viên/trang</div>
        </div>
      </div>
</template>

<script>
export default {
    name: 'TheFooter'
}
</script>

<style scoped>

</style>