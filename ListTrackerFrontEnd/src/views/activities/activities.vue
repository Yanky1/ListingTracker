<template>
  <Layout>
    <Topbar>
      <div class="w-full max-w-[625px]">
        <SearchInput
          v-model="searchText"
          placeholder="Search categories by any keyword"
        />
      </div>
    </Topbar>

    <div class="max-w-[365px] py-7 mx-auto">
      <ActivityItem
        :key="activity.id"
        v-for="activity of activities"
        :activity="activity"
      />
    </div>
  </Layout>
</template>

<script lang="ts">
import Header from "../../components/header/header.vue"
import Layout from "../../components/layout/dash-layout/dash-layout.vue"
import ActivityItem, {
  ActivityType,
} from "../../components/activity/activity-item.vue"
import Topbar from "../../components/top-bar/top-bar.vue"
import SearchInput from "../../components/input/search-input.vue"
import { getLogUploaded } from "../../services/activityServices"
import moment from 'moment';
interface DataType {
  activities: ActivityType[]
  searchText: string
}

export default {
  components: {
    Header,
    Layout,
    ActivityItem,
    Topbar,
    SearchInput,
  },
  data(): DataType {
    return {
      activities: [
        {
          id: "1",
          type: "",
          description: "",
          updatedAt: "",
        },
        
      ],
      searchText: "",
    }
  },
  created: function () {
    this.getInitList();
  },
  methods: {
    async getInitList(){
      var res=await getLogUploaded();
      var data=res.data;
      this.activities=data.map((act:any)=>({
           id: act.id,
          type: act.logType,
          description: act.logDescription,
          updatedAt: moment(act.logDate).format('DD-MMM-YYYY'),

      }));
      console.log(res.data);
    }
  }
}
</script>
