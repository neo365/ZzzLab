<template>
  <el-scrollbar class="menu-scrollbar z-sidemenu">
    <el-menu
        :collapse="collapse"
        :default-active="activeMenu"
    >
      <SideMenuItem :menuItems="menuItems"/>
    </el-menu>
  </el-scrollbar>
</template>

<script>
import SideMenuItem from './SideMenuItem'

export default {
  name: "SideMenu",
  components: {
    SideMenuItem
  },
  props: {
    collapse: {type: Boolean, default: false},
    activeMenu: {type: String, default: ''},
  },
  data: () => ({
    menuItems: []
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
      let items = [
        {
          id: "1",
          parentId: null,
          icon: "fas fa-exchange-alt",
          text: "인터페이스1",
          path: "1a"
        },
        {
          id: "2",
          parentId: null,
          icon: "fas fa-exchange-alt",
          text: "인터페이스2",
          path: "2a"
        },
        {
          id: "21",
          parentId: "2",
          icon: "fas fa-exchange-alt",
          text: "인터페이스21",
          path: "21a"
        },
        {
          id: "211",
          parentId: "21",
          icon: "fas fa-exchange-alt",
          text: "인터페이스211",
          path: "211a"
        }
      ];

      this.menuItems = this.getMenuListToMenuTree(items);
    },
    getMenuListToMenuTree(items) {
      let map = {}, node, roots = [], i;

      for (i = 0; i < items.length; i += 1) {
        map[items[i].id] = i; // initialize the map
        items[i].children = []; // initialize the children
      }

      for (i = 0; i < items.length; i += 1) {
        node = items[i];
        if (node.parentId && node.parentId !== "0") {
          // if you have dangling branches check that map[node.parentId] exists
          items[map[node.parentId]].children.push(node);
        } else {
          roots.push(node);
        }
      }
      return roots;
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

.z-sidemenu .el-menu {
  border-right-width: 0 !important;
}


</style>