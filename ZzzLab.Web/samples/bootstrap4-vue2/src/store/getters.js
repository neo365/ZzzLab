/* eslint-disable */
const getters = {
    // state.app/state.permission는 사용하는곳이 없어보여도 내부적으로 사용
    sidebar: function (state) {
        return state.app.sidebar;
    },
    language: function (state) {
        return state.app.language;
    },
    size: function (state) {
        return state.app.size;
    },
    device: function (state) {
        return state.app.device;
    },
    permission_routers: function (state) {
        return state.permission.routers;
    },
    addRouters: function (state) {
        return state.permission.addRouters;
    },
    errorLogs: function (state) {
        return state.errorLog.logs;
    },
    appType: function (state) {
        return state.user.appType;
    },
    mainPage: function (state) {
        return state.user.mainPage;
    },
    loginType: function (state) {
        return state.user.loginType;
    },
    // 사용자정보
    user: function (state) {
        return state.user.user;
    },
    roles: function (state) {
        return state.user.roles;
    },
    deptNo: function (state) {
        return state.user.user.deptNo;
    },
    deptNm: function (state) {
        return state.user.user.deptNm;
    },
    userId: function (state) {
        return state.user.user.userId;
    },
    userNo: function (state) {
        return state.user.user.userNo;
    },
    userNm: function (state) {
        return state.user.user.userNm;
    },
    menuInfo: function (state) {
        return state.user.menu;
    },
    gAdminFlag: function (state) {
        return state.user.user.adminFlag;
    },
    test: function (state) {
        return 'aaaaa';
    }
};
export default getters;
