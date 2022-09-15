<template>
  <v-menu bottom offset-y right>
    <template v-slot:activator="{ on, attrs }">
      <v-btn icon v-bind="attrs" v-on="on">
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-list>
        <v-list-item>
          <v-list-item-avatar>
            <img :src="require('@/assets/Images/Profile/user.png')" alt="John"/>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ $store.getters.userName }}</v-list-item-title>
            <v-list-item-subtitle>{{ $store.getters.companyName }} | {{ $store.getters.deptName }}
            </v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon v-bind="attrs" @click="logout()" v-on="on">
                  <v-icon>mdi-logout</v-icon>
                </v-btn>
              </template>
              <span>로그아웃</span>
            </v-tooltip>
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-divider></v-divider>
      <v-list>
        <v-list-item @click="handleDummy('사용자설정')">
          <v-list-item-icon>
            <v-icon>fa-user-gear</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>사용자설정</v-list-item-title>
            <v-list-item-subtitle>
              개인정보 변경 또는 비밀번호 변경등 로그인한 사용자에 관련된 설정을
              합니다.
            </v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item @click="handleDummy('시스템정보')">
          <v-list-item-icon>
            <v-icon>fa-circle-info</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>시스템정보</v-list-item-title>
            <v-list-item-subtitle>
              현재 시스템의 정보를 확인 합니다.
            </v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>
  </v-menu>
</template>

<script>
export default {
  name: "AppBarMenu",
  data: () => ({}),
  computed: {
    message: function () {
      return this.$route.params.id;
    },
  },
  watch: {},
  beforeCreate() {
  },
  created() {
  },
  beforeMount() {
    this.init();
  },
  mounted() {
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
    },
    logout() {
      this.$store
          .dispatch('Signout', {})
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

            this.$root.$emit('showModalAlert', '에러', message);

            window.location.href = "/";
          })
    },
  },
};
</script>

<style scoped>
</style>
