import { createStore } from 'vuex'
import axios from "./modules/axios-module";
import router from "./router";
import createPersistedState from 'vuex-persistedstate';
import SecureLS from "secure-ls";
const ls = new SecureLS({ isCompression: false });

const initUserStore = () => {
    return {
        accessToken: "",
        refreshToken: "",
        error: ""
    }
}

export default createStore({
    plugins: [
        createPersistedState({
            storage: window.sessionStorage,
            getItem: key => ls.get(key),
            setItem: (key, value) => ls.set(key, value),
            removeItem: key => ls.remove(key)
        })
    ],
    state: initUserStore(),
    mutations: {
        setError: (state, error) => state.error = error,
        clearError: (state) => state.error = "",
        setToken: (state, token) => {
            //console.log(JSON.stringify(token));
            state.accessToken = token.accessToken;
            state.refreshToken = token.refreshToken;
        },
        clearToken: (state) => {
            Object.assign(state, initUserStore());
        },
    },
    getters: {
        isLoggedIn: (state) => !!state.accessToken,
        getRefreshToken: (state) => state.refreshToken,
        getAccessToken: (state) => state.accessToken,
        //isAuthenticated: (state) => state.token.length > 0 && state.expiration > Date.now()
    },
    actions: {
        login: async ({ commit }, model) => {
            try {
                commit("clearError");
                console.log("grecaptcha");

                grecaptcha.ready(() => {

                    //console.log('1. grecaptcha.ready');
                    //console.log("2. 6Lfq3HsaAAAAAAJd-q-NUR1enFjEprK4JkhrGioT', { action: 'login' }");

                    grecaptcha.execute('6Lfq3HsaAAAAAAJd-q-NUR1enFjEprK4JkhrGioT', { action: 'login' }).then((token) => {

                        //console.log('3. Get token from reCAPTCHA service => ', token);
                        //console.log('4. Verifying Bot...');

                        axios.post("/api/auth/Verify/?token=" + token).then(result => {
                            //console.log(JSON.stringify(result.data));
                            if (result.data.success) {
                                axios.post("/api/auth/Login", model).then(response => {
                                    if (response.data.success) {
                                        commit("setToken", response.data.data);
                                        router.push("/");
                                    }
                                    else {
                                        commit("setError", "Authentication Failed");
                                    }
                                });
                            }
                            else {
                                this.$swal(result.data.message);
                                //router.push("/");
                            }
                        });
                    });
                });
            } catch (ex) {
                console.log(JSON.stringify(ex));
                console.log("fail to login");
                commit("setError", "Failed to login");
            }
        },
        updateToken({ commit }, token) {
            // Update token in store's state
            commit('setToken', token)
        },
        logout: ({ commit }) => {
            commit("clearToken");
            router.push("/login");
        }
    }
})