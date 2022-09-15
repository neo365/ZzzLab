<template>
  <el-scrollbar class="menu-scrollbar">
    <el-menu
        :collapse="isCollapse"
        :default-active="$route.path"
        class="menu-container"
        mode="vertical"
        @select="handleSelect"
        no-border
    >
      <el-menu-item index="0">
        <i class="fa-solid fa-bug"></i>
        <template #title><strong>Devtools</strong> for RIST</template>
      </el-menu-item>
      <div class="flex-grow" />
      <SideMenuItem :menuItems="menuItems"/>
    </el-menu>
  </el-scrollbar>
</template>
<script>

import SideMenuItem from './SideMenuItem'

// eslint-disable-next-line no-unused-vars
function getTreeNode(curLevelNodes, targetpath){
  // 현재 Array level에 찾는 node가 존재하면 바로 반환한다.
  let node = curLevelNodes.find(n => n.path === targetpath)
  if (node) return node

  // 다음 Level의 node들을 모은다. 이 때 undefined는 삭제한다.
  let nextLevelNodes = curLevelNodes
      .flatMap(n => n.child)
      .filter(c => c)

  // 재귀적으로 id를 찾는다
  return getTreeNode(nextLevelNodes, targetpath)
}

export default {
  name: "SideMenu",
  components: {
    SideMenuItem
  },
  props: {
    isCollapse: {type: Boolean, default: false},
  },
  data: () => ({
    menuItems: [
      {
        icon: "fa-solid fa-flask",
        text: "Navigator Onea",
        path: "1a",
        items: []
      },
      {
        icon: "fa-solid fa-flask",
        text: "Navigator Oneb",
        path: "2a",
        items: []
      },
      {
        icon: "fa-solid fa-flask",
        text: "Navigator Onec",
        path: "3a",
        items: [{
          icon: "fa-solid fa-flask",
          text: "Navigator Onez",
          path: "3a-1",
          items: [
            {
              icon: "fa-solid fa-flask",
              text: "Navigator Onec",
              path: "3a2",
              items: [{
                icon: "fa-solid fa-flask",
                text: "Navigator Onez",
                path: "3a-31",
                items: [
                  {
                    icon: "fa-solid fa-flask",
                    text: "Navigator Onec",
                    path: "3aa",
                    items: [{
                      icon: "fa-solid fa-flask",
                      text: "Navigator Onez",
                      path: "3a-41",
                      items: []
                    }]
                  }
                ]
              }]
            }
          ]
        }]
      }
    ]
  }),
  computed: {},
  methods: {
    // handleOpen(key, keyPath) {
    //   //this.$emit('toggle', true);  // 확인 버튼 클릭 후 이벤트 처리
    //   //console.log(key, keyPath);
    // },
    // handleClose(key, keyPath) {
    //   //console.log(key, keyPath);
    // },
    handleSelect(key, keyPath) {
      console.log(key, keyPath);

      let pathNode = [];
      let currentNode = this.menuItems;
      keyPath.forEach(function(item) {
        currentNode = getTreeNode(currentNode, item)
        pathNode.push(currentNode)
      });
      this.$emit('selected', pathNode);
    },
    toggleCollapse() {
      //this.isCollapse = !this.isCollapse;
      this.$emit('toggle');
    },
  }
}
</script>

<style scoped>
.menu-scrollbar {
  height: calc(100vh - var(--app-header-height) - var(--app-nav-height) - 1px);
  border-right: solid 0px #e6e6e6;
}

.menu-container {
  border-right: solid 0px #e6e6e6;
  overflow-x: hidden;
}

.el-sub-menu .b-icon,
.el-menu-item .b-icon,
.el-sub-menu svg,
.el-menu-item svg,
.el-sub-menu [class^=fa-],
.el-menu-item [class^=fa-] {
  margin-right: 5px;
  width: 24px;
  text-align: center;
  font-size: 18px;
  vertical-align: middle;
  color: #909399;
}

.el-sub-menu.is-active svg,
.el-menu-item.is-active svg,
.el-sub-menu.is-active [class^=fa-],
.el-menu-item.is-active [class^=fa-] {
  color: inherit;
}

</style>