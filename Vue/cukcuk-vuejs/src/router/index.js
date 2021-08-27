import Vue from 'vue'
import VueRouter from 'vue-router'
import Employee from '../view/employee/EmployeeList.vue'
import Customer from '../view/customer/CustomerList.vue'

Vue.use(VueRouter)

const routers = [
    {path: '/dictionary/Employee', name: 'Employee', component: Employee},
    {path: '/dictionary/Customer', name: 'Customer', component: Customer}

];

const router = new VueRouter({
    mode: 'history',
    routes: routers
});

export default router
