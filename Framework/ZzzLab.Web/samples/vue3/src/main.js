import { createApp } from 'vue'
import App from './App.vue'

const app = createApp(App);

import router from './router'
app.use(router)
import store from './store'
app.use(store)

import axios from 'axios' // [axios]
axios.defaults.baseURL = 'http://localhost:8888'
app.config.globalProperties.$http = axios;

// install session
import ZLSession from './modules/ZLSession'
app.use(ZLSession,  { persist: true })

// install Cookies
import VueCookies from 'vue-cookies'
// default options config: { expires: '1d', path: '/', domain: '', secure: '', sameSite: 'Lax' }
app.use(VueCookies, {})

// Install fortawesome
import '@fortawesome/fontawesome-free/js/all.js'


import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './styles/element/index.css'
app.use(ElementPlus)

import * as ElementPlusIconsVue from '@element-plus/icons-vue'
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}

import Copyright from "./components/Copyright.vue";
// eslint-disable-next-line vue/multi-word-component-names
app.component(Copyright.name, Copyright);

import ZLDivider from "./components/ZLDivider.vue";
// eslint-disable-next-line vue/multi-word-component-names
app.component(ZLDivider.name, ZLDivider);



app.mount('#app')
