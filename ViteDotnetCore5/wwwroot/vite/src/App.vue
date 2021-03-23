<template>
    <nav-menu></nav-menu>

    <div class="container">
        <div class="row">
            <router-view />
        </div>
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
