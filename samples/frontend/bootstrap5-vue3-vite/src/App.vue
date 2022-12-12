<template>
  <router-view/>
</template>
<script>
export default {
  name: "app",
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
  beforeUnmount() {
  },
  unmounted() {
  },
  methods: {
    init() {
    }
  },
};
</script>

<style>
#app {
  font-family: 'Noto Sans KR', "Nanum Gothic", sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-size: 1rem;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body::-webkit-scrollbar {
  width: 5px;
}

body::-webkit-scrollbar-thumb {
  height: 10%;
  background: #217af4;

  border-radius: 10px;
}

body::-webkit-scrollbar-track {
  background: rgba(33, 122, 244, .1);
}
</style>
