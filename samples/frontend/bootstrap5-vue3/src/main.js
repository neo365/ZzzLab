import { createApp } from 'vue'
import App from './App.vue'
const app = createApp(App);

import router from './router'
app.use(router)
import store from './store'
app.use(store)

import axios from 'axios' // [axios]
axios.defaults.baseURL = 'http://localhost:8089'
app.config.globalProperties.$http = axios;

// Install fortawesome
import '@fortawesome/fontawesome-free/js/all.js'

// install bootstrap
import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
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


// import DataTable from 'datatables.net-vue3'
// app.use(DataTable)
//import DataTableBs5 from 'datatables.net-bs5'
//DataTable.use(DataTableBs5);

import 'datatables.net-bs5/css/dataTables.bootstrap5.min.css';

// Install custom components
import ZBButton from './components/bootstrap5/ZBButton'
import ZBRow from './components/bootstrap5/ZBRow'
import ZBCol from './components/bootstrap5/ZBCol'
app.component(ZBButton.name, ZBButton)
app.component(ZBRow.name, ZBRow)
app.component(ZBCol.name, ZBCol)

import './styles/index.css'

// event bus -> mitt
import mitt from 'mitt'

const emitter = mitt();
app.config.globalProperties.emitter = emitter

app.mount('#app')
