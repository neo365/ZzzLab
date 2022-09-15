<template>
  <v-navigation-drawer :mini-variant.sync="sidebarStatus" app permanent>
    <template v-slot:prepend>
      <v-list height="60">
        <v-list-item>
          <v-list-item-action @click.stop="sidebarStatus = !sidebarStatus">
            <v-icon :class='{ "rotate": sidebarStatus }' class="toggleRotateIcon">mdi-menu-open</v-icon>
          </v-list-item-action>
          <v-list-item-title>
            <strong>Devtools</strong>
          </v-list-item-title>
        </v-list-item>
      </v-list>
      <v-divider/>
    </template>
    <v-list v-model="menuSelect" dense>
<!--      <v-list-item-group color="red">-->
        <MenuItem :items="menuItems"/>
<!--      </v-list-item-group>-->
    </v-list>
  </v-navigation-drawer>
</template>
<script>
import MenuItem from "./MenuItems";

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Navigation",
  components: {
    MenuItem
  },
  props: {},
  data: () => ({
    menuSelected: 0,
    menuItems: [
      {
        icon: "mdi-view-dashboard",
        title: "Dashboard",
        link: "/Dashboard"
      },
      {
        icon: "mdi-robot",
        title: "시스템",
        children: [
          {
            title: "자동화",
            children: [
              {
                icon: "mdi-clock-outline",
                title: "스케쥴",
                link: "/system/Automation/Scheduler/List"
              }
            ]
          },
          {
            title: "상태",
            children: [
              {
                icon: "mdi-human-queue",
                title: "접속현황",
              },
              {
                icon: "fa-solid fa-hard-drive",
                title: "스토리지 사용현황",
              },
            ]
          }

        ]
      },
      {
        icon: "mdi-ladybug",
        title: "디버깅",
        children: [
          {
            icon: "mdi-database",
            title: "데이터베이스",
            children: [
              {
                icon: "mdi-table-large",
                title: "테이블 목록",
                link: "/Debug/DataBase/TableList"
              },
              {
                icon: "mdi-table-eye-off",
                title: "테이블보기",
              },
              {
                icon: "mdi-file-excel",
                title: "테이블 명세서",
              }
            ]
          },
          {
            //icon: "mdi-compare",
            icon: null,
            title: "파일비교",
            children: [
              {
                icon: "fa-solid fa-file-pdf",
                title: "PDF",
              },
              {
                icon: "mdi-image",
                title: "이미지",
              }
            ]
          },
          {
            icon: "fa-solid fa-arrows-rotate",
            title: "파일변환",
            children: [
              {
                icon: "fa-solid fa-file-pdf",
                title: "PDF",
              },
              {
                icon: "mdi-web",
                title: "URL to PDF",
              }
              ,
              {
                icon: "mdi-xml",
                title: "HTML to PDF",
              }
            ]
          }
        ]
      },
      {
        icon: "mdi-human-male-male",
        title: "사용자",
        children: [
          {
            icon: "mdi-domain",
            title: "회사",
          },
          {
            icon: "mdi-graph",
            title: "부서",
          },
          {
            icon: "mdi-account",
            title: "사용자",
          }
        ]
      },

      {
        icon: "mdi-cog",
        title: "설정",
        children: [
          {
            title: "데이터베이스",
            children: [
              {
                icon: "mdi-connection",
                title: "연결설정",
                link: "/Settings/Database/Config/List",
              },
              {
                icon: "mdi-file-document-outline",
                title: "쿼리설정",
                link: "/Settings/Database/Query/List"
              }
            ]
          },
          {
            title: "권한관리",
            children: [
              {
                icon: "mdi-account-multiple",
                title: "그룹",
              },
              {
                icon: "mdi-menu",
                title: "메뉴권한",
              }
            ]
          },

        ]
      },
    ],
  }),
  computed: {
    sidebarStatus: {
      get: function () {
        return !this.$store.getters.sidebarOnOff;
      },
      set: function (onoff) {
        this.$store.commit('SET_SIDEBAR_ONOFF', !onoff)
      }
    },
    menuSelect: {
      get: function () {
        return this.menuSelected;
      },
      set: function (value) {
        console.log(("menuSelect: " + value))
        this.menuSelected = value;
      }
    }
  },
  watch: {
    $route(to, from) {
      console.log(to)
      console.log(from)
    }
  },
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
  beforeDestroy() {
  },
  destroyed() {
  },
  methods: {
    init() {
    },
  },
};
</script>

<style scoped>
.toggleRotateIcon {
  transition: transform .3s ease-in-out !important;
}

.toggleRotateIcon.rotate {
  transform: rotate(180deg);
}

</style>