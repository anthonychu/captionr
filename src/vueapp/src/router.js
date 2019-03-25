import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/host',
      name: 'host',
      component: () => import('./views/CaptionHost.vue')
    },
    {
      path: '/join',
      name: 'join',
      component: () => import('./views/CaptionJoin.vue'),
      props: true
    }
  ]
})
