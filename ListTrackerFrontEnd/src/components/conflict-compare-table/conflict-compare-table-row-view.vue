<template lang="">
  <table class="conflict-compare-table-v2 w-full">
    <tbody>
      <tr v-for="(row, key) of records">
        <td class="w-[190px]">
          <div class="text-red font-bold">
            {{ `Record #${key + 1}` }}
          </div>
        </td>

        <td
          :style="`width: ${cellWidths[headCell.field]}px`"
          v-for="(headCell, index) of headCells"
        >
          <div class="capitalize">
            <template v-if="headCell.field === 'source'">
              <template v-if="typeof row[headCell.field] === 'string'">
                {{ row[headCell.field] }}
              </template>
            </template>
            <template v-else-if="headCell.field === 'address'">
              <span
                class="flex gap-2 items-center cursor-pointer"
                :title="row[headCell.field].value"
              >
                <Radio :id="`${row.id}-${index}-address`" />
                <Icon
                  :name="
                    row[headCell.field].status === 0 ? 'unverified' : 'verified'
                  "
                  :class="
                    row[headCell.field].status === 0
                      ? 'text-red'
                      : 'text-primary'
                  "
                />
                <label
                  :for="`${row.id}-${index}-address`"
                  class="cursor-pointer line-clamp-2"
                  >{{ row[headCell.field].value }}</label
                >
              </span>
            </template>
            <template v-else-if="typeof row[headCell.field] === 'string'">
              <Radio :id="`${row.id}-${index}`" :label="row[headCell.field]" />
            </template>
          </div>
        </td>

        <td v-if="tableActions.length > 0">
          <TableActions :tableActions="tableActions" />
        </td>
      </tr>
    </tbody>
  </table>

  <div class="flex">
    <div class="flex gap-2 py-4 px-4">
      <Button color="white">
        <Icon name="undo" class="min-w-[20px]" />Revert Changes</Button
      >
      <Button
        class="bg-primary-950"
        @click="openReplaceConfirmModal"
        @confirm="onReplaceConfirm"
        ><Icon name="checkbox" class="min-w-[20px]" /> Resolve Changes</Button
      >
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
</template>

<script lang="ts">
import { PropType } from "vue"
import { RecordType } from "../../types/record"
import { HeadCellType } from "../table/table-header/table-header.vue"
import { TableActionType } from "../table/table.vue"
import Button from "../button/button.vue"
import Radio from "../radio/radio.vue"
import Icon from "../icons/base-icon.vue"
import ReplaceConfirmModal from "../modal/confirm-modal.vue"
import ConfirmSuccessModal from "../modal/confirmation-success-modal.vue"
import TableActions from "../table/table-actions/table-actions.vue"

export default {
  props: {
    records: { type: Object as PropType<RecordType[]>, required: true },
    headCells: { type: Array<HeadCellType>, required: true },
    tableActions: { type: Object as PropType<TableActionType[]>, default: [] },
    cellWidths: { type: Object },
  },
  components: {
    Button,
    Radio,
    Icon,
    ConfirmSuccessModal,
    ReplaceConfirmModal,
    TableActions,
  },
  computed: {},
  data() {
    return {
      showReplaceConfirmModal: false,
      showSuccessModal: false,
      successMessage: "",
    }
  },
  methods: {
    handleAction(callback: Function, row: any) {
      callback(row) // Invoke the callback function with the row data
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
      this.successMessage = `<span class="font-semibold">Information has been updated.`
      this.showSuccessModal = true
    },
  },
}
</script>

<style lang="scss">
table.conflict-compare-table-v2 {
  tr {
    &:first-child {
      td {
        @apply border-t-0;
      }
    }

    td {
      @apply py-8;
      @apply px-5;
      @apply border;
      @apply border-neutral-500;
      @apply bg-[#F3F6FF];
    }
  }
}
</style>
