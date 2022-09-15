/* eslint-disable */
import axios from 'axios';

const user = {
    // 프로젝트 공통 변수 정의
    state: {
        user: null
    },
    mutations: {
        SET_LOGIN(state, res) {
            axios.defaults.headers.common['Authorization'] = `${res.token_type} ${res.access_token}`;
        },
        SET_USER(state, userinfo) {
            state.user = userinfo
            localStorage.setItem('user', JSON.stringify(userinfo))
        },
        REST_ALL: state => {
            state.user = null;
            axios.defaults.headers.common['Authorization'] = null;
            // STORE의 정보를 CLEAR
            localStorage.removeItem('user');

            location.reload();
        },
    },
    actions: {
        Signin({commit}, credentials) {
            console.log(credentials);
            return axios
                .post(process.env.VUE_APP_API_BASE + '/Api/Auth/Signin', credentials)
                .then((Response) => {
                    console.log(Response.data);
                    commit('SET_LOGIN', Response.data);
                    commit('SET_USER', Response.data.user);
                })
        },
        // Log out
        Signout({commit}) {
            console.log('Signout');
            return axios
                .post(process.env.VUE_APP_API_BASE + '/Api/Auth/Signout')
                .then(({data}) => {
                    commit('REST_ALL');
                })
                .catch(error => {
                    // 오류가 나더라도 강제 logout
                    commit('REST_ALL');
                });
        },
    },
};

export default user;
