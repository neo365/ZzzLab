<template>

  <b-card class="login">
    <b-img :src="require('@/assets/Images/Login/logo.png')" alt="login logo" center fluid></b-img>
    <hr/>
    <validation-observer ref="observer" v-slot="{ handleSubmit }">
      <b-form @reset="onReset" @submit.stop.prevent="handleSubmit(onSubmit)">
        <validation-provider v-slot="errors" :rules="{ required: true, min: 3 }" name="아이디">
          <b-form-input v-model="loginForm.userId" :state="getValidationState(errors)"
                        aria-describedby="userid-live-feedback" placeholder="아이디"
                        type="text"/>
          <b-form-invalid-feedback id="userid-live-feedback" class="form-error-live-feedback">&nbsp;{{
              errors.errors[0]
            }}
          </b-form-invalid-feedback>
        </validation-provider>
        <validation-provider v-slot="errors" :rules="{ required: true, min: 6 }" name="비밀번호">
          <b-form-input v-model="loginForm.password" :state="getValidationState(errors)"
                        aria-describedby="password-live-feedback"
                        placeholder="비밀번호" type="password"/>
          <b-form-invalid-feedback id="password-live-feedback" class="form-error-live-feedback">&nbsp;{{
              errors.errors[0]
            }}
          </b-form-invalid-feedback>
        </validation-provider>
        <b-button block type="submit" variant="danger">
          로그인
        </b-button>
        <b-overlay :show="loginForm.progress" no-wrap rounded="sm" variant="transparent"/>
      </b-form>
    </validation-observer>
    <hr/>
    <b-row class="mx-auto">
      <Copyright/>
    </b-row>
  </b-card>
</template>
<script>
import Copyright from "@/views/Components/Copyright.vue";

export default {
  // eslint-disable-next-line
  name: "Login",
  components: {
    Copyright
  },
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
  beforeDestroy() {
  },
  destroyed() {
  },
  methods: {
    init() {
      this.$root.$emit("setDocTitle", "로그인");
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
      document.getElementById('app').style.backgroundImage = "url(" + require('@/assets/Images/Login/' + bgList[randNum]) + ")";
    }
  },
};
</script>
<style lang="scss">
#app {
  min-height: 100vh;
  background-image: url('~@/assets/Images/Login/vis01-00.jpg') no-repeat center center fixed;
  -webkit-background-size: cover;
  -moz-background-size: cover;
  -o-background-size: cover;
  background-size: cover;
}
</style>
<style>
.app-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
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
}

.form-error-live-feedback {
  display: block;
  margin-top: 8px;
  margin-bottom: 5px;
  font-size: 12px;
  line-height: 12px;
  word-break: break-word;
  overflow-wrap: break-word;
  word-wrap: break-word;
  -webkit-hyphens: auto;
  hyphens: auto;
}
</style>