<template lang="">
  <div class="flex gap-2 items-center">
    <template class="w-full" v-for="(action, index) of tableActions">
      <template v-if="action.type === 'button'">
        <Button
          :class="action.class"
          :color="action.color"
          @click="$emit('click', action)"
        >
          <template v-if="action.icon">
            <Icon :name="action.icon" />
          </template>

          <template v-if="action.label">
            {{ action.label }}
          </template>
        </Button>
      </template>
      <template v-if="action.type === 'iconButton'">
        <span
          v-if="action.icon"
          class="cursor-pointer"
          @click="$emit('click', action)"
        >
          <Icon :name="action.icon" :class="action.class" />
        </span>
      </template>
    </template>
  </div>
</template>
<script lang="ts">
import { TableActionType } from "../table.vue"
import Icon from "../../icons/base-icon.vue"
import Button from "../../button/button.vue"
import { PropType } from "vue"

export default {
  props: {
    tableActions: { type: Object as PropType<TableActionType> },
  },
  components: {
    Icon,
    Button,
  },
  $emit: ["click"],
}
</script>
<style lang=""></style>
