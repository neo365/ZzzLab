<template>
  <aside class="side-menu" :style="style">
    <header>
      <div class="side-menu-header-icon">
        <el-button circle icon="fa-solid fa-bug" size="default" style="font-size: inherit; color: inherit;"
                   type="text"
                   @click="toggleCollapse()"></el-button>
      </div>
      <div :hidden="isCollapse" class="side-menu-header-title"><strong>Devtools</strong> for RIST</div>
      <div :hidden="isCollapse" class="side-menu-header-tail">
        <el-button round type="text" @click="toggleCollapse()"><i class="fa-solid fa-chevron-left"></i>
        </el-button>
      </div>
    </header>
<!--    <div class="top-bg">-->
<!--      <el-button id="menu" type="button" class="m-btn" aria-haspopup="true" aria-expanded="false"><svg-icon icon-class="bar" style="color: #fff; margin-right: 0; font-size: 14px" /></el-button>-->
<!--      <el-button id="favLabel" type="button" class="none-btn" aria-haspopup="true" aria-expanded="false"><svg-icon icon-class="star" style="color: #2684cd" /></el-button>-->
<!--    </div>-->
    <el-menu mode="vertical"
             :show-timeout="200"
             :default-active="$route.path"
             class="side-menu-container"
             @open="handleOpen"
             @close="handleClose"
             :collapse="isCollapse">
<!--      <template v-for="item in menuItems">-->
<!--        <el-submenu v-if="item.items && item.items.length > 0" :key="item.path" :item="item" >-->
<!--          <template slot="title">-->
<!--            <i :class="item.icon"></i>-->
<!--            <span slot="title">{{ item.text }}</span>-->
<!--          </template>-->
<!--          <el-menu-item v-for="subitem in item.items" :key="subitem.path" :item="subitem">{{ subitem.text }}</el-menu-item>-->
<!--        </el-submenu>-->
<!--        <el-menu-item v-else :key="item.path" :item="item">-->
<!--          <i :class="item.icon"></i>-->
<!--          <span slot="title">{{ item.text }}</span>-->
<!--        </el-menu-item>-->
<!--      </template>-->
      <SideMenuItem :menuItems="menuItems" />
      <el-submenu index="1">
        <template slot="title">
          <i class="fa-solid fa-flask"></i>
          <span slot="title">Navigator Onezxc</span>
        </template>
        <el-menu-item-group>
          <span slot="title">Group One</span>
          <el-menu-item index="1-1">item one</el-menu-item>
          <el-menu-item index="1-2">item two</el-menu-item>
        </el-menu-item-group>
        <el-menu-item-group title="Group Two">
          <el-menu-item index="1-3">item three</el-menu-item>
          <el-submenu index="1-3a">
            <template slot="title">
              <i class="fa-solid fa-flask"></i>
              <span slot="title">item threea</span>
            </template>
            <el-menu-item index="1-3a-1">item one</el-menu-item>
          </el-submenu>
        </el-menu-item-group>
        <el-submenu index="1-4">
          <template slot="title">
            <i class="el-icon-menu"></i>
            <span slot="title">item four</span>
          </template>
          <el-menu-item index="1-4-1">item one</el-menu-item>
        </el-submenu>
      </el-submenu>
      <el-menu-item index="2">
        <i class="el-icon-menu"></i>
        <span slot="title">Navigator Two</span>
      </el-menu-item>
      <el-menu-item index="3" >
        <i class="el-icon-document"></i>
        <span slot="title">리포트</span>
      </el-menu-item>
      <el-menu-item index="4">
        <i class="el-icon-setting"></i>        
        <span slot="title">설정</span>
      </el-menu-item>
      <el-menu-item index="debug">
        <i class="fa-solid fa-bug"></i>        
        <span slot="title">디버깅</span>
      </el-menu-item>
    </el-menu>
  </aside>
</template>
<script>

import SideMenuItem from './SideMenuItem'

