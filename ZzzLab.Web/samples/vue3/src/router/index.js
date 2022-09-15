import { createRouter, createWebHistory } from 'vue-router'
import routes from './routes';


const router = createRouter({
  history: createWebHistory(),
  routes: routes,
  linkActiveClass: 'active',
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    const loggedIn = localStorage.getItem('user');

    // 이 라우트는 인증이 필요하며 로그인 한 경우 확인하십시오.
    // 그렇지 않은 경우 로그인 페이지로 리디렉션하십시오.
    if (to.meta.requiresAuth && !loggedIn) {
      next({
        path: '/login',
        query: {redirect: to.fullPath}
      })
    } else {
      next()
    }
  } else {
    next() // 반드시 next()를 호출하십시오!
  }
})

export default router
