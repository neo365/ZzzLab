<template>
  <z-container fluid="sm" tag="span">
    <z-row>
      <z-col>
        <z-button variant="primary" v-b-popover.hover="'I am popover content!'" title="Popover Title">primary</z-button>
        <z-button class="ms-1" variant="secondary">secondary</z-button>
        <z-button class="ms-1" variant="success">success</z-button>
        <z-button class="ms-1" variant="danger">danger</z-button>
        <z-button class="ms-1" variant="warning">warning</z-button>
        <z-button class="ms-1" variant="info">info</z-button>
        <z-button class="ms-1" variant="light">light</z-button>
        <z-button class="ms-1" variant="dark">dark</z-button>
        <z-button class="ms-1" variant="link">link</z-button>
      </z-col>
    </z-row>
    <br />
    <z-row>
      <z-col>
        <z-button variant="outline-primary">outline-primary</z-button>
        <z-button class="ms-1" variant="outline-secondary">outline-secondary</z-button>
        <z-button class="ms-1" variant="outline-success">outline-success</z-button>
        <z-button class="ms-1" variant="outline-danger">outline-danger</z-button>
        <z-button class="ms-1" variant="outline-warning">outline-warning</z-button>
        <z-button class="ms-1" variant="outline-info">outline-info</z-button>
        <z-button class="ms-1" variant="outline-light">outline-light</z-button>
        <z-button class="ms-1" variant="outline-dark">outline-dark</z-button>
        <z-button class="ms-1" variant="outline-link">outline-link</z-button>
      </z-col>
    </z-row>
    <br />
    <z-row>
      <z-col>
        <z-button variant="primary" squared>squared</z-button>
        <z-button class="ms-1" variant="secondary" pill>pill</z-button>
        <z-button class="ms-1" variant="success" pill icon="bi bi-bell">pill + icon</z-button>
        <z-button class="ms-1" variant="danger" pill icon="bi bi-bell" />
        <z-button tag="input" class="ms-1" variant="warning" value="input tag" />
        <z-button tag="a" class="ms-1" variant="info">a tag</z-button>
        <z-button class="ms-1" variant="light" small >small</z-button>
        <z-button class="ms-1" variant="dark" large>large</z-button>
        <z-button class="ms-1" variant="link" squared>link</z-button>
      </z-col>
    </z-row>
    <br />
    <z-row>
      <z-col>
        <z-button variant="primary" :loading="loading" icon="bi bi-bell">
          loading
        </z-button>
        <z-button variant="primary" class="ms-1" :loading="loading" icon="bi bi-bell" small>
          loading
        </z-button>
        <z-button variant="primary" class="ms-1" :loading="loading" icon="bi bi-bell" large>
          loading
        </z-button>
        <z-button variant="primary" class="ms-1" :loading="loading">
          loading
        </z-button>
        <z-button variant="primary" class="ms-1" icon="bi bi-bell" @click="loading = !loading">
          loading
        </z-button>
      </z-col>
    </z-row>
  </z-container>
</template>
<script>
import ZContainer from "@/components/bootstrap5/ZBContainer.vue";
export default {
  name: "ZLButtons",
  components: {ZContainer},
  props: {},
  data: () => ({
    loading : false
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
  methods: {
    init() {
    },
    testMsg(msg) {
      alert(msg);
    }
  },
};
</script>