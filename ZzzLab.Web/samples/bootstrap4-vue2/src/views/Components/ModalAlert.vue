<template>
  <b-modal id="global-modal-alert" v-model="visible" :title="title">
    <p>{{ message }}</p>
    <template #modal-footer>
      <div class="btn-wrap">
        <b-button variant="danger" @click="confirm">{{ buttonName }}</b-button>
      </div>
    </template>
  </b-modal>
</template>

<script>

export default {
  name: 'ModalAlert',
  props: {
    title: {type: String, default: '알림'},
    message: {type: String, default: ''},
    buttonName: {type: String, default: '확인'}
  },
  data: () => ({
    visible: false,
  }),
  computed: {},
  watch: {},
  beforeCreate() {
  },
  created() {
    // 자신의 처리
    this.$on('open', function () {
      this.visible = true;
      this.$bvModal.show('global-modal-alert');
    });

    this.$on('close', function () {
      this.visible = false;
      this.$bvModal.hide('global-modal-alert');
    })
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
    this.$off('open');
    this.$off('close');
  },
  methods: {
    init() {
      this.visible = false;
    },
    confirm() {
      this.visible = false;
      this.$bvModal.hide('global-modal-alert');
      this.$emit('click-confirm');  // 확인 버튼 클릭 후 이벤트 처리
    }
  }
}
</script>