<template>
  <div>
    <main>
      <b-jumbotron bg-variant="white" border-variant="white" fluid style="text-align: center;">
        <template #header>
          <b-img :src="require('@/assets/Images/Login/logout.png')" alt="not found" centered fluid/>
        </template>
        <template #lead>
          <br/>
          <h1><strong>로그아웃중</strong></h1>
        </template>
        <b-progress :value="100" animated class="mt-3" variant="danger"></b-progress>
      </b-jumbotron>
    </main>
    <footer class="copyright">
      <Copyright/>
    </footer>
  </div>
</template>

<script>
import Copyright from "@/views/Components/Copyright.vue";

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Logout",
  components: {
    Copyright,
  },
  data: () => ({}),
  computed: {},
  watch: {},
  beforeCreate() {
  },
  created() {
  },
  beforeMount() {
    this.init();

  },
  mounted() {
    this.onLogout();
  },
  beforeDestory() {
  },
  methods: {
    init() {
    },
    onLogout() {
      this.$store
          .dispatch('Signout')
          .then(() => {
            window.location.href = "/";
          })
          .catch((Error) => {
            console.log(Error);
            let message;
            if (Error.response && Error.response.data) {
              message = Error.response.data.error;
            } else if (Error.message) {
              message = Error.message;
            }

            //EventBus.$emit('ShowErrorAlert', '알림', message);
            this.$root.$emit('showModalAlert', '에러', message);

            window.location.href = "/";
          })

      return false;
    },
  },
};
</script>
<style>
footer.copyright {
  border-top: 1px solid #000000;
  margin-top: 10px;
  padding-top: 5px;
  text-align: center;
}
</style>