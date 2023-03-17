<template>
  <header class="app" style="display:flex">
    <div class="collapse-icon">
      <el-button link round @click="isCollapse=!isCollapse">
        <i class="fa-solid fa-bars"></i>
      </el-button>
    </div>
    <div>Debugging for RIST<i class="fa-solid fa-spider ms-1"></i></div>
    <div class="flex-grow"/>
    <div class="dropdown">
      <button id="dropdownMenuButton1" aria-expanded="false" class="btn dropdown-toggle z-dropdown" data-bs-toggle="dropdown"
              type="button">
        <i class="fa-solid fa-gear"></i>
      </button>
      <ul aria-labelledby="dropdownMenuButton1" class="dropdown-menu">
        <li><a class="dropdown-item" href="#">Action</a></li>
        <li><a class="dropdown-item" href="#">Another action</a></li>
        <li><a class="dropdown-item" href="#">Something else here</a></li>
      </ul>
      <button aria-expanded="false" class="btn" data-bs-toggle="dropdown" type="button">
        <i class="fa-solid fa-right-from-bracket"></i>
      </button>
    </div>
  </header>
  <aside :class="{ 'is-collapse' : isCollapse }" class="app">
    <SideMenu :collapse="isCollapse" @selected="sideMenuSelect" @toggle="sideMenuToggle"/>
  </aside>
  <main :class="{ 'is-collapse' : isCollapse }" class="app ">
    <div>
      <router-view/>
    </div>

  </main>
  <footer class="app">
    <Copyright/>
  </footer>

</template>
<script>


import SideMenu from '@/Layout/Components/SideMenu/index.vue'

export default {
  name: "DockLayout",
  components: {
    SideMenu,
  },
  data: () => ({
    isCollapse: false,
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
<style scoped>
/* =====================================
Header
======================================== */
header.app {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  padding: 0 10px;
  margin: 0;
  width: 100vw;
  height: var(--app-header-height);
  line-height: var(--app-header-height);
  border-bottom: solid 1px #e6e6e6;
  box-sizing: border-box;
  background-image: radial-gradient(transparent 1px, #ffffff 1px);
  background-size: 4px 4px;
  backdrop-filter: saturate(50%) blur(4px);
  -webkit-backdrop-filter: saturate(50%) blur(4px);
}

header.app .el-menu-demo {
  height: var(--app-header-height);
}

.flex-grow {
  flex-grow: 1;
}

/* =====================================
    Side Menu
======================================== */
aside.app {
  position: fixed;
  top: var(--app-header-height);
  bottom: 0;
  left: 0;
  width: var(--app-menu-width);
  overflow-x: hidden;
  overflow-y: auto;
  border-right: solid 1px #e6e6e6;
  -webkit-transition: 0.5s;
  transition: 0.5s;
  box-sizing: border-box;
  background-color: white;
}

.collapse-icon {
  width: var(--app-menu-width-collapse);
  padding: 0;
}

aside.app.is-collapse {
  width: var(--app-menu-width-collapse);
}

/* =====================================
    main
======================================== */

main.app {
  max-height: 100vh;
  margin: 0 0 0 var(--app-menu-width);
  padding: calc(var(--app-main-padding) + var(--app-header-height)) var(--app-main-padding) var(--app-footer-height) var(--app-main-padding);
  overflow-x: auto;
  overflow-y: auto;
  -webkit-transition: 0.5s;
  transition: 0.5s;
}

main.app.is-collapse {
  margin-left: var(--app-menu-width-collapse);
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
  background-image: radial-gradient(transparent 1px, #ffffff 1px);
  background-size: 4px 4px;
  backdrop-filter: saturate(50%) blur(4px);
  -webkit-backdrop-filter: saturate(50%) blur(4px);
}
</style>
