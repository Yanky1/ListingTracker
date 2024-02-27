<template lang="">
  <table class="conflict-compare-table w-full">
    <thead class="bg-[#FFE49E]">
      <TableHead :disableRemove="true"></TableHead>
      <TableHead v-for="(record, index) of records">{{
        `Record #${index + 1}`
      }}</TableHead>
    </thead>

    <tbody>
      <tr v-for="(row, key) of data">
        <th class="bg-[#F3F6FF]">
          <div class="capitalize">
            {{ getLabel(key) }}
          </div>
        </th>
        <TBody
          v-for="(column, index) of row"
          :field="getLabel(key)"
          :key="`${key}-${index}`"
          :index="index"
          :value="getColumn(column)"
          :rows="data"
        ></TBody>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { PropType } from "vue"
import { RecordType } from "../../types/record"
import TableHead from "./table-head.vue"
import TBody from "./table-body.vue"

export default {
  props: {
    records: { type: Object as PropType<RecordType[]>, required: true },
  },
  components: {
    TableHead,
    TBody,
  },
  computed: {
    data() {
      const convertedObject: any = {}
      this.records.forEach((record: RecordType) => {
        Object.entries(record).forEach(([key, value]) => {
          if (!convertedObject[key]) {
            convertedObject[key] = []
          }
          convertedObject[key].push(value)
        })
      })

      return convertedObject
    },
  },
  methods: {
    getColumn (column: any) {
      if (typeof column === 'string') {
        return column
      } else {
        return column.value
      }
    },
    getLabel(key: string) {
      switch (key) {
        case "first_name":
          return "first name"
        case "last_name":
          return "last name"
        default:
          return key
      }
    },
  },
}
</script>

<style lang="scss">
table.conflict-compare-table {
  @apply overflow-hidden;
  outline: 1px solid;
  @apply rounded-lg;
  @apply outline-neutral-500;

  thead {
    th {
      @apply border-s;
      @apply border-neutral-500;

      &:first-child {
        @apply border-s-0;
        @apply border-b;
      }
    }
  }

  tr {
    td {
      @apply border;
      @apply border-neutral-500;
      &:last-child {
        @apply border-e-0;
      }
    }
    &:last-child {
      td {
        @apply border-b-0;
      }
    }
  }
}
</style>
