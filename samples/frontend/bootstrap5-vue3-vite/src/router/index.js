//import { createRouter, createWebHashHistory } from 'vue-router'
import { createRouter, createWebHistory } from 'vue-router'
import routes from './routes';


const createCustomRouter = () => createRouter({
  // mode: 'history', // require service support
  history: createWebHistory(),
  scrollBehavior: () => ({ y: 0 }),
  routes: routes,
  linkActiveClass: 'active',
})

const router = createCustomRouter()
// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createCustomRouter()
  router.matcher = newRouter.matcher // reset router
}


// router.beforeEach((to, from, next) => {
//   if (to.matched.some(record => record.meta.requiresAuth)) {
//     const loggedIn = localStorage.getItem('user');
//
//     // 이 라우트는 인증이 필요하며 로그인 한 경우 확인하십시오.
//     // 그렇지 않은 경우 로그인 페이지로 리디렉션하십시오.
//     if (to.meta.requiresAuth && !loggedIn) {
//       next({
//         path: '/login',
//         query: {redirect: to.fullPath}
//       })
//     } else {
//       next()
//     }
//   } else {
//     next() // 반드시 next()를 호출하십시오!
//   }
// })

export default router
