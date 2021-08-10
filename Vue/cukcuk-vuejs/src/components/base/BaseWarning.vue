<template lang="">
    <div class="warning" :class="{'right-warning': rightWarning}" :style="{ display: 'flex'}">
        <div class="warning-rotate"></div>
        <div class="warning-box">
                <p>Trường này bắt buộc phải nhập!</p>
        </div>
    </div>  
</template>
<script>
export default {
    name: 'TheWarning',
    props: {
        rightWarning: {
            type: Boolean,
        },
    }
}
</script>
<style scoped>
    @import "../../css/base/warning.css"
</style>