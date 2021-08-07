<template>
  <div class="wrap-dropdown">
    <div class="dropdown dropdown--modal" @click="showDataDropdown()">
      <div class="dropdown-title dropdown-title2" :filter="type">
        <!-- mode: 0 - Thêm, 1 - Sửa -->
        <p v-if="typeName == 'GenderName'">{{ dataDropdown.GenderName }}</p>
        <p v-if="typeName == 'DepartmentName'">{{ dataDropdown.DepartmentName }}</p>
        <p v-if="typeName == 'PositionName'">{{ dataDropdown.PositionName }}</p>
      </div>
      <span class="dropdown-img">
        <i class="fas fa-angle-down"></i>
      </span>
    </div>
    <div class="dropdown-data dropdown-data2" v-bind:class="{ show: show }">
      <div
        class="dropdown-data-item"
        :class="{ active: itemClicked == index ? true : false }"
        v-for="(item, index) in dropdownData"
        :key="index"
        @click="getValueDropdown(index, item[typeName], item[typeId])"
      >
        <div class="tick-item">
          <i
            class="fas fa-check"
            v-show="itemClicked == index ? true : false"
          ></i>
        </div>
        <span>{{ item[typeName] }}</span>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "TheDropdown",
  // props: ["api", "data", "type"],
  props: {
    api: {
      type: String,
      default: function () {
        return "";
      },
    },
    dataValue: {
      type: Array,
      default: function () {
        return [];
      },
    },
    type: {
      type: String,
      default: function () {
        return "";
      },
    },
    dataDropdown:{
        type: Object,
        required: false,
        default(){
          return {}
        }
    },
    callApi: {
        type: Boolean,
        required: false,
    },
    mode: {
        type: Number,
        default: 0,
    },
  },
  data() {
    return {
      show: false,
      dropdownData: this.dataValue,
      typeName: this.type + "Name",
      typeId: this.type + "Id",
      itemClicked: 0,
      itemName: [],
    };
  },
  created() {
    let vm = this;
    if (!vm.dropdownData.length) {
      axios
        .get(vm.api)
        .then((res) => {
          vm.dropdownData = [];
          res.data.forEach((element) => {
            vm.dropdownData.push(element);
          });
          vm.itemName = vm.dropdownData[0][vm.typeName];
          vm.$emit("gender", vm.itemName);
        })
        .catch((err) => {
          console.error(err);
        });
    } else {
      vm.itemName = vm.dataValue[0][vm.typeName];
    }
  },

  methods: {
    /**
     * @description Hiên thị data dropdown
     * @author DUNGLHT
     * @since 29/07/2021
     */
    showDataDropdown() {
      this.show = !this.show;
    },

    getValueDropdown(index, value, id) {
      this.itemName = value;
      this.itemClicked = index;
      this.$emit("gender", id, value);
      this.$emit("department", id, value);
      this.$emit("position", id, value);
      this.show = !this.show;
    },
  },
};
</script>

<style>

</style>