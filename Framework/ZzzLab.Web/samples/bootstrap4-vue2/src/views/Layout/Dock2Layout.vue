<template>
  <b-container class="dock-layout" fluid>
    <header class="header" :style="widhStyle">
      <b-navbar type="dark">
        <span><b-icon-grid3x3-gap/></span>
        <b-navbar-brand href="#">Test</b-navbar-brand>
        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <b-nav-item-dropdown right text="Lang">
            <b-dropdown-item href="#">English</b-dropdown-item>
            <b-dropdown-item href="#">한국어</b-dropdown-item>
            <b-dropdown-item href="#">中国語</b-dropdown-item>
            <b-dropdown-item href="#">日本語</b-dropdown-item>
          </b-nav-item-dropdown>

          <b-nav-item-dropdown text="테스터" right>
            <b-dropdown-item href="#"><i class="fas fa-user-cog"></i>설정</b-dropdown-item>
            <b-dropdown-divider/>
            <b-dropdown-item href="#"><i class="fas fa-sign-out-alt"></i>로그아웃</b-dropdown-item>

          </b-nav-item-dropdown>
        </b-navbar-nav>
      </b-navbar>
    </header>
    <SideMenu :isCollapse="isCollapse" :width="sideMenuWidth" @toggle="SideMenuToggle" />
    <main class="main" :style="widhStyle">
      <router-view/>
    </main>
    <footer class="footer">
      <Copyright/>
    </footer>
  </b-container>

</template>
<script>
import SideMenu from '@/views/Layout/Components/SideMenu'
import Copyright from "@/views/Components/Copyright.vue";

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "DockLayout",
  components: {
    SideMenu,
    Copyright,
  },
  data: () => ({
    sideMenuWidth : "200",
    isCollapse: false
  }),
  computed: {
    widhStyle() {
      return {
        'padding-left' : (this.isCollapse ? '64px': this.sideMenuWidth + 'px')
      }
    }
  },
  watch: {},
  beforeCreate() {  },
  created() {  },
  beforeMount() {
    this.init();
  },
  mounted() {  },
  beforeUpdate() {  },
  updated() {  },
  beforeDestroy() {  },
  destroyed() {  },
  methods: {
    init() {    },
    handleRoute(link) {
      if (this.$route.path !== link) this.$router.push(link)
    },
    SideMenuToggle() {
      this.isCollapse = !this.isCollapse;
    }
  },
};
</script>

<style scoped>
/* =====================================
Header
======================================== */
.header {
  background-color: #0078D4;
  color:white;
  height: 50px;
  position: fixed;
  left: 0px;
  right: 0;
  -webkit-transition: 0.5s;
  transition: 0.5s;
}

.header .dropdown-item i {
  margin-right: 5px;
  font-size: 18px;
}

.dropdown_user a{
  padding: 0;
}

/* =====================================
    main
======================================== */

main {
  padding: 55px 20px 30px 20px;
  min-height: 100vh;
  -webkit-transition: 0.5s;
  transition: 0.5s;
}

/* =====================================
    footer
======================================== */
.footer {
  font-family: tahoma,arial,Dotum,'Hiragino Sans GB',\5b8b\4f53,sans-serif;
  position: fixed;
  right: 0;
  bottom: 0;
  height: 25px;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: justify;
  -ms-flex-pack: justify;
  justify-content: space-between;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  flex-direction: row-reverse;
  -webkit-transition: 0.5s;
  transition: 0.5s;
}

.footer .copyright {
  margin-left: 15px;
  margin-right: 15px;
  font-size: 0.5em;
}

</style>
