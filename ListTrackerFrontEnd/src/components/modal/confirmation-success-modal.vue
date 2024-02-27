<template lang="">
  <Modal :open="open" @close="$emit('close')">
    <div class="text-center">
      <span class="inline-block">
        <span
          class="p-3 bg-success-100 border-8 border-success-50 rounded-full inline-block"
        >
          <Icon name="circle-checkbox-2" class="text-success-600" />
        </span>
      </span>

      <div v-html="_message" class="text-md text-gray-900 mt-5"></div>

      <div class="flex gap-3 mt-8">
        <Button class="w-full leading-6 rounded-lg" @click="$emit('close')"
          >Okay</Button
        >
      </div>
    </div>
  </Modal>
</template>

<script lang="ts">
import Modal from "./model.vue"
import Icon from "../icons/base-icon.vue"
import Button from "../button/button.vue"

export default {
  components: {
    Modal,
    Icon,
    Button,
  },
  emits: ["close"],
  props: {
    open: { type: Boolean, required: true },
    message: String,
  },
  watch: {
    message: {
      immediate: true,
      handler(newVal: string) {
        this._message = newVal || ""
      },
    },
  },
  data() {
    return { _message: "" }
  },
}
</script>
<style lang=""></style>
