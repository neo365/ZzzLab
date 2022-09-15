<template>
  <div class="errPage-container">
    <h1>Oops!</h1>
    <p>접근권한이 없습니다.</p>
    <b-button variant="danger" @click="back">뒤로가기</b-button>
    &nbsp;
    <b-button variant="success" @click="login">로그인</b-button>
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'Page401',
  methods: {
    login() {
      if (this.$route.query.noGoBack) {
        this.$router.push({path: '/login'})
      } else {
        this.$router.go(-1)
      }
    },
    back() {
      if (this.$route.query.noGoBack) {
        this.$router.push({path: '/dashboard'})
      } else {
        this.$router.go(-1)
      }
    }
  }
}
</script>