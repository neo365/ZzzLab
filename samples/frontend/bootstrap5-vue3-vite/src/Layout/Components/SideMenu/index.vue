<template>
  <el-scrollbar class="menu-scrollbar z-sidemenu">
    <el-menu
        :collapse="collapse"
        :default-active="activeMenu"
        @close="handleClose"
        @open="handleOpen"
        @select="handleSelect"
    >
      <SideMenuItem :menuItems="menuTree"/>
    </el-menu>
  </el-scrollbar>
</template>

<script>
import {uuid} from 'vue-uuid';
import SideMenuItem from './SideMenuItem.vue'

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
    menuItems: [],
    menuTree: []
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
      this.$http({
        url: "./menu.json",
        method: 'GET'
      }).then(
          result => {
            if (result.data !== null) {
              this.menuItems = result.data.items;
              this.menuTree = this.getMenuListToMenuTree(result.data.items);
            }
          }
      );

      //this.menuItems = this.getMenuListToMenuTree(items);
    },
    getMenuListToMenuTree(items) {
      let map = {}, node, roots = [], i;

      for (i = 0; i < items.length; i += 1) {
        map[items[i].id] = i; // initialize the map
        items[i].children = []; // initialize the children
      }

      for (i = 0; i < items.length; i += 1) {
        node = items[i];
        if (!node.path) node.path = uuid.v1();

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
      this.$emit('toggle');
    },
    handleOpen(key, keyPath) {
      console.log('handleOpen', key, keyPath)
    },
    handleClose(key, keyPath) {
      console.log('handleClose', key, keyPath)
    },
    handleSelect(key, keyPath) {
      console.log('handleSelect', key, keyPath)

      const node = this.menuItems.find(x => x.id === key);

      console.log('handleSelect => find:', node.link);

      this.$router.push({path: node.link});
    }
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