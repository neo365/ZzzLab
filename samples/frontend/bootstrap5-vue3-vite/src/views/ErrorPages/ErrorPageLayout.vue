<template>
    <div id="error-pages">
      <router-view/>
    </div>
</template>

<script>
export default {
  name: "ErrorPageLayout",
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
    },
  },
};
</script>
<style scoped>
* {
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
}

#error-pages {
  position: relative;
  height: 100vh;
}
</style>
<style>

#error-pages .error {
  position: absolute;
  left: 50%;
  top: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}

#error-pages .error {
  max-width: 520px;
  width: 100%;
  line-height: 1.4;
  text-align: center;
}

#error-pages .error .error-content {
  position: relative;
  height: 240px;
}

#error-pages .error .error-content h1 {
  font-family: 'Montserrat', sans-serif;
  position: absolute;
  left: 50%;
  top: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
  font-size: 252px;
  font-weight: 900;
  margin: 0px;
  color: #262626;
  text-transform: uppercase;
  letter-spacing: -40px;
  margin-left: -20px;
}

#error-pages .error .error-content h1>span {
  text-shadow: -8px 0px 0px #fff;
}

#error-pages .error .error-content h3 {
  font-family: 'Cabin', sans-serif;
  position: relative;
  font-size: 16px;
  font-weight: 700;
  text-transform: uppercase;
  color: #262626;
  margin: 0px;
  letter-spacing: 3px;
  padding-left: 6px;
}

#error-pages .error h2 {
  font-family: 'Cabin', sans-serif;
  font-size: 20px;
  font-weight: 400;
  text-transform: uppercase;
  color: #000;
  margin-top: 0px;
  margin-bottom: 25px;
}

@media only screen and (max-width: 767px) {
  #error-pages .error .error-content {
    height: 200px;
  }
  #error-pages .error .error-content h1 {
    font-size: 200px;
  }
}

@media only screen and (max-width: 480px) {
  #error-pages .error .error-content {
    height: 162px;
  }
  #error-pages .error .error-content h1 {
    font-size: 162px;
    height: 150px;
    line-height: 162px;
  }
  .error h2 {
    font-size: 16px;
  }
}

#error-pages .copyright {
  border-top: 1px solid #000000;
  margin-top: 10px;
  padding-top: 5px;
  text-align: center;
}
</style>
