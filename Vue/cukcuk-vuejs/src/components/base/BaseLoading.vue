<template lang="">
    <div class="wrap-loading" :class="{'hide-loading': hideLoading}">
        <div class="lds-ring"><div></div><div></div><div></div><div></div></div>
    </div>
</template>
<script>
export default {
    name: 'TheLoading',
    props: {
        hideLoading: {
            typeo: Boolean,
            default(){
                return false;
            }
        }
    }
}
</script>
<style scoped>
    @import "../../css/base/baseloading.css";
</style>