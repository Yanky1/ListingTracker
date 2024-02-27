<template lang="">
  <div class="flex gap-6">
    <div class="left-bar">
      <BaseIcon name="circleCheckbox" />
    </div>
    <div class="flex flex-col gap-2 mt-7">
      <div :class="['font-semibold', textSize]">
        <span class="text-[#226BFF]">{{ activity.type }}</span>
        {{ activity.description }}
      </div>
      <div :class="['text-sm font-normal text-[#636366]', timeStepSize]">
        {{ activity.updatedAt }}
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import BaseIcon from "../icons/base-icon.vue"
import { PropType } from "vue"

export interface ActivityType {
  id: string
  type: string
  description: String
  updatedAt: String
}
export default {
  components: {
    BaseIcon,
  },
  computed: {
    textSize() {
      if (this.size === "small") {
        return "text-xs"
      }

      return "text-md"
    },
    timeStepSize() {
      if (this.size === "small") {
        return "text-xs"
      }

      return "text-sm"
    },
  },
  props: {
    size: { type: String as PropType<"small" | "medium">, default: "medium" },
    activity: { type: Object as PropType<ActivityType>, required: true },
  },
}
</script>
<style lang="scss">
.left-bar {
  @apply relative;
  @apply flex;
  @apply items-end;

  svg {
    @apply z-[2];
    @apply relative;
  }
  &::before {
    content: "";
    @apply h-full;
    @apply absolute;
    @apply left-0;
    @apply right-0;
    @apply w-[1px];
    @apply bg-[#DDD];
    @apply mx-auto;
    @apply z-[1];
  }
}
</style>
