<template lang="">
  <div class="text-black">
    <div class="flex gap-1 items-center">
      <Icon
        name="file-open"
        class="min-w-[15px] w-[15px] min-h-[15px] h-[15px] text-primary-950"
      />
      <div class="text-xs">Record sources</div>
    </div>
    <div class="mt-[5px] flex flex-col gap-1">
      <div
        v-for="(source, index) of sources"
        class="text-xxs"
        :key="`source-${index}`"
      >
        {{ source }}
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import Icon from "../../components/icons/base-icon.vue"

export default {
  components: {
    Icon,
  },
  props: {
    sources: { type: Array<String[]>, required: true },
  },
}
</script>
<style lang=""></style>
