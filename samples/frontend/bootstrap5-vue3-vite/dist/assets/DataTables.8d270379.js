import{_ as r,f as s,D as i,r as l,o as d,c,d as u,g as a,F as p}from"./index.4d0a1238.js";const m={name:"DataTables",components:{DataTable:s},props:{},data:()=>({options:{},headers:[{title:"a",data:"name"},{title:"b",data:"position"},{title:"c",data:"office"},{title:"d",data:"extn"},{title:"e",data:"start_date"},{title:"f",data:"salary"}],items:[]}),computed:{},watch:{},beforeCreate(){},created(){},beforeMount(){s.use(i),this.init()},mounted(){},beforeUpdate(){},updated(){},methods:{init(){let t={processing:!0,serverSide:!1,searching:!1,ordering:!!this.headers.find(e=>e.orderable),paging:!0,pageLength:30,lengthMenu:[10,20,30,40,50,60,70,80,90,100,200,300,400,500,1e3],autoWidth:!1,scrollY:"calc(100vh - 290px)",scrollX:!0,stateSave:!0,stateDuration:86400,colReorder:!0,colResize:{isEnabled:!0,hoverClass:"dt-colresizable-hover",hasBoundCheck:!0,minBoundClass:"dt-colresizable-bound-min",maxBoundClass:"dt-colresizable-bound-max",isResizable:function(e){return!0},onResize:function(e){},onResizeEnd:function(e,o){},getMinWidthOf:function(e){}},rowReorder:!0};this.options=t,this.$http({url:"http://localhost:8081/data.json",method:"GET"}).then(e=>{e.data!==null&&(this.items=e.data.data)})},testMsg(t){alert(t)}}};function h(t,e,o,b,f,g){const n=l("z-datatables");return d(),c(p,null,[u(n,{id:"3",headers:t.headers,items:t.items,options:t.options,class:"table-bordered table-condensed nowrap"},null,8,["headers","items","options"]),a("  <DataTable"),a('      :columns="headers"'),a('      :options="options"'),a('      :data="items"'),a('      class="table table-hover table-striped"'),a("  />")],2112)}const v=r(m,[["render",h],["__file","C:/Workspace/Project/RIST/src/extra/RistService/Frontend/src/views/Samples/DataTables.vue"]]);export{v as default};
