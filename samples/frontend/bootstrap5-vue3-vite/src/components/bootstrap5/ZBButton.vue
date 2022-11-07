<template>
  <button :class=mainClass :type=mainType>
    <i v-if="icon != ''" :class="this.iconClass"></i>
    <slot/>
  </button>
</template>

<script>
export default {
  name: "z-button",
  components: {},
  props: {
    type: {type: String, default: ''},
    submit: {type: Boolean, default: false},
    reset: {type: Boolean, default: false},
    size: {type: String, default: 'small'},
    icon: {type: String, default: ''},
    class: {type: String, default: ''},
    outline: {type: Boolean, default: false},
    round: {type: Boolean, default: false},
  },
  data: () => ({}),
  computed: {
    isExistSlot: function () {
      return this.$slots.default ? true : false;
    },
    mainType: function () {
      if (this.submit) return "submit";
      else if (this.reset) return "reset";
      else return "button";
    },
    mainClass: function () {

      let btnClass = 'btn z-button';

      switch (this.type) {
        case "primary":
        case "secondary":
        case "success":
        case "danger":
        case "warning":
        case "info":
        case "light":
        case "dark":
        case "link":
          btnClass += ' btn-' + (this.outline ? 'outline-' : '') + this.type;
          break;

        default:
          break;
      }

      switch (this.size) {
        case "small":
          btnClass += ' btn-sm';
          break;

        case "large":
          btnClass += ' btn-lg';
          break;

        default:
          break;
      }

      if (this.round) btnClass += this.$slots.default ? ' is-round' : ' is-circle';

      return this.class + ' ' + btnClass;
    },
    iconClass: function () {
      return this.$props.icon + (this.$slots.default ? ' pe-1' : '');
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
  methods: {
    init() {
    }
  },
};
</script>

<style scoped>
.z-button.is-circle {
  border-radius: 50%;
}

.z-button.is-round {
  border-radius: var(--z-border-radius-round);
}
</style>