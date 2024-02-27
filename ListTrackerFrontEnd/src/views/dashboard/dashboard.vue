<template>
  <Layout>
    <TopBar>
      <div class="flex items-center gap-[25px] justify-between">
        <div class="flex items-center gap-[25px] w-full">
          <Dropdown
            :items="recordSortItems"
            v-model="activeRecord"
            label="Sort record by"
            rounded
          />

          <Dropdown
            :items="recordSortItems"
            label="Filter"
            icon="bars-filter"
            iconClass="min-w-5 min-h-5"
          />

          <div class="w-full max-w-[600px]">
            <SearchInput
              v-model="searchText"
              :fullWidth="true"
              placeholder="Search student by any keyword"
            />
          </div>
        </div>

        <Button variant="outlined" class="py-[15px] ring-[#226BFF]">
          <span class="text-nowrap"> Download Doc </span>
          <Icon name="download" />
        </Button>
      </div>
    </TopBar>
    <div class="container mx-auto pt-8 pb-4">
      <Alert color="warning" icon="circle-info"
        >you can toggle down the details to quickly resolve conflicting records,
        you can choose the main information of any conflicting item by clicking
        the item.
      </Alert>

      <Table
        :headCells="headCells"
        :data="getData"
        :showExpandIcon="true"
        :showCheckbox="true"
        class="mt-[23px]"
        :tableActions="tableActions"
        :rowTableHeadCells="recordHeadCells"
        :rowTableActions="recordTableActions"
      />
    </div>
  </Layout>
</template>

<script lang="ts">
import Header from "../../components/header/header.vue"
import Layout from "../../components/layout/dash-layout/dash-layout.vue"
import TopBar from "../../components/top-bar/top-bar.vue"
import Alert from "../../components/alert/alert.vue"
import Table, { TableActionType } from "../../components/table/table.vue"
import { HeadCellType } from "../../components/table/table-header/table-header.vue"
import Dropdown, { ItemType } from "../../components/dropdown/dropdown.vue"
import Button from "../../components/button/button.vue"
import Icon from "../../components/icons/base-icon.vue"
import SearchInput from "../../components/input/search-input.vue"
import { v4 as uuidv4 } from "uuid"
import { RecordType } from "../../types/record"

interface TableDataType extends RecordType {
  children: RecordType[]
}

interface DataType {
  headCells: HeadCellType[]
  recordHeadCells: HeadCellType[]
  tableData: TableDataType[]
  activeRecord: string | undefined
  recordSortItems: ItemType[]
  searchText: string
  tableActions: TableActionType[]
  recordTableActions: TableActionType[]
}

export default {
  components: {
    Header,
    Layout,
    TopBar,
    Alert,
    Table,
    Dropdown,
    Button,
    Icon,
    SearchInput,
  },
  data(): DataType {
    return {
      headCells: [
        {
          field: "number",
          name: "SN",
        },
        {
          field: "first_name",
          name: "First name",
        },
        {
          field: "last_name",
          name: "Last name",
        },
        {
          field: "phone",
          name: "Phone Number",
        },
        {
          field: "birthday",
          name: "Date of Birth",
        },
        {
          field: "address",
          name: "Address",
        },
        {
          field: "status",
          name: "Status",
        },
      ],
      tableData: [
        {
          id: uuidv4(),
          first_name: "Aminu",
          last_name: "Aminu",
          phone: "0903485423",
          birthday: "21/10/2001",
          address: { value: "21 maple avenue", status: 0 },
          status: "2 Conflict",
          age: "24",
          source: "Category 1\ Algebra student .Xls",
          children: [
            {
              id: uuidv4(),
              first_name: "Aminu",
              last_name: "Alex",
              address: {
                value: "Northbridge California,(CA), 89000, USA",
                status: 0,
              },
              age: "24",
              gender: "male",
              phone: "0903485423",
              class: "Grade 12",
              birthday: "21/10/2001",
              source: "Category 1\ Algebra student .Xls",
            },
            {
              id: uuidv4(),
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
              phone: "0903485423",
              birthday: "21/10/2001",
            },
          ],
        },
        {
          id: uuidv4(),
          first_name: "Aminu",
          last_name: "Aminu",
          phone: "0903485423",
          birthday: "21/10/2001",
          address: { value: "21 maple avenue", status: 0 },
          status: "2 Conflict",
          age: "24",
          source: "Category 1\ Algebra student .Xls",
          children: [
            {
              id: uuidv4(),
              first_name: "Aminu",
              last_name: "Alex",
              address: {
                value: "Northbridge California,(CA), 89000, USA",
                status: 0,
              },
              age: "24",
              gender: "male",
              phone: "0903485423",
              class: "Grade 12",
              birthday: "21/10/2001",
              source: "Category 1\ Algebra student .Xls",
            },
            {
              id: uuidv4(),
              first_name: "Aminu",
              last_name: "Alex",
              address: {
                value: "Northbridge California,(CA), 89000, USA",
                status: 1,
              },
              age: "12",
              gender: "male",
              class: "Grade 12",
              source: "Category 4\ Chem. student .Xls",
              phone: "0903485423",
              birthday: "21/10/2001",
            },
          ],
        },
      ],
      activeRecord: "",
      recordSortItems: [
        {
          value: "name",
          label: "Name",
        },
        {
          value: "date_modified",
          label: "Date modified",
        },
        {
          value: "date_added",
          label: "Date added",
        },
      ],
      searchText: "",
      tableActions: [
        {
          type: "button",
          label: "View",
          class: "w-full bg-purple-500 ring-1 ring-primary-1000",
          callback: this.handleViewDetail,
        },
      ],
      recordHeadCells: [
        {
          field: "first_name",
          name: "First name",
        },
        {
          field: "last_name",
          name: "Last name",
        },
        {
          field: "phone",
          name: "Phone",
        },
        {
          field: "birthday",
          name: "birthday",
        },
        {
          field: "address",
          name: "Address",
        },
        {
          field: "source",
          name: "Source",
        },
      ],
      recordTableActions: [
        {
          type: "button",
          label: "Delete record",
          color: "red",
          callback: this.deleteRecord,
        },
      ],
    }
  },
  computed: {
    getData() {
      return this.tableData.map((row: any, index: number) => {
        return {
          ...row,
          number: index + 1,
        }
      })
    },
  },
  methods: {
    handleViewDetail(row: any) {
      this.$router.push(`/record/${row.id}/review-conflict`)
    },
  },
}
</script>
