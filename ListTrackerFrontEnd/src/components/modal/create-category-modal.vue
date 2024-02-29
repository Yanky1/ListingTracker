<template lang="">
  <Modal :open="open" @close="$emit('close')">
    <div class="text-center">
      <span
        class="inline-block p-3 rounded-[10px] border border-gray-200 shadow-sm"
      >
        <Icon name="folder" class="text-gray-700" />
      </span>
      <div class="text-md text-gray-700 mt-5">{{title}}</div>
      <div class="mt-3">
        <Input
          v-model="categoryName"
          placeholder="Name your category"
          :fullWidth="true"
        />
      </div>
      <div class="flex gap-3 mt-8">
        <Button
          class="w-full leading-6 rounded-lg"
          color="white"
          @click="$emit('close')"
          >Cancel</Button
        >
        <Button class="w-full leading-6 rounded-lg" @click="handleClick">{{
          buttonTitle
        }}</Button>
      </div>
    </div>
  </Modal>
</template>

<script lang="ts">
import Modal from "./model.vue"
import Icon from "../icons/base-icon.vue"
import Input from "../input/input.vue"
import Button from "../button/button.vue"
import { CategoryType } from "../../types/category"
import { PropType } from "vue"

interface DataType {
  categoryName: string | null | undefined
  buttonTitle: string
  title: string
}

export default {
  components: {
    Modal,
    Icon,
    Input,
    Button,
  },
  emits: ["close", "create"],
  props: {
    open: { type: Boolean, required: true },
    category: { type: Object as PropType<CategoryType | null> },
  },
  watch: {
    category: {
      immediate: true,
      handler(newVal: CategoryType | null) {
        this.categoryName = newVal?.categoryName || ""

        if (newVal?.categoryName) {
          this.buttonTitle = "Update category"
          this.title = "Update new category"
        } else {
          this.title = "Create new category"
          this.buttonTitle = "Create category"
        }
      },
    },
  },
  data(): DataType {
    return {
      categoryName: "",
      buttonTitle: "",
      title: ""
    }
  },
  methods: {
    handleClick() {
      if (!this.categoryName) return
      const category = this.category || {}
      this.$emit("create", { ...category, name: this.categoryName })
      this.categoryName = ""
    },
  },
}
</script>
<style lang=""></style>
