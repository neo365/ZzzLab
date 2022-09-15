<template>
  <li>
  <template v-for="(menuItem, i) in menuItems">
  <el-submenu v-if="(menuItem.items && menuItem.items.length > 0)" :key="i" :index="menuItem.path" :item="menuItem">
    <template slot="title">
      <i :class="menuItem.icon"></i>
      <span slot="title">{{ menuItem.text }}</span>
    </template>
    <SideMenuItem :menuItems="menuItem.items"></SideMenuItem>
  </el-submenu>
  <el-menu-item v-else :key="menuItem.path" :index="menuItem.path" :item="menuItem">
    <i :class="menuItem.icon"></i>
    <span slot="title">{{ menuItem.text }}</span>
  </el-menu-item>
</template>
  </li>
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
  data: () => ({
    dummyItems: []
  }),
  computed: {},
  created() {
    //console.log(this.items);
  },
  methods: {}
}
</script>

<style scoped>
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

</style>