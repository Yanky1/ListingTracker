<template lang="">
  <tbody>
    <template :key="`${row.id}-${index}`" v-for="(row, index) of data">
      <TableRow>
        <TableCell v-if="showCheckbox && !isExpanded(row.id)">
          <Checkbox :key="`${row.id}-checkbox-${index}`" :value="row.id" />
        </TableCell>
        <TableCell v-if="showExpandIcon" :colspan="isExpanded(row.id) ? 2 : 1">
          <span @click="handleClick(row.id)" class="cursor-pointer"
            ><Icon
              name="chevron-down"
              :class="[
                'min-w-[15px] transition-all duration-300 delay-100',
                isExpanded(row.id) ? 'rotate-180' : '',
              ]"
          /></span>
        </TableCell>

        <TableCell
          ref="tableCell"
          :key="`${row.id}-${headCell.field}`"
          v-for="headCell of headCells"
        >
          <template
            v-if="
              (typeof row[headCell.field] === 'string') |
                (typeof row[headCell.field] === 'number')
            "
          >
            {{ row[headCell.field] }}
          </template>
          <template v-if="row[headCell.field]">
            {{ row[headCell.field].value }}
        </template>
          <template v-else>
          <!-- Handle the case when row[headCell.field] is undefined -->
        </template>

        </TableCell>

        <TableCell v-if="tableActions">
          <TableActions
            @click="(action) => handleAction(action.callback, row)"
            :tableActions="tableActions"
          />
        </TableCell>
      </TableRow>

      <TableRow v-if="row.children" class="w-full">
        <TableCell
          :class="[!isExpanded(row.id) ? ' border-none' : '', 'px-0 py-0']"
          :colspan="colspan"
        >
          <Collapse :when="isExpanded(row.id)" class="duration-500">
            <template v-if="Object.keys(cellWidths).length > 0">
              <RecordCompareTable
                :records="row.children"
                :headCells="rowTableHeadCells"
                :tableActions="rowTableActions"
                :cellWidths="cellWidths"
              />
            </template>
          </Collapse>
        </TableCell>
      </TableRow>
    </template>
  </tbody>
</template>

<script lang="ts">
import { HeadCellType } from "../table-header/table-header.vue"
import TableRow from "../table-row/table-row.vue"
import TableCell from "../table-cell/table-cell.vue"
import Checkbox from "../../checkbox/checkbox.vue"
import { PropType } from "vue"
import { TableActionType } from "../table.vue"
import RecordCompareTable from "../../conflict-compare-table/conflict-compare-table-row-view.vue"
import { Collapse } from "vue-collapsed"
import Icon from "../../icons/base-icon.vue"
import TableActions from "../table-actions/table-actions.vue"

interface DataType {
  expandedId: string | null
  cellWidths: Object
}

export default {
  components: {
    TableRow,
    TableCell,
    Checkbox,
    RecordCompareTable,
    Collapse,
    Icon,
    TableActions,
  },
  props: {
    data: { type: Array, required: true },
    checkedItems: { type: Array<String> },
    showCheckbox: { type: Boolean },
    showExpandIcon: { type: Boolean },
    headCells: { type: Array<HeadCellType>, required: true },
    tableActions: { type: Object as PropType<TableActionType[]>, default: [] },
    rowTableHeadCells: { type: Array<HeadCellType> },
    rowTableActions: { type: Object as PropType<TableActionType[]> },
  },
  computed: {
    colspan() {
      let length = Object.keys(this.headCells).length
      if (this.showCheckbox) {
        length += 1
      }
      if (this.tableActions) {
        length += 1
      }
      if (this.showExpandIcon) {
        length += 1
      }
      return String(length)
    },
  },
  data(): DataType {
    return {
      expandedId: null,
      cellWidths: {},
    }
  },
  async mounted() {
    const cellWidths = this.getTableCellWidth()
    this.cellWidths = cellWidths
  },
  methods: {
    getTableCellWidth() {
      const tableCellRefs: HTMLTableCellElement[] = Array.isArray(
        this.$refs.tableCell
      )
        ? (this.$refs.tableCell as HTMLTableCellElement[])
        : Object.values(
            this.$refs.tableCell as { [key: string]: HTMLTableCellElement }
          )
      const newObj = {} as any
      for (let i = 0; i < this.headCells.length; i++) {
        const ref = Object.values(tableCellRefs)[i]
        if (ref) {
          const cell = (ref as any).$el as HTMLTableCellElement
          newObj[this.headCells[i].field] = cell.getBoundingClientRect().width
        }
      }
      return newObj
    },
    handleAction(callback: Function, row: any) {
      if (callback instanceof Function) {
        callback(row) // Invoke the callback function with the row data
      }
    },
    handleClick(row_id: string) {
          const radioButtons = document.querySelectorAll('input[type="radio"]');
      radioButtons.forEach((radio: any) => {
        radio.checked = false;
      });
      if (row_id === this.expandedId) {
        this.expandedId = null
      } else {
        this.expandedId = row_id
      }
    },
    deleteRecord(row: any) {
      console.log("row", row)
    },
    isExpanded(id: string) {
      return id === this.expandedId
    },
  },
  $emit: ["row-click"],
}
</script>
<style lang=""></style>
