<template>
  <div>
      <TheWrap/>
      <!-- <ModalBox v-bind:modalBoxShow="modalBoxShow" v-on:hideModalBox="hideModalBox()"/> -->
  </div>
</template>

<script>
import TheWrap from './components/layout/TheWrap.vue'
export default {
  name: 'App',
  components: {
    TheWrap,
  
  },
  data() {
    return {
      
    }
  },
  methods: {
    
  },
}
</script>

<style>
  @import '../src/css/main.css';
  @import '../src/css/layout/popup/cancel.css';
</style>
