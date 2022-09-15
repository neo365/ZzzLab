<template>
  <v-data-table
      :footer-props="footerProps"
      :headers="headers"
      :items="items"
      :items-per-page="itemsPerPage"
      :loading="progress"
      :loading-text="progressText"
      multi-sort
  >
  </v-data-table>
</template>

<script>
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "zl-datatable",
  components: {},
  props: {
    headers: {
      type: Array,
      required: true
    },
    items: {
      type: Array,
    },
    progress: {type: Boolean, default: false},
    progressText: {type: String, default: "페이지를 읽고 있습니다."},
    itemsPerPage: {type: Number, default: 20},
    itemsPerPageOptions: {type: [Array, Number], default: () => [10, 20, 30, 50, 100, -1]},
  },
  data: () => ({}),
  computed: {
    footerProps() {
      return {
        'items-per-page-options': this.itemsPerPageOptions,
        'items-per-page-text': '페이지에 보여줄 수',
        pageText: '전체 {2}개중 {0}-{1}번째'
      }
    },
    deepFreeze(object) {
      // 객체에 정의된 속성명을 추출
      var propNames = Object.getOwnPropertyNames(object)
      // 스스로를 동결하기 전에 속성을 동결
      for (let name of propNames) {
        let value = object[name]
        object[name] =
            value && typeof value === "object" ? this.deepFreeze(value) : value
      }
      return Object.freeze(object)
    }

  },
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
<style lang="scss">
</style>
<style>
.datatable-header,
.elevation-1 th {
  /*background-color: lightgray;*/
}

tbody tr:nth-of-type(odd) {
  background-color: rgba(0, 0, 0, .05);
}
</style>