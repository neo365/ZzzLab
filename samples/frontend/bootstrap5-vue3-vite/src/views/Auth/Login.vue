<template>
  로그인
</template>

<script>
export default {
  name: "DummyLayout",
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
  mounted() {},
  beforeUpdate() {},
  updated() {},
  beforeUnmount () {},
  unmounted () {},
  methods: {
    init() {
      this.$session.set('userId', 'debug_session')
      this.$cookies.set('userId', 'debug_cookies')
    },
  },
};
</script>