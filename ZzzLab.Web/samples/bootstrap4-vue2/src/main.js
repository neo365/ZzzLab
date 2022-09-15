import './plugins/IEToEdge'
import Vue from 'vue'
import './plugins/axios'
import App from './App.vue'
import router from './router'
import store from './store'
import cookie from './plugins/cookie'
import './plugins/prototype'
// Install i18n
import i18n from './plugins/i18n'

// Install fortawesome
import '@fortawesome/fontawesome-free/js/all.js'

// Install Bootstrap
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

// Install Element UI
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import locale from 'element-ui/lib/locale/lang/ko'
Vue.use(ElementUI, { locale })

// Install portal-vue
import PortalVue from 'portal-vue'
Vue.use(PortalVue)

//Install vee-validate
import {extend, localize, ValidationObserver, ValidationProvider} from "vee-validate";
import ko from "vee-validate/dist/locale/ko.json";
import * as rules from "vee-validate/dist/rules";
// Install VeeValidate components globally
Vue.component("ValidationObserver", ValidationObserver);
Vue.component("ValidationProvider", ValidationProvider);

// Install VeeValidate rules and localization
Object.keys(rules).forEach(rule => {
    extend(rule, rules[rule]);
});

localize("ko", ko);



// Install vue session
import VueSession from 'vue-session'

var sessionOptions = {
    persist: true
}
Vue.use(VueSession, sessionOptions)

/** import custom vue component **/
import ZLDivider from '@/components/ZLDivider';

/** Global Custom Component **/
Vue.component(ZLDivider.name, ZLDivider);

//JQeury 사용하는곳에서 $를 쓰면 warning 발생 해결
global.jQuery = require('jquery');
var $ = global.jQuery;
window.$ = $;

Vue.config.productionTip = false

new Vue({
    router,
    store,
    cookie,
    i18n,
    created() {
        // 새로고침시 자동로그인
        const userString = localStorage.getItem('user')
        if (userString) {
            const userData = JSON.parse(userString)
            this.$store.commit('SET_USER', userData)
        }
    },
    render: h => h(App)
}).$mount('#app')
