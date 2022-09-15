<script>
/* eslint-disable */
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Redirect",
  beforeCreate() {
    const {params, query} = this.$route
    const {path} = params
    this.$router.replace({path: '/' + path, query})
  },
  render: function (h) {
    return h() // avoid warning message
  }
}
</script>
