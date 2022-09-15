<template>
  <v-container fill-height style="max-width: 22rem">
    <v-layout align-center row wrap>
      <v-flex xs12>
        <v-card class="login">
          <v-img :src="require('@/assets/Images/Login/logo.png')" aspect-ratio="1" contain/>
          <zl-divider style="margin: 5px 0"/>
          <validation-observer ref="observer" v-slot="{ handleSubmit }">
            <form @reset="onReset" @submit.prevent="handleSubmit(onSubmit)">
              <validation-provider v-slot="{ errors }" :rules="{ required: true, min: 3 }" name="아이디">
                <v-text-field v-model="loginForm.userId" :error-messages="errors" :state="getValidationState(errors)" label="아이디"
                              prepend-inner-icon="mdi-account" solo/>
              </validation-provider>
              <validation-provider v-slot="{ errors }" :rules="{ required: true, min: 6 }" name="비밀번호">
                <v-text-field v-model="loginForm.password" :error-messages="errors" :state="getValidationState(errors)"
                              label="비밀번호" prepend-inner-icon="mdi-lock"
                              solo type="password"/>
              </validation-provider>
              <v-checkbox
                  v-model="loginForm.rememberMe"
                  label="아이디 기억 하기"
                  style="margin-top:0;"
              ></v-checkbox>
              <v-btn block color="red" dark depressed large type="submit">
                로그인
              </v-btn>
              <v-overlay :absolute="true" :value="loginForm.progress">
                <v-progress-circular indeterminate size="64"/>
              </v-overlay>
            </form>
          </validation-observer>
          <zl-divider style="margin: 5px 0"/>
          <div class="footer">
            <Copyright/>
          </div>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>
<script>
export default {
  // eslint-disable-next-line
  name: "Login",
  components: {},
  props: {},
  data: () => ({
    loginForm: {
      userId: '',
      password: '',
      rememberMe: false,
      progress: false
    },
  }),
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
    this.randomBackground();
  },
  beforeUpdate() {
  },
  updated() {
  },
  methods: {
    init() {
      this.$root.$emit("setDocTitle", "로그인");
      this.loginForm.userId = this.$cookies.get('userId')
    },
    getValidationState({dirty, validated, valid = null}) {
      return dirty || validated ? valid : null;
    },
    onSubmit() {
      this.$refs.observer.validate();
      this.loginForm.progress = true;

      this.$store
          .dispatch('Signin', {
            userId: this.loginForm.userId,
            password: this.loginForm.password
          })
          .then(() => {
            this.$session.set('userId', this.loginForm.userId);
            //this.$cookies.set('userId', this.loginForm.userId, "30d")
            this.loginForm.progress = false;
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

            this.$root.$emit('showModalAlert', '에러', message);
            this.loginForm.progress = false;
          })

      return false;
    },
    onReset() {
      this.loginForm = {
        userId: '',
        password: '',
        rememberMe: false,
        Progress: false
      };

      this.$nextTick(() => {
        this.$refs.observer.reset();
      });
    },
    randomBackground() {
      // 랜덤 백그라운드.
      const bgList = [
        "vis01-00.jpg",
        "vis02-00.jpg",
        "vis03-00.jpg",
        "vis04-00.jpg",
        "vis05-00.jpg",
      ];
      const randNum = Math.floor(Math.random() * bgList.length);
      document.getElementById("inspire").style.backgroundImage = "url(" + require('@/assets/Images/Login/' + bgList[randNum]) + ")";
    }
  },
};
</script>
<style lang="scss">
#inspire {
  min-height: 100vh;
  background-image: url("~@/assets/Images/Login/vis01-00.jpg") no-repeat center center fixed;
  -webkit-background-size: cover;
  -moz-background-size: cover;
  -o-background-size: cover;
  background-size: cover;
}
</style>
<style scoped>
hr {
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
  border: 0;
  border-top: 1px solid rgba(0, 0, 0, 1);
}

.login {
  background-color: rgba(255, 255, 255, 0.5);
  padding: 20px;
}
</style>