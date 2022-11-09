<template>
  <div :class=mainClass>
    <slot/>
  </div>
</template>

<script>
export default {
  name: "z-container",
  components: {},
  props: {
    class: {type: String, default: ''},
    size: {type: String, default: null},
  },
  data: () => ({}),
  computed: {
    isExistSlot: function () {
      return this.$slots.default ? true : false;
    },
    mainClass: function () {
      let customClass = '';

      if(!this.size) {
        customClass = 'container-' + this.size + ' ';
      } else {
        customClass = 'container ';
      }

      return customClass + this.class;
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