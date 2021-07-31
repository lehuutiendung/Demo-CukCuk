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
    // props: ["api", "data", "type"],
    props: {
        api: {
            type: String,
            default: function () {
                return '';
            }
        },
        dataValue: {
            type: Array,
            default: function(){
                return [];
            }
        },
        type: {
            type: String,
            default: function(){
                return '';
            }
        }
    },
    data() {
        return {
            show: false,
            dropdownData: this.dataValue,
            typeName: this.type + 'Name',
        }
    },
    created() {
        let vm = this;
        if(!vm.dropdownData.length){
            axios.get(vm.api)
            .then(res => {
                vm.dropdownData = [];
                res.data.forEach(element => {
                    vm.dropdownData.push(element);
                });
                console.log(vm.dropdownData);
            })
            .catch(err => {
                console.error(err); 
            })
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