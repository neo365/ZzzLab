<template>
  <div class="container bg">
  <header class="app" style="display:flex">
    <div>asdad</div>
    <div>asdad2</div>
    <div>asdad3</div>
    <div class="flex-grow" />
  </header>
  <nav class="app" style="display:flex">
    <div style="width:34px;padding: 0 5px;">
      <el-button v-if="!isCollapse" link round @click="isCollapse=true">
        <i  class="fa-solid fa-chevron-left"></i>
      </el-button>
      <el-button v-else link round @click="isCollapse=false">
        <i class="fa-solid fa-chevron-right"></i>
      </el-button>
    </div>
    <div style="overflow-x: hidden; overflow-y: hidden">
      <el-breadcrumb separator="/" style="line-height: 30px;overflow-x: hidden;">
        <el-breadcrumb-item :to="{ path: '/' }">homepage</el-breadcrumb-item>
        <el-breadcrumb-item
        ><a href="/">promotion management</a></el-breadcrumb-item
        >
        <el-breadcrumb-item>promotion list</el-breadcrumb-item>
        <el-breadcrumb-item>promotion detail</el-breadcrumb-item>
      </el-breadcrumb>
    </div>
    <div class="flex-grow" />
  </nav>
  <aside class="app" :class="{ 'is-collapse' : isCollapse }">
    <SideMenu :isCollapse="isCollapse" @toggle="sideMenuToggle" @selected="sideMenuSelect"/>
  </aside>
  <main class="app " :class="{ 'is-collapse' : isCollapse }">
    <router-view/>
  </main>
  <footer class="app">
    <Copyright/>
  </footer>
  </div>
</template>
<script>

import SideMenu from './Components/SideMenu'
export default {
  name: "DockLayout",
  components: {
    SideMenu,
  },
  data: () => ({
    isCollapse: false,
  }),
  computed: {
  },
  watch: {},
  beforeCreate() {},
  created() {},
  beforeMount() {
    this.init();
  },
  mounted() {},
  beforeUpdate() {},
  updated() {},
  methods: {
    init() {
    },
    sideMenuToggle() {
      this.isCollapse = !this.isCollapse
      document.getElementsByClassName('app-menu').class
    },
    sideMenuSelect(keyPath) {
      console.log(keyPath)
    }

  },
};
</script>
<style lang="scss">
.bg {
  background-image: url("~@/assets/Images/Main/bg_p.gif");
  background-repeat: repeat;
}
</style>

<style scoped>
.container {
  position: fixed;
  top:0;
  left: 0;
  right: 0;
  bottom:0;
  padding: 0;
  margin:0;
  width:100vw;
  height: 100vh;
  overflow-x: hidden;
  overflow-y: hidden;
}

.flex-grow {
  flex-grow: 1;
}
/* =====================================
Header
======================================== */
header.app {
  position: fixed;
  top:0;
  left: 0;
  right: 0;
  padding: 0 10px;
  margin:0;
  width:100vw;
  height: var(--app-header-height);
  line-height:var(--app-header-height) ;
  border-bottom: solid 1px #e6e6e6;
  box-sizing: border-box;
  background-color:  #409EFF;
}

header.app .el-menu-demo {
  height: var(--app-header-height);
}

/* =====================================
nav
======================================== */
nav.app {
  position: fixed;
  top:var(--app-header-height);
  left: 0;
  right: 0;
  padding: 0 var(--el-menu-base-level-padding);
  margin:0;
  width:100vw;
  height: var(--app-nav-height);
  line-height: var(--app-nav-height);
  border-bottom: solid 1px #e6e6e6;
  box-sizing: border-box;
  background-color:   #ecf5ff;
}

/* =====================================
    Side Menu
======================================== */
aside.app {
  position: fixed;
  top: calc(var(--app-header-height) + var(--app-nav-height));
  bottom: 0;
  left: 0;
  width:var(--app-menu-width);
  overflow-x: hidden;
  overflow-y: auto;
  border-right: solid 1px #e6e6e6;
  -webkit-transition: 0.5s;
  transition: 0.5s;
  box-sizing: border-box;
  background-color:  white;
}


aside.app.is-collapse {
  width: var(--app-menu-width-collapse);
}
/* =====================================
    main
======================================== */

main.app {
  /*min-width: 100vw;*/
  /*min-height: 100vh;*/
  min-height: calc(100vh - var(--app-header-height) - var(--app-nav-height) - var(--app-footer-height) - var(--app-main-padding)- 20px);
  max-height: calc(100vh - var(--app-header-height) - var(--app-nav-height) - var(--app-footer-height) - var(--app-main-padding) - 20px);
  margin: calc(var(--app-header-height) + var(--app-nav-height) + var(--app-main-padding))  0 0 var(--app-menu-width);
  padding: var(--app-main-padding);
  overflow-x: auto;
  overflow-y: auto;
  -webkit-transition: 0.5s;
  transition: 0.5s;
}

main.app.is-collapse {
  margin-left: calc(var(--app-menu-width-collapse) + var(--app-main-padding));
}

/* =====================================
    footer
======================================== */
footer.app {
  font-family: tahoma, arial, Dotum, sans-serif;
  position: fixed;
  bottom: 0;
  right: 0;
  left: 0;
  height: var(--app-footer-height);
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
</style>
