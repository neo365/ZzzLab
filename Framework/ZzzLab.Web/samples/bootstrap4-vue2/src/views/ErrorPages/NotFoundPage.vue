<template>
  <b-jumbotron bg-variant="white" border-variant="white" fluid style="text-align: center;">
    <template #header>
      <b-img :src="require('@/assets/Images/ErrorPage/404NotFound.png')" alt="not found" centered fluid/>
    </template>
    <template #lead>
      <br/>
      <h1><strong>404 Not Found</strong></h1>
    </template>
    <p>죄송합니다. 존재하지 않는 주소를 입력하셨거나 요청하신 페이지의 주소가 변경, 삭제되어 찾을수 없습니다.</p>
    <br/><br/>
    <b-button variant="success" @click="back">메인으로</b-button>&nbsp;
    <b-button variant="primary" @click="back">뒤로가기</b-button>&nbsp;
    <b-button variant="danger" @click="bugReport">신고하기</b-button>
  </b-jumbotron>
</template>

<script>
export default {
  name: "NotFoundPage",
  methods: {
    bugReport() {
      if (this.$route.query.noGoBack) {
        this.$router.push({path: "/dashboard"});
      } else {
        this.$router.go(-1);
      }
    },
    back() {
      if (this.$route.query.noGoBack) {
        this.$router.push({path: "/dashboard"});
      } else {
        this.$router.go(-1);
      }
    },
  },
};
</script>
