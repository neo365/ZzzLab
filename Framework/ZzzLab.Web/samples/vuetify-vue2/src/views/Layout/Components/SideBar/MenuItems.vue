<template>
  <div>
    <template v-for="(item, i) in items">
        <v-list-group v-if="(item.children && item.children.length > 0) || depth < 2" :key="i"
                      no-action
                      :prepend-icon="item.icon"
                      :sub-group="(item.children && item.children.length > 0) && depth > 0"
                      active-class="child-active"
                      append-icon="fa-solid fa-disc-disk"
                      color="red"
                      @click="handleRoute(item.link)"
                      :style="item.link === $route.path ? 'color:var(--menu-active-color);' : ''"
                      >
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title>{{ item.title }}</v-list-item-title>
            </v-list-item-content>
          </template>
          <MenuItem :depth="depth + 1" :items="item.children"/>
        </v-list-group>
      <v-list-item v-else :key="i" link @click="handleRoute(item.link)"
                   :style="item.link === $route.path ? 'color:var(--menu-active-color);' : ''">
        <v-list-item-icon>
          <v-icon v-text="item.icon" :style="item.link === $route.path ? 'color:var(--menu-active-color);' : ''"/>
        </v-list-item-icon>
        <v-list-item-title v-text="item.title"/>
      </v-list-item>
    </template>
  </div>
</template>
<script>

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "MenuItem",
  components: {},
  props: {
    items: {
      type: Array,
    },
    depth: {
      type: Number,
      default: 0
    },
  },
  data: () => ({ }),
  computed: { },
  watch: { },
  beforeCreate() {
  },
  created() {
  },
  beforeMount() {
    this.init();
  },
  mounted() { },
  beforeUpdate() {
  },
  updated() {
  },
  beforeDestroy() {
  },
  destroyed() {
  },
  methods: {
    init() {
      this.menus = this.items;
    },
    handleRoute(link) {
      if (link && link != '') {
        if (this.$route.path !== link) this.$router.push(link)
      }
    },
    isSelected(link) {
      console.log(link)
      console.log(this.$route.path)
      console.log(this.$route.path == link)
      return (this.$route.path == link)
    }
  },
};
</script>

<style>
:root {
  --menu-active-color: red
}
</style>