<template lang="">
  <Modal :open="open" @close="$emit('close')">
    <div class="text-center">
      <span
        class="inline-block p-3 rounded-[10px] border border-gray-200 shadow-sm"
      >
        <Icon name="addFolder" class="text-gray-700" />
      </span>
      <div class="text-md text-gray-700 mt-5">Upload your document</div>

      <div class="mt-5">
        <div
          class="flex flex-col justify-center border border-dashed rounded-lg border-neutral-500 gap-3 items-center p-[38px] text-center"
        >
          <div v-if="Boolean(fileName)" class="flex flex-col gap-2">
            <div class="text-primary">
              {{ fileName }}
            </div>
            <span class="cursor-pointer" @click="restValues">
              <Icon name="trash" class="text-red-500" />
            </span>
          </div>

          <template v-else>
            <img
              src="/src/assets/images/xls-green.png"
              alt="xls"
              class="w-20"
            />
            <div class="text-md leading-5">
              Drop your document here or
              <span
                class="text-primary cursor-pointer font-bold"
                @click="onPickFile"
                >Upload</span
              >
            </div>
            <div class="text-[#969696]">Supports Xlss, Xls</div>
            <input
              :value="value"
              class="hidden"
              type="file"
              ref="fileInput"
              accept=".xlsx, .xls, .csv"
              @change="onChange"
            />
          </template>
        </div>
      </div>

      <div class="flex gap-3 mt-8">
        <Button
          class="w-full leading-6 rounded-lg"
          color="white"
          @click="$emit('close')"
          >Cancel</Button
        >
        <Button class="w-full leading-6 rounded-lg" @click="handleUpload"
          >Upload</Button
        >
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
  value: File | null
  fileName: string
}

export default {
  components: {
    Modal,
    Icon,
    Input,
    Button,
  },
  emits: ["close", "create", "change"],
  props: {
    modalValue: File,
    open: { type: Boolean, required: true },
    category: { type: Object as PropType<CategoryType | null> },
  },
  mounted() {},
  data(): DataType {
    return {
      categoryName: this.category?.name,
      value: null,
      fileName: "",
    }
  },
  methods: {
    handleUpload() {
      if (!this.value) return
      this.$emit("change", this.value)
      this.restValues()
    },
    restValues() {
      this.value = null
      this.fileName = ""
    },
    onChange(e: Event) {
      const target = e.target as HTMLInputElement
      if (target.files) {
        const file = target.files[0]
        this.value = file
        this.fileName = file.name
      }
    },
    onPickFile() {
      if (this.$refs.fileInput) {
        const fileInput = this.$refs.fileInput as HTMLInputElement
        fileInput.click()
      }
    },
  },
}
</script>
<style lang=""></style>
