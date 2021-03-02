import { createWebHistory, createRouter } from "vue-router";
import store from "./store";

const routes = [
    {
        path: "/",
        component: () => import("./components/Home.vue"),
    },
    {
        path: "/login",
        component: () => import("./components/account/Login.vue"),
    },
    {
        path: "/forgotPassword",
        component: () => import("./components/account/ForgotPassword.vue"),
    },
    {
        path: "/resetPassword",
        component: () => import("./components/account/ResetPassword.vue"),
    },
    {
        path: "/counter",
        component: () => import("./components/Counter.vue"),
    },
    {
        path: "/rtsp",
        component: () => import("./components/Rtsp.vue"),
    },
    {
        path: "/fetchData",
        component: () => import("./components/FetchData.vue"),
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    const isLogin = store.getters.isLoggedIn;
    if (to.path == '/login') {
        next();
    } else {
        isLogin ? next() : next('/login');
    }
});

export default router;