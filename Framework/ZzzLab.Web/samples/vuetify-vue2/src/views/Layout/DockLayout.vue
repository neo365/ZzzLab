<template>
  <v-app id="app">
    <SideBar/>
    <v-app-bar app color="#03a9f4" dark height="60" lipped-right>
      <v-toolbar-title>
        <v-icon v-text="title.icon"></v-icon>
        {{ title.text }}
      </v-toolbar-title>
      <v-spacer/>
      <v-btn icon @click="toggleFav">
        <v-icon>mdi-star-outline</v-icon>
      </v-btn>
      <AppBarMessage/>
      <AppBarAlarm/>
      <AppBarMenu/>
      <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" @click="logout()" v-on="on">
            <v-icon>mdi-logout</v-icon>
          </v-btn>
        </template>
        <span>로그아웃</span>
      </v-tooltip>
    </v-app-bar>
    <v-main app class="app-main">
      <v-breadcrumbs :items="breadItems" style="margin-bottom: 5px;"/>
      <router-view class="app-router"/>
      <v-spacer/>
      <div class="app-footer" style="text-align: right">
        <Copyright/>
      </div>
    </v-main>
  </v-app>
</template>

<script>
import AppBarMessage from "./Components/AppBar/AppBarMessage";
import AppBarAlarm from "./Components/AppBar/AppBarAlarm";
import AppBarMenu from "./Components/AppBar/AppBarMenu";
import SideBar from "./Components/SideBar";

export default {
  name: "DockLayout",
  components: {
    AppBarMessage,
    AppBarAlarm,
    AppBarMenu,
    SideBar
  },
  props: {},
  data: () => ({
    title: {
      icon: "mdi-connection",
      text: "연결설정"
    },
    breadItems: [
      {
        text: '설정',
        disabled: true,
        href: '/Settings',
      },
      {
        text: '데이터베이스',
        disabled: true,
        href: '/Settings/Database',
      },
      {
        text: '연결설정',
        disabled: true,
        href: '/Settings/Database/ConfigList',
      },
    ],
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
    toggleFav() {
      alert("fav");
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
<style lang="scss">
.app-main {
  background-image: url("~@/assets/Images/Main/bg_p.gif");
  background-repeat: repeat;
}

.app-footer {
  position: fixed;
  right:0;
  bottom:0;
}

</style>

<style>
.v-toolbar__title i {
  margin-right: 10px;
}

.v-main__wrap {
  padding: 5px;
}

.v-breadcrumbs {
  background-color: white;
  margin-bottom: 5px;
}
</style>

