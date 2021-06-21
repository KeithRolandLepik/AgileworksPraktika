import Home from '../views/Home.vue';
import Add from '../views/Add.vue';
import Authenticate from '../views/Authenticate.vue';
import { createRouter, createWebHistory } from 'vue-router';

const authGuard = (to, from, next) => {
  if (localStorage.getItem("user")) {
    next();
  } else {
    next("/login")
  }
};

const routes = [
  {
    path: '/',
    name: 'Home',
    beforeEnter: authGuard,
    component: Home,
  },
  {
    path: '/add',
    name: 'Add',
    beforeEnter: authGuard,
    component: Add,
  },  
  {
    path: '/login',
    name: 'Authenticate',
    component: Authenticate,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
