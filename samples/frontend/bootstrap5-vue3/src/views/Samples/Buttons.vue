<template>
  <div>
    <div class="row">
      <div class="col">
        <button class="btn btn-primary btn-sm" type="button">Primary</button>
        <button class="btn btn-secondary btn-sm" type="button">Secondary</button>
        <button class="btn btn-success btn-sm" type="button">Success</button>
        <button class="btn btn-danger btn-sm" type="button">Danger</button>
        <button class="btn btn-warning btn-sm" type="button">Warning</button>
        <button class="btn btn-info btn-sm" type="button">Info</button>
        <button class="btn btn-light btn-sm" type="button">Light</button>
        <button class="btn btn-dark btn-sm" type="button">Dark</button>
        <button class="btn btn-link btn-sm" type="button">Link</button>
      </div>
    </div>
    <el-row>
      <el-button>Default</el-button>
      <el-button type="primary">Primary</el-button>
      <el-button type="success">Success</el-button>
      <el-button type="info">Info</el-button>
      <el-button type="warning">Warning</el-button>
      <el-button type="danger">Danger</el-button>
    </el-row>
  </div>
  <div class="row">
    <div class="col">
      <z-button type="primary">primary</z-button>
      <z-button class="ms-1" type="secondary">secondary</z-button>
      <z-button class="ms-1" type="success">success</z-button>
      <z-button class="ms-1" type="danger">danger</z-button>
      <z-button class="ms-1" type="warning">warning</z-button>
      <z-button class="ms-1" type="info">info</z-button>
      <z-button class="ms-1" type="light">light</z-button>
      <z-button class="ms-1" type="dark">dark</z-button>
      <z-button class="ms-1" type="link">link</z-button>
    </div>
  </div>
  <z-row>
    <z-col>
      <z-button outline type="primary">primary</z-button>
      <z-button class="ms-1" outline type="secondary">secondary</z-button>
      <z-button class="ms-1" outline type="success">success</z-button>
      <z-button class="ms-1" outline type="danger">danger</z-button>
      <z-button class="ms-1" outline type="warning">warning</z-button>
      <z-button class="ms-1" outline type="info">info</z-button>
      <z-button class="ms-1" outline type="light">light</z-button>
      <z-button class="ms-1" outline type="dark">dark</z-button>
      <z-button class="ms-1" outline type="link">link</z-button>
    </z-col>
  </z-row>
  <div class="row">
    <div class="col">
      <z-button disabled icon="fa-solid fa-flask" round type="primary">test</z-button>
      <z-button class="ms-1" outline size="large" type="success">test2</z-button>
      <z-button class="ms-1" icon="bi bi-9-square" round type="info" @click="testMsg('z-button')"/>
    </div>
  </div>
</template>
<script>
export default {
  name: "ZLButtons",
  components: {},
  props: {},
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
  beforeUpdate() {
  },
  updated() {
  },
  methods: {
    init() {
    },
    testMsg(msg) {
      alert(msg);
    }
  },
};
</script>