<template lang="">
    <!-- Popup cảnh báo hủy form thêm mới-->
  <div class="background-popup-add" :style='{display: popUpState}'>
    <div class="popup">
      <div class="title-wrap">
          <div class="title-popup">
              <p>Hủy thao tác thêm mới</p>
          </div>
          <div class="exit-popup" @click="changeStatePopUp(1)">
              <i id="exit-popup" class="fas fa-times"></i>
          </div>
      </div>
      <div class="content-wrap">
          <div class="warning">
              <i class="fas fa-exclamation-triangle"></i>
          </div>
          <div class="content-popup">
              <p>Bạn có chắc muốn đóng <span>"Thêm mới nhân viên"</span> hay không?</p>
          </div>
      </div>
      <div class="button-popup">
          <div class="btn-popup btn-continue" @click="changeStatePopUp(1)">
              <p>Tiếp tục nhập</p>
          </div>
          <div class="btn-popup btn-close" @click="changeStatePopUp(0)">
              <p>Đóng</p>              
          </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
    name: 'PopUp',
    data() {
        return {
            isShowPopUp: false,
        }
    },
    props: {
        popUpShow:{
            type: Boolean,
        }
    },
    computed:{
        popUpState(){
            if(this.popUpShow){
                return 'block';
            }else{
                return 'none';
            }
        }
    },
    methods: {
        changeStatePopUp(value){
            this.$emit('hidePopUp',value);
        }
    },
}
</script>
<style lang="">
    
</style>