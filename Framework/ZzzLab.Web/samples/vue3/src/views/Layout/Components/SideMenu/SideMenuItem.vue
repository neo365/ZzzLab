<template>
  <template v-for="(menuItem, i) in menuItems">
    <el-sub-menu v-if="(menuItem.items && menuItem.items.length > 0)" :key="i" :index="menuItem.path" :item="menuItem">
      <template #title>
        <i :class="menuItem.icon"></i>
        <span>{{ menuItem.text }}</span>
      </template>
      <SideMenuItem :menuItems="menuItem.items"></SideMenuItem>
    </el-sub-menu>
    <el-menu-item v-else :key="menuItem.path" :index="menuItem.path" :item="menuItem">
      <i :class="menuItem.icon"></i>
      <template #title>{{ menuItem.text }}</template>
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
  data: () => ({
    dummyItems: []
  }),
  computed: {},
  created() {},
  methods: {}
}
</script>


<style scoped>
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