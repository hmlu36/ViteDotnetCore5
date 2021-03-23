import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './index.css'
import store from './store'
import VueSweetalert2 from 'vue-sweetalert2';

/*
const options = {
    confirmButtonColor: '#41b882',
    cancelButtonColor: '#ff7674',
};

Vue.use(VueSweetalert2, options);
*/
createApp(App)
    .use(router)
    .use(store)
    .use(VueSweetalert2)
    .mount('#app')