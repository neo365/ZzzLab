<template>
  <b-container class="app-container">
    <main>
      <router-view/>
    </main>
    <footer class="copyright">
      <Copyright/>
    </footer>
  </b-container>
</template>

<script>

import Copyright from "@/views/Components/Copyright";

export default {
  name: "ErrorPageLayout",
  components: {
    Copyright,
  },
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
  beforeDestory() {
  },
  methods: {
    init() {
    },
  },
};
</script>

<style scoped>
footer.copyright {
  border-top: 1px solid #000000;
  margin-top: 10px;
  padding-top: 5px;
  text-align: center;
}
</style>
