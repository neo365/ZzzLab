<template>
  <div id="wrapper">
    <router-view />
    <modalAlert ref="errorDialog" :title="errorDialog.title" :message="errorDialog.message" @click-confirm="callback" />
  </div>
</template>

<script>
//import "./fonts/NotoSansKR.css";
//import "./fonts/Roboto.css";
import modalAlert from '@/views/Components/ModalAlert';


export default {
  name: "App",
  components: {
    modalAlert
  },
  props: {},
  data: () => ({
    errorDialog: {
      title: '알림',
      message: ''
    }
  }),
  computed: {},
  watch: {},
  beforeCreate() { },
  created() {
    this.$root.$on('setDocTitle', (title) => {
      if (title) {
        document.title = process.env.VUE_APP_NAME + " :: " + title;
      }
    });

    this.$root.$on('showModalAlert', (title, message, callback) => {
      if (title) {
        this.errorDialog.title = title;
      }
      else {
        this.errorDialog.title = '알림';
      }

      if (message) {
        this.errorDialog.message = message;
      }
      if (callback) {
        this.errorDialog.callback = callback;
      } else {
        this.callback = () => { }
      }

      this.$refs.errorDialog.$emit('open');
    });
  },
  beforeMount() {
    this.init();
  },
  mounted() { },
  beforeUpdate() { },
  updated() { },
  beforeDestroy() { },
  destroyed() {
    this.$root.$off('setDocTitle');
    this.$root.$off('showModalAlert');
  },
  methods: {
    init() {
      document.title = process.env.VUE_APP_NAME + " :: ";
    },
    showModalAlert(title, message, callback) {
      if (title) this.errorDialog.title = title;
      if (message) this.errorDialog.message = message;

      if (callback) this.callback = callback;
      else this.callback = () => { this.errorDialog.isShow = false; }

      this.errorDialog.isShow = true;
    },
    /* dummy callback function */
    callback() { }
  },
};
</script>

<style>
#app {
  font-family: "Roboto","Noto Sans KR", "Nanum Gothic",  sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
</style>
