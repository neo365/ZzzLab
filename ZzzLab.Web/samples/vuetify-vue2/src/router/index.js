import Vue from 'vue'
import VueRouter from 'vue-router'
import routes from './routes';

Vue.use(VueRouter)


const router = new VueRouter({
  mode: 'history',
  routes,
  linkActiveClass: 'active',
  scrollBehavior: () => ({
    y: 0,
  }),
});

router.beforeEach((to, from, next) => {
  const loggedIn = localStorage.getItem('user');
  if (to.matched.some(record => record.meta.requiresAuth)) {
    // 이 라우트는 인증이 필요하며 로그인 한 경우 확인하십시오.
    // 그렇지 않은 경우 로그인 페이지로 리디렉션하십시오.

    if (!loggedIn) {
      console.log('loggedIn');
      next({
        path: '/login',
        query: { redirect: to.fullPath }
      })
    } else {

      next()
    }
  } else {
    next() // 반드시 next()를 호출하십시오!
  }
})


export default router
