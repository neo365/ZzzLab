<template>
  <template v-for="(menuItem, i) in menuItems">
    <el-sub-menu v-if="(menuItem.children && menuItem.children.length > 0)" :key="i" :index="menuItem.id"
                 :item="menuItem">
      <template #title>
        <i v-if="menuItem.icon" :class="menuItem.icon"></i>
        <i v-else class="fa-solid fa-circle-plus"></i>
        <span>{{ menuItem.title }}</span>
      </template>
      <SideMenuItem :menuItems="menuItem.children"></SideMenuItem>
    </el-sub-menu>
    <el-menu-item v-else :key="menuItem.id" :index="menuItem.id" :item="menuItem">
      <i v-if="menuItem.icon" :class="menuItem.icon"></i>
      <i v-else class="fa-solid fa-circle"></i>
      <template #title>{{ menuItem.title }}</template>
    </el-menu-item>
  </template>
</template>

<script>
export default {
  name: "SideMenuItem",
  props: {
    menuItems: {
      type: Array,
      default: () => []
    }
  },
  computed: {},
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
      console.log(this.menuItems);
    },
  }
}
</script>


<style scoped>
.el-sub-menu .b-icon,
.el-menu-item .b-icon,
.el-sub-menu svg,
.el-menu-item svg,
.el-sub-menu [class^=fa-],
.el-menu-item [class^=fa-] {
  /*margin: 0;*/
  margin-right: 7px !important;
  margin-left: 5px !important;
  /*width: 24px;*/
  text-align: center;
  font-size: 18px;
  vertical-align: middle;
  color: #909399;
  vertical-align: middle;
  /*width: var(--el-menu-icon-width);*/
}

.el-sub-menu.is-active svg,
.el-menu-item.is-active svg,
.el-sub-menu.is-active [class^=fa-],
.el-menu-item.is-active [class^=fa-] {
  color: inherit;
}

</style>