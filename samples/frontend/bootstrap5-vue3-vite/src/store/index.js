import { createStore } from 'vuex'
import getters from './getters'

// For Vite
const modulesFiles = import.meta.glob('./modules/*.js')
const modules = {}
for (const path in modulesFiles) {
  modulesFiles[path]().then((mod) => {
    let moduleName = path.replace(/^\.\/(.*)\.\w+$/, '$1').replace('modules/', '')
    modules[moduleName] = mod.default;
  })
}

// for Vue-cli
// const modulesFiles = require.context('./modules', true, /\.js$/)
// const modules = modulesFiles.reduce((modules, modulePath) => {
//   // set './app.js' => 'app'
//   const moduleName = modulePath.replace(/^\.\/(.*)\.\w+$/, '$1')
//   const value = modulesFiles(modulePath)
//   modules[moduleName] = value.default
//   return modules
// }, {})

export default createStore({
  getters,
  modules
})
