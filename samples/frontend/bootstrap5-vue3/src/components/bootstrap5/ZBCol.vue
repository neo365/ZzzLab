<template>
  <div :class=mainClass>
    <slot/>
  </div>
</template>

<script>
export default {
  name: "z-col",
  components: {},
  props: {
    class: {type: String, default: ''},
    size: {type: String, default: ''}
  },
  data: () => ({}),
  computed: {
    isExistSlot: function () {
      return this.$slots.default ? true : false;
    },
    mainClass: function () {
      return 'col ' + this.class;
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
  methods: {
    init() {
    }
  },
};
</script>

<style scoped>

</style>