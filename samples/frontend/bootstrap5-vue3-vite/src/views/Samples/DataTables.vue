<template>
  <z-datatables
      id="3"
      :headers="headers"
      :items="items"
      :options="options"
      class="table-bordered table-condensed nowrap"
  />

<!--  <DataTable-->
<!--      :columns="headers"-->
<!--      :options="options"-->
<!--      :data="items"-->
<!--      class="table table-hover table-striped"-->
<!--  />-->
</template>

<script>
import DataTable from 'datatables.net-vue3'
import DataTableBs5 from 'datatables.net-bs5'


export default {
  name: "DataTables",
  components: {
    DataTable
  },
  props: {},
  data: () => ({
    options: {},
    headers: [
      {title: 'a', data: 'name'},
      {title: 'b', data: 'position'},
      {title: 'c', data: 'office'},
      {title: 'd', data: 'extn'},
      {title: 'e', data: 'start_date'},
      {title: 'f', data: 'salary'},
    ],
    items: []
  }),
  computed: {},
  watch: {},
  beforeCreate() {
  },
  created() {
  },
  beforeMount() {
    DataTable.use(DataTableBs5);
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

      let config = {
        processing: true,
        serverSide: false,
        searching: false,
        ordering: !!this.headers.find(x => x.orderable),
        paging: true,
        pageLength: 30,
        lengthMenu: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 1000],
        autoWidth: false,
        scrollY: "calc(100vh - 290px)",
        scrollX: true,
        //deferRender:    true,
        //scroller:       true,
        //scrollCollapse: true,
        stateSave: true,
        stateDuration: 60 * 60 * 24, // 1Day
        colReorder: true,
        colResize: {
          isEnabled: true,
          hoverClass: 'dt-colresizable-hover',
          hasBoundCheck: true,
          minBoundClass: 'dt-colresizable-bound-min',
          maxBoundClass: 'dt-colresizable-bound-max',
          isResizable: function (column) { return true; },
          onResize: function (column) { },
          onResizeEnd: function (column, columns) { },
          getMinWidthOf: function ($thNode) { }
        },
        //responsive: true,
        rowReorder: true,
      };

      this.options = config;

      this.$http({
        url: "http://localhost:8081/data.json",
        method: 'GET'
      }).then(
          result => {
            if (result.data !== null) {
              //this.items =Object.freeze(result.data.data);
              this.items =result.data.data;
            }
          }
      );
    },
    testMsg(msg) {
      alert(msg);
    }
  },


}
</script>

<style scoped>

</style>