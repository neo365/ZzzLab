import DummyLayout from '../views/Layout/DummyLayout.vue';
import DockLayout from '../views/Layout/DockLayout.vue';
import ErrorPageLayout from '../views/ErrorPages/ErrorPageLayout.vue';

const routes = [
    {
        path: '/',
        redirect: 'Dashboard',
        component: DockLayout,
        meta: {requiresAuth: true},
        children: [
            {
                path: '/Dashboard',
                name: 'Dashboard',
                meta: {requiresAuth: false},
                component: () => import('../views/Pages/Dashboard.vue')
            },
            {
                path: '/samples/buttons',
                name: 'buttons',
                meta: {requiresAuth: false},
                component: () => import('../views/Samples/Buttons.vue')
            },
            {
                path: '/samples/datatables',
                name: 'datatables',
                meta: {requiresAuth: false},
                component: () => import('../views/Samples/DataTables.vue')
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
        path: '/redirect',
        component: DockLayout,
        hidden: true,
        children: [
            {
                path: '/redirect/:path*',
                component: () => import('../views/redirect.vue'),
            },
        ],
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