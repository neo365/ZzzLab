<template>
  <el-container>
    <el-header class="app-header">

          <el-button circle link size="default"
                     style="font-size: inherit; color: inherit;"
                     @click="sideMenuToggle()"><i class="fa-solid fa-bug"></i></el-button>

        <strong>Devtools</strong> for RIST

      <el-button link round @click="sideMenuToggle()"><i class="fa-solid fa-chevron-left"></i>
      </el-button>
      <el-dropdown>
    <span class="el-dropdown-link">
      Dropdown List
      <el-icon class="el-icon--right">
        <arrow-down />
      </el-icon>
    </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item>Action 1</el-dropdown-item>
            <el-dropdown-item>Action 2</el-dropdown-item>
            <el-dropdown-item>Action 3</el-dropdown-item>
            <el-dropdown-item disabled>Action 4</el-dropdown-item>
            <el-dropdown-item divided>Action 5</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </el-header>
    <el-container>
      <el-aside class="app-menu" width="299">
        <SideMenu :isCollapse="isCollapse" @toggle="sideMenuToggle"/>
      </el-aside>
      <el-container>
      <el-main>
        <router-view/>
      </el-main>
      <el-footer class="app-footer">
        <Copyright/>
      </el-footer>
        </el-container>
    </el-container>
  </el-container>
</template>

<script>
import SideMenu from '@/views/Layout/Components/SideMenu'

export default {
  name: "DockLayout",
  components: {
    SideMenu
  },
  props: {},
  data: () => ({
    sideMenuWidth: "300",
    isCollapse: false
  }),
  computed: {
    sideMenuSize() {
      return this.sideMenuWidth;
    }
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
  beforeUnmount() {
  },
  unmounted() {
  },
  methods: {
    init() {
    },
    sideMenuToggle() {
      this.isCollapse = !this.isCollapse;
    }
  },
};
</script>

<style scoped>
/*.app-header*/
/*{*/
/*  align-items: center;*/
/*  outline: none;*/
/*  text-decoration: none;*/
/*  font-size: var(--app-header-font-size);*/
/*  line-height: 45px;*/
/*  overflow-y: hidden;*/
/*  overflow-x: hidden;*/
/*  border-bottom: 1px solid #DCDFE6;*/
/*  height: var(--app-header-height);*/
/*}*/
/*!* =====================================*/
/*    Side Menu*/
/*======================================== *!*/
/*.app-menu {*/
/*  overflow-x: hidden;*/
/*  -webkit-transition: 0.5s;*/
/*  transition: 0.5s;*/
/*  z-index: 5;*/
/*  background-color: white;*/
/*}*/

/*.app-menu header {*/
/*  padding: 0 3px;*/
/*  align-items: center;*/
/*  display: flex;*/
/*  flex: 1 1 100%;*/
/*  min-height: var(--app-header-height);*/
/*  outline: none;*/
/*  position: relative;*/
/*  text-decoration: none;*/
/*  font-size: var(--app-header-font-size);*/
/*  line-height: 45px;*/
/*  overflow-y: hidden;*/
/*  overflow-x: hidden;*/
/*  border-bottom: 1px solid #DCDFE6;*/
/*  width: var(--el-aside-width, 300px);*/
/*  height: var(--app-header-height);*/
/*  box-sizing: border-box;*/
/*}*/

/*.app-menu header .brand-logo {*/
/*  align-self: flex-start;*/
/*  vertical-align: middle;*/
/*  margin-right: 5px;*/
/*  text-align: center;*/
/*  min-width:25px + var(--el-menu-base-level-padding);*/
/*  max-width:25px + var(--el-menu-base-level-padding);*/
/*}*/

/*.app-menu .side-menu-header-title {*/
/*  align-self: center;*/
/*  font-size: var(--app-header-title-font-size);*/
/*  flex: 1 1 100%;*/
/*  overflow: hidden;*/
/*  text-overflow: ellipsis;*/
/*  white-space: nowrap;*/
/*  line-height: var(--app-header-title-font-size);*/
/*  width: 150px;*/
/*}*/

/*!* =====================================*/
/*    footer*/
/*======================================== *!*/
/*.app-footer {*/
/*  font-family: tahoma, arial, Dotum, 'Hiragino Sans GB', \5b8b\4f53, sans-serif;*/
/*  position: fixed;*/
/*  right: 0;*/
/*  bottom: 0;*/
/*  height: 25px;*/
/*  padding: 0 20px;*/
/*  display: -webkit-box;*/
/*  display: -ms-flexbox;*/
/*  display: flex;*/
/*  -webkit-box-pack: justify;*/
/*  -ms-flex-pack: justify;*/
/*  justify-content: space-between;*/
/*  -webkit-box-align: center;*/
/*  -ms-flex-align: center;*/
/*  align-items: center;*/
/*  flex-direction: row-reverse;*/
/*  -webkit-transition: 0.5s;*/
/*  transition: 0.5s;*/
/*}*/

/*!*footer.copyright {*!*/
/*!*  margin-left: 15px;*!*/
/*!*  margin-right: 15px;*!*/
/*!*  font-size: 0.5em;*!*/
/*!*}*!*/

</style>