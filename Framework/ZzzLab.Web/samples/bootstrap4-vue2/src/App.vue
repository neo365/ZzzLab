<template>
  <div id="app">
    <router-view/>
    <modalAlert ref="errorDialog" :message="errorDialog.message" :title="errorDialog.title" @click-confirm="callback"/>
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
  beforeCreate() {
  },
  created() {
    this.$root.$on('setDocTitle', (title) => {
      if (title) {
        document.title = process.env.VUE_APP_NAME + " :: " + title;
      }
    });

    this.$root.$on('showModalAlert', (title, message, callback) => {
      if (title) {
        this.errorDialog.title = title;
      } else {
        this.errorDialog.title = '알림';
      }

      if (message) {
        this.errorDialog.message = message;
      }
      if (callback) {
        this.errorDialog.callback = callback;
      } else {
        this.callback = () => {
        }
      }

      this.$refs.errorDialog.$emit('open');
    });
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
      else this.callback = () => {
        this.errorDialog.isShow = false;
      }

      this.errorDialog.isShow = true;
    },
    /* dummy callback function */
    callback() {
    }
  },
};
</script>

<style>
#app,
#inspire {
  font-family: 'Noto Sans KR', "Nanum Gothic", sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-size: 1rem;
  font-weight: 400;
  line-height: 1.5;
  position: relative;
  width: 100%;
  /*min-height: 100vh;*/
  margin: 0 auto;
  left: 0;
  background-color: #f0f3f6;
  -webkit-box-shadow: 0 0 3px #ccc;
  box-shadow: 0 0 3px #ccc;
  -webkit-transition: left 0.3s ease, padding-left 0.3s ease, margin-left .5s;
  transition: left 0.3s ease, padding-left 0.3s ease, margin-left .5s;
  overflow: hidden;
}
</style>
