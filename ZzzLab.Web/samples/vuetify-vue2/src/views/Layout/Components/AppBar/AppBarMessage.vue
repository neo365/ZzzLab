<template>
  <v-menu bottom offset-y right>
    <template v-slot:activator="{ on, attrs }">
      <v-btn icon v-bind="attrs" v-on="on">
        <v-badge color="red" content="99+" overlap>
          <v-icon>mdi-message</v-icon>
        </v-badge>
      </v-btn>
    </template>
    <v-card max-width="600">
      <v-list>
        <v-list-item>
          <v-list-item-icon>
            <v-icon>mdi-alert-circle</v-icon>
          </v-list-item-icon>

          <v-list-item-title>읽지 않은 10개의 메세지가 있습니다.</v-list-item-title>


        </v-list-item>
      </v-list>
      <v-divider inset/>
      <v-list two-line>
        <v-list-item-group
            active-class="pink--text"
        >
          <template v-for="(item, index) in items">
            <v-list-item :key="item.title">
              <template v-slot:default>
                <v-list-item-avatar>
                  <img alt="John" src="https://cdn.vuetifyjs.com/images/lists/1.jpg"/>
                </v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title v-text="item.title"></v-list-item-title>
                  <v-list-item-subtitle v-text="item.subtitle"></v-list-item-subtitle>
                </v-list-item-content>

                <v-list-item-action>
                  <v-list-item-action-text v-text="item.action"></v-list-item-action-text>

                </v-list-item-action>
              </template>
            </v-list-item>
            <v-divider
                v-if="index < items.length - 1"
                :key="index"
                inset
            ></v-divider>
          </template>
        </v-list-item-group>
      </v-list>
    </v-card>
  </v-menu>
</template>

<script>
export default {
  name: "AppBarAlarm",
  data: () => ({
    items: [
      {
        action: '15 min',
        headline: 'Brunch this weekend?',
        subtitle: `I'll be in your neighborhood doing errands this weekend. Do you want to hang out?`,
        title: 'Ali Connors',
      },
      {
        action: '2 hr',
        headline: 'Summer BBQ',
        subtitle: `Wish I could come, but I'm out of town this weekend.`,
        title: 'me, Scrott, Jennifer',
      },
      {
        action: '6 hr',
        headline: 'Oui oui',
        subtitle: 'Do you have Paris recommendations? Have you ever been?',
        title: 'Sandra Adams',
      },
      {
        action: '12 hr',
        headline: 'Birthday gift',
        subtitle: 'Have any ideas about what we should get Heidi for her birthday?',
        title: 'Trevor Hansen',
      },
      {
        action: '18hr',
        headline: 'Recipe to try',
        subtitle: 'We should eat this: Grate, Squash, Corn, and tomatillo Tacos.',
        title: 'Britta Holt',
      },
    ],
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
</style>
