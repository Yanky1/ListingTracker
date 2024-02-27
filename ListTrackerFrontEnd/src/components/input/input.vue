<template lang="">
  <div :class="['flex items-center', 'gap-1.5', fullWidth ? 'w-full' : '']">
    <label
      v-show="Boolean(label)"
      for="price"
      class="block text-sm font-semibold text-gray-700"
      >{{ label }}</label
    >
    <div :class="['relative', fullWidth ? 'w-full' : '']">
      <div
        v-if="Boolean(startAdornment)"
        class="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3 w-6"
      >
        <Icon
          v-if="startAdornment.type === 'icon'"
          :name="startAdornment.name"
        />
        <div v-else>
          {{ startAdornment.name }}
        </div>
      </div>
      <input
        :type="type"
        :name="name"
        :id="id"
        :class="[
          'block',
          'w-full',
          'rounded-[8px]',
          'border-0',
          'py-[12px] px-[14px]',
          startAdornment ? 'pl-11' : '',
          'transition duration-300',
          'text-lg text-black leading-6',
          'ring-1 ring-inset ring-gray-300',
          'placeholder:text-gray-500',
          'outline-none',
          'focus:ring-1 focus:ring-inset focus:ring-purple-500',
          'sm:text-sm',
        ]"
        v-model="value"
        :placeholder="placeholder"
      />
    </div>
  </div>
</template>

<script lang="ts">
import type { PropType } from "vue"
import Icon from "../icons/base-icon.vue"

export interface AdornmentType {
  type: string
  name: string
}

export default {
  components: {
    Icon,
  },
  props: {
    fullWidth: { type: Boolean, default: false },
    label: { type: String, default: "" },
    name: { type: String, default: "" },
    id: { type: String, default: "" },
    type: { type: String, default: "text" },
    placeholder: { type: String, default: "" },
    modelValue: { type: String, default: "" },
    startAdornment: Object as PropType<AdornmentType>,
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
