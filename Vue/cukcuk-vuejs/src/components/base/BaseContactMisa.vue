<template lang="">
    <div class="background-popup-contact" :style='{display: popUpState}'>
        <div class="popup">
        <div class="title-wrap">
            <div class="title-popup">
                <p>Thông báo</p>
            </div>
            <div class="exit-popup" @click="changeStatePopUp()">
                <i id="exit-popup" class="fas fa-times"></i>
            </div>
        </div>
        <div class="content-wrap">
            <div class="warning-popup">
                <i class="fas fa-exclamation"></i>
            </div>
            <div class="content-popup">
                <p>Có lỗi xảy ra vui lòng liên hệ <span>MISA</span>!</p>
            </div>
        </div>
        <div class="button-popup">
            <div class="btn-popup btn-close" @click="changeStatePopUp()">
                <p>Đóng</p>              
            </div>
        </div>
        </div>
    </div>
</template>
<script>
export default {
    name: 'TheContactPopUp',
    props: {
        contactPopUpShow: {
            type: Boolean,
            default(){
                return false;
            }
        }
    },
    
    computed:{
        popUpState(){
            if(this.contactPopUpShow){
                return 'block';
            }else{
                return 'none';
            }
        }
    },

    methods: {
        changeStatePopUp() {
            this.$emit('hideContactPopUp');
        }
    },
    
}
</script>
<style scoped>
    @import '../../css/base/basecontact.css';
</style>