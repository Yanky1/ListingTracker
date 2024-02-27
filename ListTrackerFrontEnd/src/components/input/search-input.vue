<template lang="">
  <Input
    v-model="value"
    :fullWidth="true"
    :startAdornment="startAdornment"
    :placeholder="placeholder"
  />
</template>

<script lang="ts">
import { AdornmentType } from "./input.vue"
import Input from "./input.vue"

interface DataType {
  startAdornment: AdornmentType
}

export default {
  components: {
    Input,
  },
  props: {
    placeholder: { type: String, default: "" },
    modelValue: { type: String, default: "" },
  },
  emits: ["update:modelValue"],
  data(): DataType {
    return {
      startAdornment: { type: "icon", name: "search" },
    }
  },
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
