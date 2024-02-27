<template>
  <Menu as="div" class="relative inline-block text-left">
    <div>
      <MenuButton :class="menuButtonClass">
        {{ label }}
        <span v-if="!hideIcon">
          <BaseIcon :name="icon" :class="`min-w-[15px] ${iconClass}`" />
        </span>
      </MenuButton>
    </div>

    <transition
      enter-active-class="transition ease-out duration-100"
      enter-from-class="transform opacity-0 scale-95"
      enter-to-class="transform opacity-100 scale-100"
      leave-active-class="transition ease-in duration-75"
      leave-from-class="transform opacity-100 scale-100"
      leave-to-class="transform opacity-0 scale-95"
    >
      <MenuItems :class="menuItemListClass">
        <MenuItem
          class="py-2.5 px-4"
          :key="item.value"
          v-for="item of items"
          active="name"
          v-slot="{ active }"
          @click="($event: Event) => handleItemClick($event, item)"
        >
          <span
            :class="[
              active ? 'bg-gray-100' : 'text-gray-300',
              'block px-4 py-2 cursor-pointer text-md',
              modelValue === item.value ? 'text-gray-900 bg-gray-100' : '',
            ]"
            >{{ item.label }}</span
          >
        </MenuItem>
      </MenuItems>
    </transition>
  </Menu>
</template>

<script lang="ts">
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/vue"
import ChevronDownIcon from "../icons/icon-chevron-down.vue"
import type { PropType } from "vue"
import BaseIcon from "../icons/base-icon.vue"

export interface ItemType {
  value: string
  label: string
}

export default {
  components: {
    Menu,
    MenuButton,
    MenuItem,
    MenuItems,
    ChevronDownIcon,
    BaseIcon,
  },
  computed: {
    menuButtonClass() {
      return [
        `inline-flex`,
        `w-full`,
        `text-nowrap`,
        `justify-center`,
        `items-center`,
        `bg-primary-100`,
        `gap-2`,
        `p-[18px]`,
        `text-md`,
        `text-neutral-1000`,
        `ring-1 ring-inset ring-neutral-700`,
        `hover:bg-[#F0F0F0]`,
        `${this.rounded ? "rounded-full" : "rounded"}`,
      ]
    },
    menuItemListClass() {
      return `absolute
              py-[14px]
              left-0
              z-10
              mt-2
              w-56
              flex
              flex-col
              gap-2
              origin-top-left
              rounded-md
              text-md
              text-gray-300
              bg-white
              ring-1 ring-[#DFDFE0]
              focus:outline-none
              `
    },
  },
  props: {
    modelValue: String,
    icon: { type: String, default: "chevron-down" },
    iconClass: { type: String, default: "" },
    hideIcon: { type: Boolean, default: false },
    items: { type: Object as PropType<ItemType[]>, required: true },
    label: { type: String, required: true },
    rounded: Boolean,
  },
  emits: ["update:modelValue"],
  methods: {
    handleItemClick(_e: Event, item: ItemType) {
      if (this.modelValue === item.value) return

      this.$emit("update:modelValue", item.value)
    },
  },
}
</script>
<style lang=""></style>
