<template>
    <header v-show="isLoggedIn">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark scrolling-navbar fixed-top">
            <div class="container-fluid">
                <a class="navbar-brand" href="/">Geosat</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarResponsive">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item active">
                            <router-link to="/" class="nav-link">Home</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/counter" class="nav-link">Counter</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/fetchData" class="nav-link">Fetch Data</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/rtsp" class="nav-link">RTSP</router-link>
                        </li>
                        <li class="nav-item">
                            <button class="btn btn-link nav-link" v-on:click="logout">logout</button>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>
<script>
    import { onMounted, computed  } from "vue";
    import store from "../store";

    export default {

        setup() {
            const isLoggedIn = computed(() => store.getters.isLoggedIn);
            const logout = () => {
                console.log("logout");
                store.dispatch("logout");
            }

            onMounted(() => {

            });

            return {
                logout,
                isLoggedIn,
            };
        }
    }
</script>
