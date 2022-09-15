import cookies from "vue-cookies";

const app = {
    state: {
        sidebar: {
            opened: true,
        },
    },
    mutations: {
        SET_SIDEBAR_ONOFF: (state, onoff) => {
            console.log('SIDEBAR_ONOFF:' + onoff);
            cookies.set('sidebarOpened', onoff);
            state.sidebar.opened = onoff;
        },
    },
    actions: {
        openSideBar({ commit }) {
            commit('SET_SIDEBAR_ONOFF', true);
        },
        closeSideBar({ commit }) {
            commit('SET_SIDEBAR_ONOFF', false);
        }
    },
};

export default app;