<template>
  <div>
    <input
      class="col-1 autofocus required"
      v-if="required"
      v-on:blur="handleBlur($event)"
      v-on:focus="handleInput($event)"
      v-bind:value="value ? value : newEmployeeCode"
      type="text"
      autocomplete=""
      title="Trường bắt buộc phải nhập!"
      v-on="inputListeners"
      :tabindex="tabindex"
    />    
    
    <!-- Custom calendar -->
    <input
      class="calendar"
      :class="{DOB: DOB, identityDate: identity, joinDate: joinDate}"
      type="date"
      ref="datePicker"
      data-date-format="DD MMMM YYYY"
      title="Nhập đúng định dạng ngày/tháng/năm"
      v-if="isDate"
      v-bind:value="value"
      v-on="inputListeners"
      :tabindex="(tabindex) + 1"
    />
    <input
      :class="{'calendar-format': DOB, 'calendar-identity': identity, 'calendar-format calendar-joinDate' : joinDate}"
      type="text"
      placeholder="_ _/_ _/_ _ _ _"
      title="Nhập đúng định dạng ngày/tháng/năm"
      v-if="isDate"
      v-bind:value="[value ? formatDate(value) : '']"
      v-on="inputListeners"
    />  
    <input
      class="col-1 autofocus"
      type="text"
      autocomplete="on"
      v-if="normal"
      v-bind:value="value"
      v-on="inputListeners"
      v-on:focus="handleInput($event)"
      v-on:blur="handleBlurNormal($event)"
      :tabindex="tabindex"
    />
  </div>
</template>
<script>
export default {
    name: 'InputField',
    data() {
      return {
        borderRequired: false, //Border input required
      }
    },
    props: {
      value:{
        type: String,
        default(){
          return this.newEmployeeCode;
        }
      },
      required: {
        type: Boolean,
      },
      isDate:{
        type: Boolean,
      },
      DOB: {
        type: Boolean,
      },
      identity:{
        type: Boolean,
      },
      joinDate: {
        type: Boolean,
      },
      normal: {
        type: Boolean,
      },
      newEmployeeCode: {
        type: String,
        default(){
          return '';
        }
      },
      tabindex: {
        type: Number,
      }
    },
    mounted() {
      this.$emit('updateCode', this.newEmployeeCode);
      if(this.$refs.datePicker){
          this.$refs.datePicker.max = new Date().toISOString().split("T")[0];
      }
    },
    computed: {
      inputListeners: function () {
        var vm = this
        // `Object.assign` merges objects together to form a new object
        return Object.assign({},
          // We add all the listeners from the parent
          this.$listeners,
          // Then we can add custom listeners or override the
          // behavior of some listeners.
          {
            // This ensures that the component works with v-model
            input: function (event) {
               vm.$emit('input', event.target.value)
            }
          }
        )
      },
      
    },
    methods: {
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
       * Bắt sự kiện blur cho các trường input bình thường 
       */
      handleBlurNormal(e) {
        e.target.style.border = "1px solid #bbbbbb";
      },

      /**
       * Bắt lại sự kiện focus cho input
       */
      handleInput(e) {
        e.target.style.border = "1px solid #019160";
      },

      //Format dd/mm/yyyy
      formatDate(date){
        try{
            var rel = "";
            var word = date.split('-');
            for(var i = 0; i < 2;  i++){
                rel += word[2][i];
            }
            return rel+= '/' + word[1] + '/' + word[0];          
        }catch{
          console.log("Có lỗi xảy ra tại FormatDate!");
        }
      },
    },
  
};
</script>
<style scoped>
  @import "../../css/base/baseinput.css";
</style>