/* eslint-disable */
const getters = {
  // state.app/state.permission는 사용하는곳이 없어보여도 내부적으로 사용
  sidebar: function(state) {
    return state.app.sidebar;
  },
  sidebarOnOff: function(state) {
    return state.app.sidebar.opened;
  }, 
  // 사용자정보
  user: function(state) {
    return state.user.user;
  },
  userId: function(state) {
    return state.user.user.userId;
  },
  conmpanyCode: function(state) {
    return state.user.user.conmpanyCode;
  },
  companyName: function(state) {
    return state.user.user.companyName;
  },
  deptCode: function(state) {
    return state.user.user.deptCode;
  },
  deptName: function(state) {
    return state.user.user.deptName;
  },
  userName: function(state) {
    return state.user.user.userName;
  },
};
export default getters;
