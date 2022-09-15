<template>
    <v-container fluid>
        DashBoard<v-icon class="md settings"></v-icon>11<i class="fa-solid fa-disc-drive"></i>
        <v-btn @click="download">print</v-btn>
      <vue-pdf-embed :source="source" />
    </v-container>
</template>

<script>

import printJs from 'print-js'
import VuePdfEmbed from 'vue-pdf-embed/dist/vue2-pdf-embed'
import axios from 'axios';
export default {
    // eslint-disable-next-line vue/multi-word-component-names
    name: "DashBoard",
    components: {VuePdfEmbed},
    props: {},
    data: () => ({
      source:""
    }),
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
        print() {

        printJs({
        printable:'http://nas.neo365.net:8080/1.pdf',
         type:'pdf',
          showModal:true
        })
        },
      download() {

          console.log("asdadasd");

        axios({
          url: "http://localhost:8080/RIST/api/coa/createEstimate?reqResultNo=214133&userId=admin",
          method: 'GET', // 혹은 'POST'
          responseType: 'blob', // 응답 데이터 타입 정의
        }).then(
            _result => {
              if (_result.data !== null) {
                const blob = new Blob([_result.data]);
                const fileObjectUrl = window.URL.createObjectURL(blob);

                const link = document.createElement('a');
                link.href = fileObjectUrl;
                link.style.display = 'none';

                const disposition = _result.headers['content-disposition'];
                let fileName = decodeURI(disposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/)[1].replace(/['"]/g, ''));

                link.download = fileName + '.xlsx'; //extractDownloadFilename(_result); // 다운로드 파일 이름을 추출하는 함수

                // 링크를 body에 추가하고 강제로 click 이벤트를 발생시켜 파일 다운로드를 실행시킵니다.
                document.body.appendChild(link);
                link.click();
                link.remove();

                // 다운로드가 끝난 리소스(객체 URL)를 해제합니다.
                window.URL.revokeObjectURL(fileObjectUrl);
              }
            }
        );
      }
    },
};
</script>

<style scoped>
</style>