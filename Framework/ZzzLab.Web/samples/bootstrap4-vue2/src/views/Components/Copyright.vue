<template>
  <span class="copyright">
    © {{ new Date().getFullYear() }} <strong>ZzzLab</strong> Co.,Ltd. All Rights Reserved.
    | Powered by <strong>neo365</strong></span>
</template>
<script>
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Copyright",
  components: {},
  props: {},
  data: () => ({}),
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
  beforeDestroy() {
  },
  destroyed() {
  },
  methods: {
    init() {
    },
  },
};
</script>

<style scoped>
.copyright {
  font-family: tahoma, arial, Dotum, "Hiragino Sans GB", \5b8b\4f53, sans-serif;
  font-size: 0.5em;
}
</style>
