<template>
    <div>
        <div class="wrap-combobox combobox-room" data-id="1" v-on:click="showDataCombobox">
            <input type="text" class="combobox" placeholder="Chọn/Nhập phòng ban" v-bind:filter="type" v-if="mode==1">
            <div class="combobox-delete-text"><i class="far fa-times-circle"></i></div>
            <div class="combobox-btn">
                <button>
                    <div class="combobox-img">
                        <i class="fas fa-angle-down combobox-icon"></i>
                    </div>
                </button>
            </div>
        </div>
        <ul class="combobox-data" v-bind:class="{ 'combobox-data-active': isActive }">
            <!-- Binding data here! -->
            <li class="combobox-data-item" v-for="item in items" :key="item[typeName]">
                <div class="tick-item">
                    <!-- <i class="fas fa-check"></i> -->
                </div>
                <span >{{ item[typeName] }}</span>
            </li>
        </ul>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: 'Combobox',
    // type: "Position", "Department"
    // api: url api 
    // mode: 1 - List hiển thị có "tất cả phòng ban, nhân viên"
    // data: truyền vào data nếu có data fix cứng
    props: ["type", "api", "data", "mode"],
    data() {
        return {
            isActive: false,    //biến kiểm soát show data-dropdown
            value: "",
            current: 0,
            isDataLoaded: false,
            typeName: this.type + "Name",
            items: this.data,
            map: {
                Position: "vị trí",
                Department: "phòng ban",
            },
        }
    },
    // type: Department, Position
    created() {
        axios.get(this.api)
        .then(res => {
            this.items = [];
            if(this.mode == 1){
                this.items.push({
                    [this.typeName]:"Tất cả " + this.map[this.type]
                })
            }
            res.data.forEach(element => {
                this.items.push(element);
            });
            this.isDataLoaded = true;
        })
        .catch(err => {
            console.error(err); 
        })
    },
    methods: {
        showDataCombobox(){
            this.isActive = !this.isActive;
        }
    },
    
}
</script>

<style scoped>

</style>
