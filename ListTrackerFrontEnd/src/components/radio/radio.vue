<template lang="">
  <div class="flex gap-1">
    <div class="flex relative items-center justify-center">
      <input
        :id="id"
        :name="name"
        v-model="value"
        type="radio"
        class="peer cursor-pointer appearance-none relative h-4 w-4 bg-white border border-black border-opacity-[0.14] transition-all checked:border-primary checked:bg-primary rounded-full"
      />

      <div
        :class="[
          'pointer-events-none  text-white',
          'absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2',
          'opacity-0 transition-opacity peer-checked:opacity-100 peer-checked:border border-primary',
        ]"
      >
        <span class="w-2 h-2 bg-white block rounded-full" />
      </div>
    </div>
    <label
      v-if="label"
      :for="id"
      class="relative flex cursor-pointer items-center rounded gap-2"
    >
      <div v-html="label"></div>
    </label>
  </div>
</template>

<script lang="ts">
import { v4 as uuidv4 } from "uuid"

export default {
  components: {},
  props: {
    name: { type: String, default: "" },
    id: { type: String, default: uuidv4() },
    label: { type: String, default: "" },
    modelValue: { type: String, default: "" },
  },
  emits: ["update:modelValue"],
  computed: {
    value: {
      get() {
        return this.modelValue
      },
      set(value: any) {
        this.$emit("update:modelValue", value)
      },
    },
  },
}
</script>
<style lang=""></style>
