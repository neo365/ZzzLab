import DummyLayout from '../Layout/DummyLayout.vue';
import DockLayout from '../Layout/DockLayout.vue';
import ErrorPageLayout from '../Layout/ErrorPageLayout.vue';

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
            }
        ]
    },
    {
        path: '/',
        component: DockLayout,
        meta: {requiresAuth: true},
        children: [
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
            }
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
        path: '/error',
        component: () => import('../layout/DummyLayout'),
        redirect: 'noRedirect',
        name: 'ErrorPages',
        meta: {
            title: 'Error Pages',
            icon: '404'
        },
        children: [
            {
                path: '401',
                component: () => import('../views/errorPages/Unauthorized.vue'),
                name: 'Page401',
                meta: { title: '401', noCache: true }
            },
            {
                path: '404',
                component: () => import('../views/errorPages/NotFoundPage.vue'),
                name: 'Page404',
                meta: { title: '404', noCache: true }
            },
            {
                path: '500',
                component: () => import('../views/errorPages/InternalServerError.vue'),
                name: 'Page500',
                meta: { title: '500', noCache: true }
            }
        ]
    },
    { path: '/:catchAll(.*)', redirect: '/error/404', hidden: true },
    // {
    //     path: '/',
    //     component: ErrorPageLayout,
    //     children: [
    //         {
    //             path: '/:pathMatch(.*)*',
    //             name: 'NotFound',
    //             component: () => import('../views/errorPages/NotFoundPage.vue')
    //         },
    //     ]
    // },
]

export default routes;