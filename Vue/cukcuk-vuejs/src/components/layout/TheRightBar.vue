<template>
    <div class="right-bar">
        <Header/>
        <Content/>
        <Footer/>
    </div>
</template>

<script>
import Header from './TheHeader.vue'
import Content from './TheContent.vue'
import Footer from './TheFooter.vue'
export default {
    name: 'TheRightBar',
    components: {
        Header,
        Content,
        Footer
    },
    methods: {
        
    },
}
</script>

<style>
    .right-bar{
        position: relative;
    }
</style>