import DummyLayout from '@/views/Layout/DummyLayout.vue';
import DockLayout from '@/views/Layout/DockLayout.vue';
import ErrorPageLayout from '@/views/ErrorPages/ErrorPageLayout.vue';

const routes = [
    {
        path: '/redirect',
        component: DockLayout,
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
        component: DockLayout,
        meta: {requiresAuth: true},
        children: [
            {
                path: '/Dashboard',
                name: 'Dashboard',
                component: () => import('../views/Dashboard/Dashboard.vue')
            },
            {
                path: '/Debug/Database/TableList',
                name: 'TableList',
                component: () => import('../views/Debug/DataBase/TableList.vue')
            },
            {
                path: '/Settings/Database/Config/List',
                name: 'DataBaseConfigList',
                component: () => import('../views/Settings/Database/ConfigList.vue')
            },
            {
                path: '/Settings/Database/Query/List',
                name: 'QueryList',
                component: () => import('../views/Settings/Database/QueryList.vue')
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