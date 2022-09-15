<template>
  <div class="text-center">
    <v-dialog v-model="visible" width="500">
      <v-card>
        <v-card-title class="text-h5 grey lighten-2">
          {{ title }}
        </v-card-title>

        <v-card-text>
          <p>{{ message }}</p>
        </v-card-text>

        <v-divider></v-divider>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn depressed dark color="error" @click="confirm">
            {{ buttonName }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

export default {
  name: 'ModalAlert',
  props: {
    title: { type: String, default: '알림' },
    message: { type: String, default: '' },
    buttonName: { type: String, default: '확인' }
  },
  data: () => ({
    visible: false,
  }),
  computed: {},
  watch: {},
  beforeCreate() { },
  created() {
    // 자신의 처리
    this.$on('open', function () {
      this.visible = true;
    });

    this.$on('close', function () {
      this.visible = false;
    })
  },
  beforeMount() {
    this.init();
  },
  mounted() { },
  beforeUpdate() { },
  updated() { },
  beforeDestroy() { },
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
      this.$emit('click-confirm');  // 확인 버튼 클릭 후 이벤트 처리
    }
  }
}
</script>