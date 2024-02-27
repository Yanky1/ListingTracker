<template lang="">
  <button
    :data-variant="variant"
    :class="[_class]"
    @onClick="$emit('click', $event)"
  >
    <slot />
  </button>
</template>

<script lang="ts">
import { PropType } from "vue"

export default {
  props: {
    color: {
      type: String as PropType<"primary" | "white" | "red">,
      default: "primary",
    },
    variant: { type: String, default: "filled" },
    class: String,
  },
  $emit: ["click"],
  computed: {
    _class() {
      let _class = [
        "flex items-center justify-center gap-[8px] transition duration-150 ease",
        "text-nowrap",
        "rounded",
        "px-[18px] py-2.5",
        "text-md font-semibold",
      ]
      if (this.color === "primary") {
        _class.push("hover:bg-opacity-80")
        if (this.variant === "outlined") {
          _class.push("bg-primary-200 text-neutral-1000")
          _class.push("ring-2 ring-inset ring-primary text-neutral-1000")
          _class.push("active:bg-primary-100")
        } else {
          // background
          _class.push("bg-primary text-white")
          // active state
          _class.push("active:bg-primary-800")
        }
      }

      if (this.color === "white") {
        _class.push("hover:bg-gray-200")
        _class.push("ring-1 ring-inset ring-gray-300 text-gray-700")
      }

      if (this.color === "red") {
        _class.push("text-red-500")
        _class.push("bg-red-100")
        _class.push("hover:bg-red-200")
        _class.push("ring-1 ring-inset ring-red-500")
      }

      if (this.class) {
        _class.push(this.class)
      }
      return _class
    },
  },
}
</script>

<style lang="scss">
.bg-primary {
  &[data-variant="outlined"] {
    path {
      @apply fill-neutral-700;
    }
  }

  path {
    @apply fill-white;
  }
}
</style>
