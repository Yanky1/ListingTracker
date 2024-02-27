<template lang="">
  <table
    :class="[
      'w-full',
      'rounded-[8px] overflow-hidden',
      'border-collapse ring-1 ring-inset ring-[#B7B7B7]',
      _class,
    ]"
  >
    <TableHeader
      :cells="headCells"
      :showCheckbox="showCheckbox"
      :showExpandIcon="showExpandIcon"
      :checkedItems="checkedItems"
      :tableActions="tableActions"
    />
    <TableBody
      :headCells="headCells"
      :data="data"
      :showExpandIcon="showExpandIcon"
      :showCheckbox="showCheckbox"
      :checkedItems="checkedItems"
      :tableActions="tableActions"
      :rowTableHeadCells="rowTableHeadCells"
      :rowTableActions="rowTableActions"
    />
  </table>
</template>

<script lang="ts">
import TableHeader, { HeadCellType } from "./table-header/table-header.vue"
import TableBody from "./table-body/table-body.vue"
import { PropType } from "vue"

export interface TableActionType {
  type: string
  label?: string
  class?: string
  color?: string
  icon?: string
  callback: Function
}

export default {
  components: {
    TableHeader,
    TableBody,
  },
  props: {
    class: { type: String, default: "" },
    headCells: { type: Array<HeadCellType>, required: true },
    data: { type: Array, required: true },
    checkedItems: { type: Array<String>, default: [] },
    showCheckbox: { type: Boolean, default: false },
    showExpandIcon: { type: Boolean, default: false },
    tableActions: { type: Object as PropType<TableActionType[]> },
    hasRowTable: { type: Object as PropType<TableActionType[]> },
    rowTableHeadCells: { type: Array<HeadCellType> },
    rowTableActions: { type: Object as PropType<TableActionType[]> },
  },
  computed: {
    _class() {
      return this.class
    },
  },
}
</script>
<style lang=""></style>
