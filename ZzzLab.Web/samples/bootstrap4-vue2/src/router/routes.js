import DummyLayout from '@/views/Layout/DummyLayout.vue';
import BaseLayout from '@/views/Layout/DockLayout.vue';
import ErrorPageLayout from '@/views/ErrorPages/ErrorPageLayout.vue';

const routes = [
    {
        path: '/redirect',
        component: BaseLayout,
        hidden: true,
        children: [
            {
                path: '/redirect/:path*',
                component: () => import('@/views/Redirect'),
            },
        ],
    },
    {
        path: '/',
        redirect: 'Dashboard',
        component: BaseLayout,
        meta: {requiresAuth: true},
        children: [
            {
                path: '/Dashboard',
                name: 'Dashboard',
                meta: {requiresAuth: true},
                component: () => import('../views/Dashboard/Dashboard.vue')
            },
        ]
    },
    {
        path: '/',
        redirect: 'Login',
        component: DummyLayout,
        children: [
            {
                path: '/Login',
                name: 'Login',
                component: () => import('../views/Auth/Login.vue')
            },
            {
                path: '/Logout',
                name: 'Logout',
                component: () => import('../views/Auth/Logout.vue')
            },
        ]
    },
    {
        path: '/',
        component: ErrorPageLayout,
        children: [
            {
                path: '/:pathMatch(.*)*',
                name: 'NotFound',
                component: () => import('../views/ErrorPages/NotFoundPage.vue')
            },
        ]
    },
]

export default routes;