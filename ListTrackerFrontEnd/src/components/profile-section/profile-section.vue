<template lang="">
  <div class="flex gap-2">
    <img
      src="/src/assets/images/avatar.png"
      class="w-20 min-w-20 h-20"
      :alt="record.first_name"
    />
    <div>
      <div class="text-black text-md font-bold">
        {{ `${record.first_name} ${record.last_name}` }}
      </div>
      <div class="text-black text-xs mt-3">
        {{ record.address }}
      </div>
      <div class="text-black text-xs mt-2 flex gap-1">
        <!-- <span> Age: {{ record.age }} | </span>
        <span> Gender: {{ record.gender }} | </span>
        <span> Class: {{ record.class }} </span> -->
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { RecordType } from "../../types/record"
import { PropType } from "vue"

export default {
  props: {
    record: { type: Object as PropType<RecordType>, required: true },
  },
}
</script>

<style lang=""></style>