export default {
    name: "SideMenu",
    components: {
      SideMenuItem
    },
    props: {
      isCollapse : {type: Boolean, default: false},
      width: { default: '256'},

    },
    data: () => ({
      menuItems: [
        {
          icon: "fa-solid fa-flask",
          text: "Navigator Onea",
          path:"1a",
          items: []
        },
        {
          icon: "fa-solid fa-flask",
          text: "Navigator Oneb",
          path:"2a",
          items: []
        },
        {
          icon: "fa-solid fa-flask",
          text: "Navigator Onec",
          path:"3a",
          items: [{
            icon: "fa-solid fa-flask",
            text: "Navigator Onez",
            path:"3a-1",
            items: []
          }]
        }
      ]
    }),
    computed: {
      style() {
          return {
            'width' : (this.isCollapse ? '64px': this.width + 'px')
          }
      },
    },
    methods: {
      handleOpen(key, keyPath) {
        //this.$emit('toggle', true);  // 확인 버튼 클릭 후 이벤트 처리
        console.log(key, keyPath);
      },
      handleClose(key, keyPath) {
        console.log(key, keyPath);
      },
      toggleCollapse() {
        //this.isCollapse = !this.isCollapse;
        this.$emit('toggle');
      }
    }
  }
</script>

<style scoped>
.scrollbar-wrapper {
  overflow-x: hidden; /* Disable horizontal scroll */
}
/* =====================================
    Side Menu
======================================== */
aside.side-menu {
    position: fixed;
    top: 0px;
    bottom:0;
    left: 0px;
    overflow-x: hidden; /* Disable horizontal scroll */
    -webkit-transition: 0.5s;
    transition: 0.5s;
    z-index: 5;
    border-right: solid 1px #e6e6e6;
    background-color: white;
}

.side-menu header {
  padding: 0 3px;
  align-items: center;
  display: flex;
  flex: 1 1 100%;
  letter-spacing: normal;
  min-height: 50px;
  outline: none;
  position: relative;
  text-decoration: none;
  font-size: 1.3rem;
  line-height: 50px;
  overflow-y: hidden;
  overflow-x: hidden;
  border-bottom: 1px solid #DCDFE6;
}

.side-menu-header-icon {
  align-self: flex-start;
  vertical-align: middle;
  margin-right: 5px;
  padding-left: 5px;
  /*width: 64px;*/
  text-align: center;
}

.side-menu-header-title {
  align-self: center;
  font-size: 1rem;
  flex: 1 1 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  line-height: 1.2;
  width: 170px;
}

.side-menu-header-tail {
  align-items: center;
  color: inherit;
  display: flex;
  flex: 1 0 auto;
  justify-content: inherit;
  line-height: normal;
  position: relative;
  transition: inherit;
  transition-property: opacity;
}

.side-menu .side-menu-container {
    width: 100%;
    height:calc(100vh - 84px);
    overflow-y: auto;
    overflow-x: hidden;
}



.side-menu .side-menu-container:not(.el-menu--collapse) {
  /* height:100%; */
  min-height: 400px;
  overflow-y: auto;
  overflow-x: hidden;
  border-right: solid 0px #e6e6e6;
}

.side-menu .side-menu-container.el-menu--collapse {
  /*height:100%;*/
  min-height: 400px;
  border-right: solid 0px #e6e6e6;
}

.side-menu .side-menu-container::-webkit-scrollbar-track {
    background-color: #2c353e;
}

.el-submenu .b-icon,
.el-menu-item .b-icon,
.el-submenu svg,
.el-menu-item svg,
.el-submenu [class^=fa-],
.el-menu-item [class^=fa-] {
  margin-right: 5px;
  width: 24px;
  text-align: center;
  font-size: 18px;
  vertical-align: middle;
  color: #909399;
}

.el-submenu.is-active svg,
.el-menu-item.is-active svg,
.el-submenu.is-active [class^=fa-],
.el-menu-item.is-active [class^=fa-] {
  color: inherit;
}

.side-menu footer {
    position: absolute;
    bottom: 5px;
    left: 0;
    width: 100%;
    display:flex;
    flex-direction: row-reverse;
    padding-right: 15px;
    padding-left: 15px;
}
</style>