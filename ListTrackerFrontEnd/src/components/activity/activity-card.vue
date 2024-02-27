<template lang="">
  <div
    :class="[
      'overflow-hidden rounded-md',
      'w-full max-w-[335px]',
      'border border-neutral-500',
      'h-[400px]',
      'flex flex-col',
    ]"
  >
    <div class="bg-[#636366] py-5 px-2.5">
      <span class="text-sm font-medium text-white">Activity Log</span>
    </div>

    <div class="px-2.5 pb-5 activity-list">
      <ActivityItem
        size="small"
        v-for="(activity, index) of activities"
        :activity="activity"
      />
    </div>
  </div>
</template>
<script lang="ts">
import { PropType } from "vue"
import ActivityItem, {
  ActivityType,
} from "../../components/activity/activity-item.vue"

export default {
  props: {
    activities: { type: Object as PropType<ActivityType> },
  },
  components: {
    ActivityItem,
  },
}
</script>

<style lang="scss">
.activity-list {
  @apply h-full;
  @apply overflow-auto;
}
</style>
