<template>
  <header class="app" style="display:flex">
    <div class="collapse-icon">
      <el-button link round @click="isCollapse=!isCollapse">
        <i class="fa-solid fa-bars"></i>
      </el-button>
    </div>
    <div>Debugging for RIST<i class="fa-solid fa-spider ms-1"></i></div>
    <div class="flex-grow" />
    <div class="dropdown">
      <button class="btn dropdown-toggle z-dropdown" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
        <i class="fa-solid fa-gear"></i>
      </button>
      <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
        <li><a class="dropdown-item" href="#">Action</a></li>
        <li><a class="dropdown-item" href="#">Another action</a></li>
        <li><a class="dropdown-item" href="#">Something else here</a></li>
      </ul>
      <button class="btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        <i class="fa-solid fa-right-from-bracket"></i>
      </button>
    </div>
  </header>
  <aside class="app" :class="{ 'is-collapse' : isCollapse }">
    <SideMenu :collapse="isCollapse" @toggle="sideMenuToggle" @selected="sideMenuSelect"/>
  </aside>
  <main class="app " :class="{ 'is-collapse' : isCollapse }">
    <div><router-view/></div>

  </main>
  <footer class="app">
    <Copyright/>
  </footer>

</template>
<script>

import SideMenu from './Components/SideMenu'
export default {
  name: "DockLayout",
  components: {
    SideMenu,
  },
  data: () => ({
    isCollapse: true,
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
  background-image: radial-gradient(transparent 1px,#ffffff 1px);
  background-size: 4px 4px;
  backdrop-filter: saturate(50%) blur(4px);
  -webkit-backdrop-filter: saturate(50%) blur(4px);
}

header.app .el-menu-demo {
  height: var(--app-header-height);
}

/* =====================================
    Side Menu
======================================== */
aside.app {
  position: fixed;
  top: var(--app-header-height) ;
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

.collapse-icon {
  width:var(--app-menu-width-collapse);
  padding: 0;
}

aside.app.is-collapse {
  width: var(--app-menu-width-collapse);
}
/* =====================================
    main
======================================== */

main.app {
  max-height: calc(100vh - var(--app-main-padding)- var(--app-footer-height));
  margin: 0  0 0 var(--app-menu-width);
  padding: calc(var(--app-main-padding) + var(--app-header-height)) var(--app-main-padding) calc(var(--app-main-padding) + var(--app-footer-height)) var(--app-main-padding) ;
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
  background-image: radial-gradient(transparent 1px,#ffffff 1px);
  background-size: 4px 4px;
  backdrop-filter: saturate(50%) blur(4px);
  -webkit-backdrop-filter: saturate(50%) blur(4px);
}
</style>
