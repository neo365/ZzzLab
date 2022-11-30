<template>
  <div ref="container" :class=computedClasses>
    <slot/>
  </div>
</template>

<script>
export default {
  name: "z-container",
  components: {},
  props: {
    class: {type: String, default: ''},
    fluid: {type: Object, default: false}
  },
  data: () => ({}),
  computed: {
    isExistSlot: function () {
      return this.$slots.default ? true : false;
    },
    computedClasses: function () {
      let customClass = '';
      
      if(this.fluid === false) customClass += 'container';
      else if(this.fluid === true) customClass += 'container-fluid';
      else if(typeof this.fluid === 'string') customClass += `container-${this.fluid.toString()}`;

      return customClass + ' ' +this.class;
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