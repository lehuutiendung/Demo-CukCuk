<template>
    <div class="wrap">
        <Menu/>
        <TheRightBar/>
    </div>
</template>

<script>
import Menu from './TheMenu.vue'
import TheRightBar from './TheRightBar.vue'
export default {
    name: 'TheWrap',
    components: {
        Menu,
        TheRightBar,
    },
    methods: {

    },
}
</script>

<style scoped>

</style>