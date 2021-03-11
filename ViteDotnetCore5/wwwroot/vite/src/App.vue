<template>
    <nav-menu></nav-menu>
    <div class="container mt-5 pt-5">
        <router-view />
    </div>
</template>

<script>
    import NavMenu from './components/NavMenu.vue'

    export default {
        name: 'App',
        components: {
            NavMenu
        }
    }
</script>

<style>
</style>
