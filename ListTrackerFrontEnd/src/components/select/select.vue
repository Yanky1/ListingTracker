<template>
  <Listbox :id="id" as="div" v-model="value">
    <div class="relative mt-2">
      <ListboxButton
        class="relative w-full cursor-default rounded-md bg-white py-3 pl-3 pr-10 text-left text-black shadow-sm ring-1 ring-inset ring-gray-300 focus:outline-none focus:ring-1 focus:ring-primary sm:text-sm sm:leading-6"
      >
        <span class="flex items-center">
          <span class="block truncate">{{ value?.label ?? placeholder }}</span>
        </span>
        <span
          class="pointer-events-none absolute inset-y-0 right-0 ml-3 flex items-center pr-2"
        >
          <Icon name="chevronUpDown" class="min-w-[15px] min-h-[15px]" />
        </span>
      </ListboxButton>

      <transition
        leave-active-class="transition ease-in duration-100"
        leave-from-class="opacity-100"
        leave-to-class="opacity-0"
      >
        <ListboxOptions
          class="absolute z-10 mt-1 max-h-56 w-full overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm"
        >
          <ListboxOption
            as="template"
            v-for="item in items"
            :key="item.value"
            :value="item"
            v-slot="{ active, selected }"
          >
            <li
              :class="[
                active ? 'bg-primary text-white' : 'text-black',
                'relative cursor-default select-none py-3 pl-3 pr-9',
              ]"
            >
              <div class="flex items-center">
                <span
                  :class="[
                    selected ? 'font-semibold' : 'font-normal',
                    'block truncate',
                  ]"
                  >{{ item.label }}</span
                >
              </div>

              <span
                v-if="selected"
                :class="[
                  active ? 'text-white' : 'text-primary',
                  'absolute inset-y-0 right-0 flex items-center pr-4',
                ]"
              >
                <Icon name="checkbox" class="h-5 w-5" aria-hidden="true" />
              </span>
            </li>
          </ListboxOption>
        </ListboxOptions>
      </transition>
    </div>
  </Listbox>
</template>

<script lang="ts">
import {
  Listbox,
  ListboxButton,
  ListboxOption,
  ListboxOptions,
} from "@headlessui/vue"
import { PropType } from "vue"
import Icon from "../icons/base-icon.vue"

export interface ItemType {
  value: string
  label: string
}

export default {
  components: {
    Icon,
    Listbox,
    ListboxOption,
    ListboxOptions,
    ListboxButton,
  },
  props: {
    modelValue: { type: String, default: "" },
    items: {
      type: Object as PropType<ItemType[]>,
      required: true,
    },
    id: String,
    placeholder: { type: String, default: "Select" },
  },
  emits: ["update:modelValue"],
  computed: {
    value: {
      get() {
        return this.items.find(
          (item: ItemType) => item.value === this.modelValue
        )
      },
      set(value: any) {
        this.$emit("update:modelValue", value.value)
      },
    },
  },
}
</script>
