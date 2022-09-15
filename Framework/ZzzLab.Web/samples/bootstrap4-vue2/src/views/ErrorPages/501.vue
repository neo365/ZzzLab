<template>
  <div class="errPage-container">
    <h1>Oops!</h1>
    <p>서버에러가 발생하였습니다.</p>
    <b-button variant="danger" @click="back">뒤로가기</b-button>
    &nbsp;
    <b-button variant="warning" @click="bugReport">신고하기</b-button>
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'Page401',
  methods: {
    bugReport() {
      if (this.$route.query.noGoBack) {
        this.$router.push({path: '/dashboard'})
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
