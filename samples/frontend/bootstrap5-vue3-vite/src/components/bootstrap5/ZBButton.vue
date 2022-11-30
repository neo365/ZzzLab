<template>
  <component :is="computedTag"
             :class=computedClass
             :type=computedType class="btn z-button"
             :value="value"
             :disabled="computedDisabled"
             data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Disabled popover">
    <span v-if="loading == true" class="spinner-border me-1" :class="computedSpinerClass" role="status" aria-hidden="true"></span>
    <i v-if="icon != '' && loading == false"  :class="this.iconClass"></i>
    <slot />
  </component>
</template>

<script>
export default {
  name: "z-button",
  components: {},
  props: {
    tag: {type: String, default: 'button'},
    type: {type: String, default: 'button'},
    small: {type: Boolean, default: false},
    large: {type: Boolean, default: false},
    variant: {type: String, default: 'secondary'},
    icon: {type: String, default: ''},
    squared: {type: Boolean, default: false},
    pill: {type: Boolean, default: false},
    loading: {type: Boolean, default: false},
    value: {type: String, default: null},
  },
  data: () => ({}),
  computed: {
    isExistSlot: function () {
      return this.$slots.default ? true : false;
    },
    computedLoading: function () {
      console.log(this.loading)
      return this.loading;
    },
    computedDisabled: function () {
      if(this.loading) return true;

      return false;
    },
    computedTag: function () {
      if (this.tag) return this.tag;
      else return "button";
    },
    computedType: function () {
      if (this.type) return this.type;
      else return "button";
    },
    computedClass: function () {
      let btnClass = '';

      if (this.variant) btnClass += ' btn-' + (this.variant);

      if (this.large) btnClass += ' btn-lg';
      else if (this.small) btnClass += ' btn-sm';
      else btnClass += ' btn-md';

      if (this.squared) btnClass += ' is-squared';
      else if (this.pill) btnClass += this.$slots.default ? ' is-round' : ' is-circle';

      return btnClass;
    },
    iconClass: function () {
      return this.$props.icon + (this.$slots.default ? ' pe-1' : '');
    },
    computedSpinerClass: function() {
      let style = '';
      if (this.large) style += ' spinner-border-lg';
      else if (this.small) style += ' spinner-border-xs';
      else style += ' spinner-border-sm';

      return style;
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

.z-button.is-squared {
  border-radius: 0;
}

.spinner-border-xs {
  --bs-spinner-width: var(--bs-btn-font-size);
  --bs-spinner-height: var(--bs-spinner-width);
  --bs-spinner-border-width:calc(var(--bs-spinner-width)/5);
}

.spinner-border-lg {
  --bs-spinner-width: var(--bs-btn-font-size);
  --bs-spinner-height: var(--bs-spinner-width);
  --bs-spinner-border-width:calc(var(--bs-spinner-width)/5);
}
</style>