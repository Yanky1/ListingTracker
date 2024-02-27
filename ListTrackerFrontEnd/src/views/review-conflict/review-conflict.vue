<template lang="">
  <Layout>
    <Topbar>
      <div class="flex justify-between gap-[25px]">
        <span
          class="flex gap-2.5 text-black items-center cursor-pointer"
          @click="handleBack"
        >
          <Icon name="back" />
          <span class="text-lg font-medium">Back</span>
        </span>

        <Button
          variant="outlined"
          class="py-[15px] ring-[#226BFF]"
          @click="onClickOverwrite"
        >
          Overwrite profile
        </Button>
      </div>
    </Topbar>

    <div class="container mx-auto py-[15px]">
      <div class="flex lg:flex-row flex-col gap-[50px] justify-between">
        <div class="w-full">
          <div class="flex gap-[73px]">
            <ProfileSection :record="currentRecord" />
            <RecordSources :sources="sources" />
          </div>
          <div class="mt-[45px]">
            <RecordCompareTable :records="records" />
          </div>
        </div>

        <div class="flex flex-col items-center">
          <ActivityCard :activities="activities" />
          <div class="flex gap-2 mt-[50px]">
            <Button color="white">
              <Icon name="undo" class="min-w-[20px]" />Revert Changes</Button
            >
            <Button
              class="bg-primary-950"
              @click="openReplaceConfirmModal"
              @confirm="onReplaceConfirm"
              ><Icon name="checkbox" class="min-w-[20px]" /> Resolve
              Changes</Button
            >
          </div>
        </div>
      </div>
    </div>
    <ReplaceConfirmModal
      title="Replace record?"
      description="Do you want to replace all conflicting records with the selected record?"
      :open="showReplaceConfirmModal"
      @close="closeReplaceConfirmModal"
      @confirm="onReplaceConfirm"
      closeButton="Keep Existing"
      confirmButton="Replace All"
    />
    <ConfirmSuccessModal
      :open="showSuccessModal"
      :message="successMessage"
      @close="closeSuccessModal"
    />
  </Layout>
</template>
<script lang="ts">
import Layout from "../../components/layout/dash-layout/dash-layout.vue"
import Button from "../../components/button/button.vue"
import Topbar from "../../components/top-bar/top-bar.vue"
import Icon from "../../components/icons/base-icon.vue"
import ProfileSection from "../../components/profile-section/profile-section.vue"
import RecordSources from "../../components/profile-section/record-sources.vue"
import { RecordType } from "../../types/record"
import ActivityCard from "../../components/activity/activity-card.vue"
import { ActivityType } from "../../components/activity/activity-item.vue"
import ReplaceConfirmModal from "../../components/modal/confirm-modal.vue"
import ConfirmSuccessModal from "../../components/modal/confirmation-success-modal.vue"
import RecordCompareTable from "../../components/conflict-compare-table/conflict-compare-table.vue"
import { v4 as uuidv4 } from "uuid"

interface DataType {
  records: RecordType[]
  activities: ActivityType[]
  showReplaceConfirmModal: boolean
  showSuccessModal: boolean
  successMessage: string
}

export default {
  components: {
    Button,
    Topbar,
    Icon,
    Layout,
    ProfileSection,
    RecordSources,
    ActivityCard,
    ReplaceConfirmModal,
    ConfirmSuccessModal,
    RecordCompareTable,
  },
  data(): DataType {
    return {
      activities: [
        {
          id: uuidv4(),
          type: "Aminu",
          description:
            "from 'Grade 6 student list .Xls' was accepted as correct first name.",
          updatedAt: "Feb 17 2024 - 12:32:13 pm",
        },
        {
          id: uuidv4(),
          type: "Aminu",
          description:
            "from 'Grade 6 student list .Xls' was accepted as correct first name.",
          updatedAt: "Feb 17 2024 - 12:32:13 pm",
        },
      ],
      showReplaceConfirmModal: false,
      showSuccessModal: false,
      successMessage: "",
      records: [
        {
          id: "1",
          first_name: "Aminu",
          last_name: "Alex",
          address: {
            value: "Northbridge California,(CA), 89000, USA",
            status: 0,
          },
          age: "24",
          gender: "male",
          class: "Grade 12",
          source: "Category 1\ Algebra student .Xls",
        },
        {
          id: "1",
          first_name: "Aminu",
          last_name: "Alex",
          address: {
            value: "Northbridge California,(CA), 89000, USA",
            status: 0,
          },
          age: "12",
          gender: "male",
          class: "Grade 12",
          source: "Category 4\ Chem. student .Xls",
        },
      ],
    }
  },
  computed: {
    currentRecord() {
      return this.records[0]
    },
    sources() {
      const sources = []
      for (const record of this.records) {
        sources.push(record.source)
      }
      return sources
    },
  },
  methods: {
    handleBack() {
      this.$router.push("/record")
    },
    openReplaceConfirmModal() {
      this.showReplaceConfirmModal = true
    },
    closeReplaceConfirmModal() {
      this.showReplaceConfirmModal = false
    },
    closeSuccessModal() {
      this.showSuccessModal = false
    },
    onReplaceConfirm() {
      console.log("handle replacing")
      this.showReplaceConfirmModal = false
      this.successMessage = `<span class="font-semibold">${this.currentRecord?.first_name} ${this.currentRecord?.last_name}</span> information has been updated.`
      this.showSuccessModal = true
    },
    onClickOverwrite() {
      const record_id = this.$route.params.record_id
      this.$router.push(`/record/${record_id}/overwrite`)
    },
  },
}
</script>
<style lang=""></style>
