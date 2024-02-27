<template lang="">
  <div :class="_class">
    <BaseIcon :name="_icon" :class="iconClass" />

    <slot />
  </div>
</template>

<script lang="ts">
import type { PropType } from "vue"
import BaseIcon from "../icons/base-icon.vue"

export default {
  components: {
    BaseIcon,
  },
  props: {
    class: { type: String, default: "" },
    color: {
      type: String as PropType<"warning" | "primary">,
      default: "primary",
    },
    icon: String,
  },

  computed: {
    _class() {
      let _class = [
        "py-[12px] px-2 text-md",
        "flex items-center gap-2",
        "rounded",
        "ring-1 ring-inset ring-transparent",
      ]

      if (this.color === "primary") {
        _class.push("bg-primary text-white")
      }

      if (this.color === "warning") {
        _class.push("bg-orange-100 text-orange-500 ring-orange-500")
      }

      if (this.class) {
        _class.push(this.class)
      }

      return _class
    },
    _icon() {
      return this.icon
    },
    iconClass() {
      if (this.color === "primary") {
        return "text-white"
      }

      if (this.color === "warning") {
        return "text-orange-500"
      }
    },
  },
}
</script>
<style lang=""></style>
