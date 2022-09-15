<template>
  <zl-datatable
      :headers="headers"
      :items="items"
      :items-per-page="10"
      :itemsPerPage="itemsPerPage"
      :itemsPerPageOptions="itemsPerPageOptions"
      :progress="progress"
      class="elevation-1"
      loading-text="페이지를 읽고 있습니다."
      multi-sort
  >
  </zl-datatable>
</template>

<script>

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "DataBaseConfigList",
  components: {},
  props: {},
  data: () => ({
    progress: true,
    headers: [
      {text: '섹센', value: 'section', align: 'start'},
      {text: '라벤', value: 'label'},
      {text: '커맨드', value: 'command'},
    ],
    itemsPerPageOptions: [10, 20, 50, 100, -1],
    itemsPerPage: 100,
    items: [],

  }),
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
      this.progress = true;

      //
      this.$http.get('/Api/Config/Query/List')
          .then((res) => {
            this.items = Object.freeze(res.data.items);
            this.progress = false;
          })
          .catch((Error) => {
            console.log(Error);
            let message;
            if (Error.response && Error.response.data) {
              message = Error.response.data.error;
            } else if (Error.message) {
              message = Error.message;
            }

            this.$root.$emit('showModalAlert', '에러', message);
            this.progress = false;
          })
    },
  },
};
</script>