<template>
    <div v-if="isLoggedIn">
        <div class="container">
            <a href="#" data-target="slide-out" class="top-nav sidenav-trigger waves-effect waves-light circle hide-on-large-only">
                <i class="material-icons">menu</i>
            </a>
        </div>
        <ul id="slide-out" class="sidenav sidenav-fixed indigo lighten-5" style="overflow: auto; transform: translateX(0%);">
            <li>
                <a href="/">
                    <i class="material-icons left">account_circle</i>
                    Geosat
                </a>
            </li>

                <li class="nav-item">
                    <router-link to="/">Home</router-link>
                </li>
                <li class="nav-item">
                    <router-link to="/counter">Counter</router-link>
                </li>
                <li class="nav-item">
                    <router-link to="/fetchData">Fetch Data</router-link>
                </li>
                <li class="nav-item">
                    <router-link to="/rtsp">RTSP</router-link>
                </li>
                <li class="nav-item">
                    <button class="btn btn-link indigo" v-on:click="logout">logout</button>
                </li>
        </ul>

    </div>
</template>
<script>
    import { onMounted, computed } from "vue";
    import store from "../store";

    export default {

        setup() {
            const isLoggedIn = computed(() => store.getters.isLoggedIn);
            const logout = () => {
                console.log("logout");
                store.dispatch("logout");
            }

            onMounted(() => {
                document.addEventListener('DOMContentLoaded', function () {
                    var elems = document.querySelectorAll('.sidenav');
                    M.Sidenav.init(elems, {
                        inDuration: 300,
                        outDuration: 300,
                    });
                });


            });

            return {
                logout,
                isLoggedIn,
            };
        }
    }
</script>
