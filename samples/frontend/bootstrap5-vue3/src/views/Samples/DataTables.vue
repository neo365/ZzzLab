<template>
  <DataTable
      ajax="/data.json"
      class="table table-hover table-striped"
      width="100%"
  >
    <thead>
    <tr>
      <th>Name</th>
      <th>Position</th>
      <th>Office</th>
      <th>Extn.</th>
      <th>Start date</th>
      <th>Salary</th>
    </tr>
    </thead>
    <tfoot>
    <tr>
      <th>Name</th>
      <th>Position</th>
      <th>Office</th>
      <th>Extn.</th>
      <th>Start date</th>
      <th>Salary</th>
    </tr>
    </tfoot>
  </DataTable>
</template>

<script>
import DataTable from 'datatables.net-vue3'
// import DataTableBs5 from 'datatables.net-bs5'
// DataTable.use(DataTableBs5);

export default {
  name: "DataTables",
  components: {
    DataTable
  },
}
</script>

<style scoped>

</style>