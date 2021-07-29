<template>
    <div class="wrap-dropdown">
        <div class="dropdown dropdown--modal" @click="showDataDropdown()">
            <div class="dropdown-title dropdown-title2" :filter="type">
                <p>Chọn vị trí</p>
            </div>
            <span class="dropdown-img">
                <i class="fas fa-angle-down"></i>
            </span>
        </div>
        <div class="dropdown-data dropdown-data2" v-bind:class="{'show' : show }">
            <div class="dropdown-data-item" v-for="item in dropdownData" :key="item[typeName]">
                <div class="tick-item">
                    <i class="fas fa-check"></i>
                </div>
                <span>{{ item[typeName] }}</span>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";


export default {
    name: 'TheDropdown',
    created() {
        axios.get(this.api)
        .then(res => {
            this.dropdownData = [];
            res.data.forEach(element => {
                this.dropdownData.push(element);
            });
            console.log(this.dropdownData);
        })
        .catch(err => {
            console.error(err); 
        })
    },
    props: ["api", "data", "type"],
    data() {
        return {
            show: false,
            dropdownData: this.data,
            typeName: this.type + 'Name',
        }
    },
    methods: {
        showDataDropdown(){
            this.show = !this.show;
        }
    },
};
</script>

<style>

</style>