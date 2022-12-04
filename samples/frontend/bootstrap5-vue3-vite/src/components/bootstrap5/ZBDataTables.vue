<template>
  <DataTable :id="computedId"
             ref="zdatatable"
             class="table"
             :class="computedClass"
             :columns="computedHeaders"
             :data="computedItems"
             :options="computedOptions"
  />

  <z-button @click="redrawGrid()">test</z-button>
  <z-button @click="copyExcel()">test</z-button>
</template>
<script>
import DataTable from 'datatables.net-vue3'
import DataTableBs5 from 'datatables.net-bs5'

import DataTableRowReorder from 'datatables.net-rowreorder'
import DataTableColReorder from 'datatables.net-colreorder'
// import DataTableSelect from 'datatables.net-select'
import DataTableResponsive from 'datatables.net-responsive'
import DataTableColResize from 'datatables.net-colresize-unofficial'
//import DataTableDefault from '@/components/dataTables.default.js'

import 'datatables.net-bs5/css/dataTables.bootstrap5.min.css';
import 'datatables.net-colresize-unofficial/jquery.dataTables.colResize.css';

export default {
  name: "z-datatables",
  components: {
    DataTable
  },
  props: {
    id: {type: String, default: ''},
    headers: {type: Array, default: []},
    items: {type: Array, default: []},
    options: {type: Object, default: {}},
    class: {type: String, default: ''},
    hover: {type: Boolean, default: true},
    striped: {type: Boolean, default: true},
    rowReorder: {type: Boolean, default: false},
    colReorder: {type: Boolean, default: false},
    colResizeble: {type: Boolean, default: false},
    responsive: {type: Boolean, default: false},
  },
  data: () => ({
    tableObject: null
  }),
  computed: {
    computedId: function () {
      return this.id;
    },
    computedClass: function () {
      let customClass = '';
      if (this.hover) customClass += ' table-hover';
      if (this.striped) customClass += ' table-striped';

      return customClass + ' ' + this.class;
    },
    computedHeaders: function () {
      let mainHreaders = [];

      this.headers.forEach(function (value) {
        let item = value;
        if (!item.orderable) item.orderable = false;

        if(item.type) {
          switch (item.type) {
            case "ShortDate":
              item.render = function (value, idx, data) {
                if (value) return value.parseDate().toShortDateString();
                return null;
              }
          }
        }
        mainHreaders.push(item);
      });

      return mainHreaders;
    },
    computedOptions: function () {
      let config = this.options;
      config.ordering = !!this.headers.find(x => x.orderable)
      config.initComplete = function (settings, json) {
        console.log('initComplete', settings, json);
        // const api = new jquery.fn.dataTable.Api(settings);
        // console.log('initComplete2', api);
      }
      config.select = {
        style: 'os',
        items: 'cell'
      }

      if(!config.language) {
        config.language = {
            emptyTable: "데이터가 없습니다.",
            info: "전체 _TOTAL_ 개",//"Showing _START_ to _END_ of _TOTAL_ 개의 항목",
            infoEmpty: "",//"Showing 0 to 0 of 0 entries",
            infoFiltered: "(총 _MAX_ 개)", //"(filtered from _MAX_ total entries)",
            infoPostFix: "",
            decimal: ",",
            thousands: ",",
            lengthMenu: '<span class="dataTables_lengthMenu">페이지당 줄수</span> _MENU_', //Show _MENU_ entries,
            loadingRecords: "읽는중...",
            processing: "처리중...", //"Processing...",
            search: '<span class="dataTables_search">검색</span> : ', //"Search:",
            zeroRecords: "검색 결과가 없습니다", //"No matching records found",
            paginate: {
            first: "처음", //"First",
            last: "마지막",//"Last",
            next: "다음",//"Next",
            previous: "이전" //"Previous"
          },
          aria: {
            sortAscending: ": 오름차순 정렬", //": activate to sort column ascending",
            sortDescending: ": 내림차순 정렬", //": activate to sort column descending"
          }
        }
      }

      return config;
    },
    computedItems: function () {
      return this.items;
    },

  },
  watch: {},
  beforeCreate() {
  },
  created() {
  },
  beforeMount() {
    DataTable.use(DataTableBs5);
    // DataTable.use(DataTableSelect);

    if(this.rowReorder) DataTable.use(DataTableRowReorder);
    if(this.colReorder) DataTable.use(DataTableColReorder);
    if(this.responsive) DataTable.use(DataTableResponsive);

    if(this.colResizeble) DataTable.use(DataTableColResize);
    //DataTable.use(DataTableDefault);

    this.init();
  },
  mounted() {
    this.tableObject = this.$refs.zdatatable.dt();
    // this.tableObject.cells( { selected: true } ).data();
    // this.tableObject.rows( '.important', { selected: true } ).data();

    window.addEventListener('resize', this.resizeGrid);
    window.addEventListener(
        window.location.pathname,
        () => {
          window.removeEventListener('resize', this.resizeGrid);
          this.$destroy();
        },
        {once: true}
    );

    window.addEventListener("paste", this.paste, event);
  },
  beforeUpdate() {
  },
  updated() {
  },
  beforeUnmount() {
    window.removeEventListener('resize', this.resizeGrid);
    window.removeEventListener('paste');
  },
  unmounted() {
  },
  methods: {
    init() {
    },
    resizeGrid() {
      this.redrawGrid();
    },
    redrawGrid() {
      this.tableObject.draw();
    },
    copy() {
    },
    paste(e) {
      var paste = e.clipboardData && e.clipboardData.getData ?
          e.clipboardData.getData('text/plain') :                // Standard
          window.clipboardData && window.clipboardData.getData ?
              window.clipboardData.getData('Text') :                 // MS
              false;
      if (paste) {
        console.log("gogo", paste)
      }
    }
  },
};
</script>

<style>
table.dataTable thead>tr>th {
  text-align: center !important;
}



div.dataTables_scrollBody::-webkit-scrollbar-track {
  background: rgba(33, 122, 244, .1);
}

table.dataTable th {
  text-align: center;
  vertical-align: middle !important;
  font-size: .8rem;
  padding: .3rem;
}

table.dataTable td {
  font-size: .8rem;
  padding: .25rem;
  vertical-align: middle !important;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  min-width: 10px;
}


/* 페이징 : 좌상단 */
div.dataTables_wrapper div.dataTables_length label {
  font-size: .9rem;
}

/* 페이징 : 좌상단 xx개 선택부분*/
div.dataTables_wrapper div.dataTables_length select {
  height: 28px;
  border: 1px solid #eee;
  border-left: 3px solid;
  border-radius: 5px;
  margin: 5px;
  transition: border-color .5s ease-out;
}

/* 페이징: 우하단 1,3,4,5 표시 부분 */
div.dataTables_wrapper div.dataTables_paginate li {
  font-size: .9rem;
}

/* 전체 xxx개 표시 부분 */
div.dataTables_wrapper div.dataTables_info {
  font-size: .9rem;
}

/* 스크롤 */
div.dataTables_scrollBody::-webkit-scrollbar {
  width: 5px;
  height: 10px;
}

div.dataTables_scrollBody::-webkit-scrollbar-thumb {
  height: 10%;
  background: gray;
  border-radius: 5px;
}

</style>