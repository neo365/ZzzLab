<template>
  <b-container class="app-container">
    <router-view/>
  </b-container>
</template>

<script>
export default {
  name: "DummyLayout",
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
