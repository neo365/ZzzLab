import './plugins/IEToEdge'
import Vue from 'vue'
import './plugins/axios'
import App from './App.vue'
import router from './router'
import store from './store'
import './plugins/prototype'

// install Session
import VueSession from 'vue-session'

Vue.use(VueSession, {
  persist: true
})

// install Cookies
import VueCookies from "vue-cookies";
//쿠키를 사용한다.
Vue.use(VueCookies);
//Vue.$cookies.config("7d"); //쿠키의 만료일은 7일이다. (글로벌 세팅)

import vuetify from './plugins/vuetify'
// import MaterialDesignIcons from './plugins/material-design-icons'
// import MaterialIcons from './plugins/material-icons'
// import FontAwesomeVuetify from './plugins/font-awesome-vuetify'
// import '@fortawesome/fontawesome-free/js/all.js'

// portal-vue
import PortalVue from 'portal-vue'
Vue.use(PortalVue)

import {
  ValidationObserver,
  ValidationProvider,
  extend,
  localize
} from "vee-validate";
import ko from "vee-validate/dist/locale/ko.json";
import * as rules from "vee-validate/dist/rules";

// Install VeeValidate rules and localization
Object.keys(rules).forEach(rule => {
  extend(rule, rules[rule]);
});

localize("ko", ko);

// Install VeeValidate components globally
Vue.component("ValidationObserver", ValidationObserver);
Vue.component("ValidationProvider", ValidationProvider);


// Install Components
import Copyright from "./components/Copyright.vue";
import ZLDivider from "./components/ZLDivider.vue";
Vue.component(Copyright.name, Copyright);
Vue.component(ZLDivider.name, ZLDivider);
import ZlDatatable from "./components/ZLDataTable";
Vue.component(ZlDatatable.name, ZlDatatable);


// import printJS from 'print-js'
// Vue.use(printJS);


Vue.config.productionTip = false

new Vue({
  router,
  store,
  vuetify,
  // MaterialDesignIcons,
  // MaterialIcons,
  // FontAwesomeVuetify,
  created() {
    const userString = localStorage.getItem('user')
    if (userString) {
      const userData = JSON.parse(userString)
      this.$store.commit('SET_USER', userData)
    }

    const sidebarOpened = Vue.$cookies.get('sidebarOpened') === "true" || false;

    console.log('START_SIDEBAR_ONOFF:' + sidebarOpened + '/' + Vue.$cookies.get('sidebarOpened'));
    this.$store.commit('SET_SIDEBAR_ONOFF', sidebarOpened)
  },
  render: h => h(App)
}).$mount('#app')
