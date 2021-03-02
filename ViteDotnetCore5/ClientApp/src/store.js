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
                await axios.post("/api/auth/Login", model).then(result => {
                    if (result.data.success) {
                        commit("setToken", result.data.data);
                        router.push("/");
                    }
                    else {
                        commit("setError", "Authentication Failed");
                    }
                });

            } catch {
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