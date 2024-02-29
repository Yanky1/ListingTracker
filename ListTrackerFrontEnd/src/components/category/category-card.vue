<template lang="">
  <div
    class="rounded border border-[#B7B7B7] w-[250px] overflow-hidden cursor-pointer"
  >
    <div class="flex items-center justify-center bg-white">
      <img
        src="/src/assets/images/xlsx-red.png"
        :alt="name"
        class="w-[134px]"
      />
    </div>
    <div
      class="flex gap-3 bg-[#F3F6FF] border-t border-neutral-500 items-center justify-between px-2 h-[60px]"
    >
      &nbsp;<span :title="name" class="text-md font-semibold line-clamp-2">{{
      ' '+  name
      }}</span>
      <!-- Display the level field -->
      <span :title="'Level ' + level" class="text-md font-semibold">{{ 'Level: ' + level }}</span>
      <div class="flex gap-4 items-center">
        <span title="edit" @click="handleEdit($event, id)"
          ><Icon name="pencil" class="text-black cursor-pointer"
        /></span>
        <span title="remove" @click="handleRemove($event, id)"
          ><Icon name="trash" class="text-[#FF0000] cursor-pointer"
        /></span>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Icon from "../icons/base-icon.vue"

export default {
  components: {
    Icon,
  },
  $emit: ["edit", "remove"],
  props: {
    name: String,
    level:Number,
    id: { type: String, required: true },
  },
  methods: {
    handleEdit(e: Event, id: string) {
      e.stopPropagation()
      this.$emit("edit", id)
    },
    handleRemove(e: Event, id: string) {
      e.stopPropagation()
      this.$emit("remove", id)
    },
  },
}
</script>
<style lang=""></style>
