<template>
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
  Dashboard<br />
</template>

<script>
export default {
  name: "DashBoard",
  components: {},
  props: {},
  data: () => ({}),
  computed: {},
  watch: {},
  beforeCreate() {},
  created() {},
  beforeMount() {
    this.init();
  },
  mounted() {
      this.emitter.on('DebugMessage',this.getDebugMessage)
  },
  beforeUpdate() {},
  updated() {},
  beforeUnmount () {},
  unmounted () {},
  methods: {
    init() {
    },
    getDebugMessage(message) {
      alert(message);
    }
  },
};
</script>