<template lang="">
  <Modal :open="open" @close="$emit('close')">
    <div class="text-center">
      <span class="inline-block">
        <span
          class="p-3 bg-warning-100 border-8 border-warning-50 rounded-full inline-block"
        >
          <Icon name="warning" class="text-warning-800" />
        </span>
      </span>

      <div class="text-md text-gray-900 mt-5 font-semibold">
        {{ title }}
      </div>

      <div v-if="description" class="mt-2 text-gray-500 text-sm">
        {{ description }}
      </div>

      <div v-if="alert" class="mt-5">
        <Alert icon="warning" color="warning" class="text-left">{{
          alert
        }}</Alert>
      </div>

      <div class="flex gap-3 mt-8">
        <Button
          class="w-full leading-6 rounded-lg"
          color="white"
          @click="$emit('close')"
          >{{ closeButton }}</Button
        >
        <Button class="w-full leading-6 rounded-lg" @click="$emit('confirm')">{{
          confirmButton
        }}</Button>
      </div>
    </div>
  </Modal>
</template>

<script lang="ts">
import Modal from "./model.vue"
import Icon from "../icons/base-icon.vue"
import Button from "../button/button.vue"
import Alert from "../alert/alert.vue"

export default {
  components: {
    Modal,
    Icon,
    Button,
    Alert,
  },
  emits: ["close", "confirm"],
  props: {
    open: { type: Boolean, required: true },
    title: { type: String, required: true },
    description: { type: String },
    closeButton: { type: String, default: "close" },
    confirmButton: { type: String, default: "Confirm" },
    alert: { type: String, default: "" },
  },
}
</script>
<style lang=""></style>
