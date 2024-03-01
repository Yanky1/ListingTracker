<template lang="">
  <th :class="['h-10 px-2.5 text-sm', _class]">
    <div class="flex justify-between items-center">
      <slot />
      <span 
        v-if="!disableRemove" @click="removClk"
        class="flex gap-2 items-center text-xs text-red cursor-pointer"
      >
        <Icon name="trash" class="min-w-4 w-4" />
        Delete record
      </span>
    </div>
  </th>
</template>
<script lang="ts">
import Icon from "../icons/base-icon.vue"

export default {
  props: {
    class: String,
    disableRemove: Boolean,
  },
  components: {
    Icon,
  },
  computed: {
    _class() {
      return this.class
    },
  },
  methods:{
    removClk(){
      this.$emit('dltClk1');
    },
  }
}
</script>
<style lang=""></style>
