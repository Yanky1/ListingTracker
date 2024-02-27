<template>
  <Layout>
    <Topbar>
      <div class="flex justify-between gap-[25px]">
        <span
          class="flex gap-2.5 text-black items-center cursor-pointer"
          @click="onBack"
        >
          <Icon name="back" />
          <span class="text-lg font-medium">Back</span>
        </span>
      </div>
    </Topbar>

    <div class="container mx-auto py-[65px]">
      <div
        :class="[
          'w-full max-w-[500px] mx-auto  rounded-lg p-8',
          'border bg-white border-neutral-500',
          'flex flex-col gap-6',
        ]"
      >
        <div class="flex flex-col gap-1.5">
          <label
            for="overwrite-first-name"
            class="text-sm font-medium text-gray-700"
            >First name</label
          >
          <Input
            id="overwrite-first-name"
            type="text"
            fullWidth
            placeholder="Enter second name"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label
            for="overwrite-second-name"
            class="text-sm font-medium text-gray-700"
            >Second name</label
          >
          <Input
            id="overwrite-second-name"
            type="text"
            fullWidth
            placeholder="Enter second name"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label for="gender" class="text-sm font-medium text-gray-700"
            >Gender</label
          >
          <Select
            id="gender"
            type="text"
            v-model="gender"
            :items="genderItems"
            fullWidth
            placeholder="Select gender"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label for="overwrite-birth" class="text-sm font-medium text-gray-700"
            >Date of birth</label
          >
          <Input
            id="overwrite-birth"
            type="date"
            fullWidth
            placeholder="Enter address"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label
            for="overwrite-address-name"
            class="text-sm font-medium text-gray-700"
            >Address</label
          >
          <Input
            id="overwrite-address-name"
            type="text"
            fullWidth
            placeholder="Enter address"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label for="overwrite-phone" class="text-sm font-medium text-gray-700"
            >Phone number</label
          >
          <Input
            id="overwrite-phone"
            type="text"
            fullWidth
            placeholder="Enter phone number"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label
            for="overwrite-guardian-name"
            class="text-sm font-medium text-gray-700"
            >Guardian name</label
          >
          <Input
            id="overwrite-guardian-name"
            type="text"
            fullWidth
            placeholder="Enter guardian name"
          />
        </div>

        <div class="flex flex-col gap-1.5">
          <label
            for="overwrite-guardian-phone"
            class="text-sm font-medium text-gray-700"
            >Guardian phone</label
          >
          <Input
            id="overwrite-guardian-phone"
            type="text"
            fullWidth
            placeholder="Enter guardian phone"
          />
        </div>

        <div class="flex gap-2 w-full mt-5">
          <Button color="white" class="w-full" @click="showRevertModal = true"
            ><Icon name="undo" />Revert Changes</Button
          >
          <Button
            class="bg-primary-950 w-full"
            @click="showSaveConfirmModal = true"
            ><Icon name="checkbox" />Save new profile</Button
          >
        </div>
      </div>
    </div>

    <ConfirmModal
      title="Revert changes?"
      description="Do you want to revert changes you made?"
      :open="showRevertModal"
      @close="showRevertModal = false"
      @confirm="onRevertChanges"
      closeButton="Discard"
      confirmButton="Revert changes"
    />

    <ConfirmModal
      title="Are you sure about this changes?"
      alert="You're about to overwrite information with the provided information. If you proceed with this, you won't be able to undo your action"
      :open="showSaveConfirmModal"
      @close="showSaveConfirmModal = false"
      @confirm="onSave"
      closeButton="Discard"
      confirmButton="Revert changes"
    />
  </Layout>
</template>

<script lang="ts">
import Header from "../../components/header/header.vue"
import Layout from "../../components/layout/dash-layout/dash-layout.vue"
import Topbar from "../../components/top-bar/top-bar.vue"
import Button from "../../components/button/button.vue"
import Icon from "../../components/icons/base-icon.vue"
import Input from "../../components/input/input.vue"
import Dropdown, { ItemType } from "../../components/dropdown/dropdown.vue"
import Select from "../../components/select/select.vue"
import ConfirmModal from "../../components/modal/confirm-modal.vue"

interface DataType {
  genderItems: ItemType[]
  gender: string
  showRevertModal: boolean
  showSaveConfirmModal: boolean
}

export default {
  components: {
    Header,
    Topbar,
    Layout,
    Button,
    Icon,
    Input,
    Dropdown,
    Select,
    ConfirmModal,
  },
  data(): DataType {
    return {
      genderItems: [
        {
          value: "male",
          label: "Male",
        },
        {
          value: "female",
          label: "Female",
        },
      ],
      gender: "",
      showRevertModal: false,
      showSaveConfirmModal: false,
    }
  },
  methods: {
    onBack() {
      const record_id = this.$route.params.record_id
      this.$router.push(`/record/${record_id}/review-conflict`)
    },
    onRevertChanges() {
      console.log("onRevertChanges")
    },
    onSave() {
      console.log("onSave")
    },
  },
}
</script>
