<template>
  <div>
      <TheWrap v-on:modalBoxUpdated="modalBoxUpdated()"/>
      <ModalBox v-bind:modalBoxShow="modalBoxShow"/>
  </div>
  
</template>

<script>
import TheWrap from './components/layout/TheWrap.vue'
import ModalBox from './components/layout/TheModalBox.vue'
export default {
  name: 'App',
  components: {
    TheWrap,
    ModalBox
  },
  data() {
    return {
      modalBoxShow: false,
    }
  },
  methods: {
    modalBoxUpdated(){
      console.log('emited to App');
      this.modalBoxShow = true;
    }
  },
}
</script>

<style>
  @import '../src/css/main.css';
  @import '../src/css/layout/popup/cancel.css';
</style>
