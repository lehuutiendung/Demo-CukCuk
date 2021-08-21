<template>
  <div>
    <div
      class="wrap-combobox combobox-room"
      data-id="1"
      v-on:click="showDataCombobox"
    >
      <input
        type="text"
        class="combobox"
        :placeholder="itemComboboxName"
        :value="itemComboboxName"
        v-bind:filter="type"
        v-if="mode == 1"
      />
      <div class="combobox-delete-text">
        <i class="far fa-times-circle"></i>
      </div>
      <div class="combobox-btn">
        <button>
          <div class="combobox-img">
            <i class="fas fa-angle-down combobox-icon"></i>
          </div>
        </button>
      </div>
    </div>
    <ul
      class="combobox-data"
      v-bind:class="{ 'combobox-data-active': isActive }"
    >
      <!-- Binding data here! -->
      <li
        class="combobox-data-item"
        :class="{ 'item-active': itemClicked == index ? true : false }"
        v-for="(item, index) in items"
        :key="item[typeName]"
        @click="getValueCombobox(index, item[typeName], item[typeId], typeName)"
      >
        <div class="tick-item">
          <i
            class="fas fa-check"
            v-show="itemClicked == index ? true : false"
          ></i>
        </div>
        <span>{{ item[typeName] }}</span>
      </li>
    </ul>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Combobox",
  // type: "Position", "Department"
  // api: url api
  // mode: 1 - List hiển thị có "tất cả phòng ban, nhân viên"
  // data: truyền vào data nếu có data fix cứng
  // props: ["type", "api", "data", "mode"],
  props: {
    type: {
      type: String,
    },
    api: {
      type: String,
    },
    data: {
      type: Array,
    },
    mode: {
      type: Number,
    },
    department: {
        type: String,
        default(){
            return '';
        }
    },
    position: {
        type: String,
        default(){
            return '';
        }
    }

  },
  data() {
    return {
      isActive: false, //biến kiểm soát show data-dropdown
      value: "",
      isDataLoaded: false,
      typeName: this.type + "Name",
      typeId: this.type + "Id",
      items: this.data,
      map: {
        Position: "vị trí",
        Department: "phòng ban",
      },
      itemComboboxName: "",
      itemClicked: 0,
    };
  },
  // type: Department, Position
  created() {
    axios
      .get(this.api)
      .then((res) => {
        this.items = [];
        if (this.mode == 1) {
          this.items.push({
            [this.typeName]: "Tất cả " + this.map[this.type],
            [this.typeId]: ""
          });
        }
        res.data.forEach((element) => {
          this.items.push(element);
        });
        this.isDataLoaded = true;
        this.itemComboboxName = this.items[0][this.typeName];
      })
      .catch((err) => {
        console.error(err);
      });
  },
  methods: {
    showDataCombobox() {
      this.isActive = !this.isActive;
    },
    getValueCombobox(current, name, id, typeName) {  
      this.itemClicked = current;
      this.itemComboboxName = name;
      this.$emit(typeName, name, id);
      this.isActive = !this.isActive;
    },
  },
};
</script>

<style scoped>
</style>
