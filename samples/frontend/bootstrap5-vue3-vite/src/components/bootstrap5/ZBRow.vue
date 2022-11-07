<template>
  <div :class=mainClass>
    <slot/>
  </div>
</template>
<script>
export default {
  name: "z-row",
  components: {},
  props: {
    class: {type: String, default: ''}
  },
  data: () => ({}),
  computed: {
    isExistSlot: function () {
      return this.$slots.default ? true : false;
    },
    mainClass: function () {
      return 'row ' + this.class;
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