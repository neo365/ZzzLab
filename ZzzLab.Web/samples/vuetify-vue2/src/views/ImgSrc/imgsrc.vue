<template>
    <v-container fluid>
        {{ $route.params.tableName }}
    </v-container>
</template>

<script>
export default {
    // eslint-disable-next-line vue/multi-word-component-names
    name: "ImgSrc",
    components: {},
    props: {},
    data: () => ({}),
    computed: {},
    watch: {},
    beforeCreate() { },
    created() { },
    beforeMount() {
        this.init();
    },
    mounted() { },
    beforeUpdate() { },
    updated() { },
    beforeDestroy() { },
    destroyed() { },
    methods: {
        init() {
            this.$root.$emit("setDocTitle", "대쉬보드")
        },
    },
};
</script>

<style scoped>
</style>