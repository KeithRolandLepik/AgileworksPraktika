import Home from '../views/Home.vue';
import Add from '../views/Add.vue';
import { createRouter, createWebHistory } from 'vue-router';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/add',
    name: 'Add',
    component: Add,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
