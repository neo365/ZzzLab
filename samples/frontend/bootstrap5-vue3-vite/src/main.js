import "./plugins/IEToEdge.js";

import { createApp } from 'vue'
import App from './App.vue'
const app = createApp(App);

import router from './router'
app.use(router)
import store from './store'
app.use(store)

console.log(import.meta.env.VITE_APP_BASE_URL);

import axios from 'axios' // [axios]
axios.defaults.baseURL = import.meta.env.VITE_APP_SERIVE_URL
app.config.globalProperties.$http = axios;

// Install fortawesome
import '@fortawesome/fontawesome-free/js/all.js'

// install bootstrap
import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";

import BootstrapVue3 from 'bootstrap-vue-3'
app.use(BootstrapVue3)
import 'bootstrap-vue-3/dist/bootstrap-vue-3.css'
import './styles/bootstrap/index.css'

// Install bootstrap icons
import "bootstrap-icons/font/bootstrap-icons.css";

// Install element plus
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './styles/element/index.css'
app.use(ElementPlus)

// Install element plus icons
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}

// event bus -> mitt
import mitt from 'mitt'
const emitter = mitt();
app.config.globalProperties.emitter = emitter

// install session
// import ZLSession from '@/modules/ZLSession'
// app.use(ZLSession,  { persist: true })

// install Cookies
import VueCookies from 'vue-cookies'
// default options config: { expires: '1d', path: '/', domain: '', secure: '', sameSite: 'Lax' }
app.use(VueCookies, {})

// import './plugins/SignalRHub.js'


import './plugins/Prototype.js'

//Install custom components
import ZBButton from './components/bootstrap5/ZBButton.vue'
import ZBRow from './components/bootstrap5/ZBRow.vue'
import ZBCol from './components/bootstrap5/ZBCol.vue'
import ZBDataTables from './components/bootstrap5/ZBDataTables.vue'
app.component(ZBButton.name, ZBButton)
app.component(ZBRow.name, ZBRow)
app.component(ZBCol.name, ZBCol)
app.component(ZBDataTables.name, ZBDataTables)

import Copyright from "./components/Copyright.vue";
app.component(Copyright.name, Copyright);

import './styles/index.css'

app.mount('#app')

