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
  /*width: 100vw;*/
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body::-webkit-scrollbar {
  width: 5px;  /* 스크롤바의 너비 */
}

body::-webkit-scrollbar-thumb {
  height: 10%; /* 스크롤바의 길이 */
  background: #217af4; /* 스크롤바의 색상 */

  border-radius: 10px;
}

body::-webkit-scrollbar-track {
  background: rgba(33, 122, 244, .1);  /*스크롤바 뒷 배경 색상*/
}
</style>
