import DummyLayout from '@/views/Layout/DummyLayout.vue';
import DockLayout from '@/views/Layout/DockLayout.vue';
import ErrorPageLayout from '@/views/ErrorPages/ErrorPageLayout.vue';

const routes = [
    // {
    //     path: '/',
    //     redirect: 'Debug/axios',
    //     component: DummyLayout,
    //     meta: {requiresAuth: true},
    //     children: [
    //         {
    //             path: '/Debug/Axios',
    //             name: 'axios',
    //             meta: {requiresAuth: false},
    //             component: () => import('@/views/Debug/Axios.vue')
    //         },
    //         {
    //             path: '/Debug/SignalR',
    //             name: 'SignalR',
    //             meta: {requiresAuth: false},
    //             component: () => import('@/views/Debug/SignalR.vue')
    //         },
    //         {
    //             path: '/Debug/Session',
    //             name: 'Session',
    //             meta: {requiresAuth: false},
    //             component: () => import('@/views/Debug/Session.vue')
    //         },
    //     ]
    // },
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
                component: () => import('@/views/Dashboard.vue')
            },
            {
                path: '/samples/buttons',
                name: 'buttons',
                meta: {requiresAuth: false},
                component: () => import('@/views/Samples/Buttons.vue')
            },
            {
                path: '/samples/datatables',
                name: 'datatables',
                meta: {requiresAuth: false},
                component: () => import('@/views/Samples/DataTables.vue')
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
                component: () => import('@/views/ErrorPages/NotFoundPage.vue')
            },
        ]
    },
]

export default routes;