<template>
    <v-container style="max-width: 22rem;" fill-height>
        <v-layout align-center row wrap>
            <v-flex xs12>
                <v-card class="login">
                    <v-img :src="require('@/assets/Images/Login/logout.png')" block />
                    <div class="footer">
                        <Copyright />
                    </div>
                </v-card>
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script>

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Logout",
  components: { },
  data: () => ({}),
  computed: {},
  watch: {},
  beforeCreate() { },
  created() { },
  beforeMount() {
    this.init();

  },
  mounted() {
    this.onLogout();
  },
  beforeDestory() { },
  methods: {
    init() {},
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
          }
          else if (Error.message) {
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