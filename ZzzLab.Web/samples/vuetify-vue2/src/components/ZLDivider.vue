<template>
  <div class="zl-divider" :class="{'zl-divider-vertical' : vertical,'zl-divider-horizontal' : vertical == false}"><!----></div>
</template>

<script>
export default {
  name: "zl-divider",
  props: {
    vertical: {
      type: Boolean,
      default: false
    }
  }
}
</script>

<style scoped>
.zl-divider {
  background-color: black;
  position: relative;
}

.zl-divider-horizontal {
  display: block;
  height: 1px;
  width: 100%;
  margin: 0;
}

.zl-divider-vertical {
  display: block;
  width: 1px;
  height: 100%;
  margin: 0;
}
</style>