

import '@mdi/font/css/materialdesignicons.css' // Ensure you are using css-loader
import 'material-design-icons-iconfont/dist/material-design-icons.css' // Ensure you are using css-loader
import '@fortawesome/fontawesome-free/css/all.css' // Ensure you are using css-loader
import 'vuetify/dist/vuetify.min.css'

import Vue from 'vue'
import Vuetify from 'vuetify/lib'


Vue.use(Vuetify);

const options = {
    icons: {
        iconfont: 'mdi' || 'md' || 'fa',
    },
}

export default new Vuetify(options);
