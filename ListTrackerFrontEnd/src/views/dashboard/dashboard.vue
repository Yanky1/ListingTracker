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
            @change="handleSortChange"
          />


          <!-- <Dropdown
            :items="recordSortItems"
            label="Filter"
            icon="bars-filter"
            iconClass="min-w-5 min-h-5"
          /> -->

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
        :data="filteredAndSortedData"
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
// import { v4 as uuidv4 } from "uuid"
import { RecordType } from "../../types/record"
import { getPersonList } from "../../services/dashboardService"

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
        field: "phoneNumber",
        name: "Phone Number",
      },
      {
        field: "email",
        name: "Email",
      },
      {
        field: "address",
        name: "Address",
      },
      {
        field: "city",
        name: "City",
      },
      {
        field: "state",
        name: "State",
      },
      {
        field: "zipCode",
        name: "Zip Code",
      },
      {
        field: "country",
        name: "Country",
      },
      {
        field: "status",
        name: "Status",
      },
    ],
      tableData: 
      [
      {
        id: "",
        first_name: "",
        last_name: "",
        email: "",
        phoneNumber: "",
        address: "",
        city: "",
        state: "",
        zipCode: "",
        country: "",
        status:"",
        source:"",
        children: [
          {
            id: "",
            first_name: "",
            last_name: "",
            email: "",
            phoneNumber: "",
            address: "",
            city: "",
            state: "",
            zipCode: "",
            country: "",
            status:"",
            source:""
          },
        ],
      }
      ], // Initialize empty array
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
          field: "phoneNumber",
          name: "Phone",
        },
        {
          field: "email",
          name: "email",
        },
        {
          field: "address",
          name: "Address",
        },
        {
          field: "city",
          name: "City",
        },
        {
          field: "state",
          name: "State",
        },
        {
          field: "zipCode",
          name: "Zip Code",
        },
        {
          field: "country",
          name: "Country",
        },
        {
          field:"source",
          name:"source"
        }
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
    filteredAndSortedData() {
    let filteredData = this.tableData.filter(item => {
      return (
        item.first_name.toLowerCase().includes(this.searchText.toLowerCase()) ||
        item.last_name.toLowerCase().includes(this.searchText.toLowerCase())
      );
    });

    if (this.activeRecord) {
      filteredData = filteredData.sort((a:any, b) => {
        if (this.activeRecord === 'name') {
          return a.first_name.localeCompare(b.first_name);
        } else if (this.activeRecord === 'date_modified') {
        } else if (this.activeRecord === 'date_added') {
        }
      });
    }

    return filteredData;
  },
    getData() {
      return this.tableData.map((row: any, index: number) => {
        return {
          ...row,
          number: index + 1,
        }
      })
    },
  },
  created: function () {
    this.getInitCategoryList();
  },
  methods: {
    handleSortChange(value:any) {
    this.activeRecord = value;
  },
    async getInitCategoryList() {
      try {
        const response = await getPersonList();
        const personListData = response.data;
        console.log(personListData); // Check if you receive the correct data

        // Update the tableData with the response data
        this.tableData = personListData.map((person: any,index:any) => ({
          id: person.acceptedPerson.id,
          first_name: person.acceptedPerson.firstName,
          last_name: person.acceptedPerson.lastName,
          email: person.acceptedPerson.email,
          phoneNumber: person.acceptedPerson.phoneNumber,
          address: person.acceptedPerson.address,
          city: person.acceptedPerson.city,
          state: person.acceptedPerson.state,
          zipCode: person.acceptedPerson.zipCode,
          country: person.acceptedPerson.country,
          status:person.personList.length+' Conflict',
          children: person.personList.map((child: any) => ({
            id: child.id,
            first_name: child.firstName,
            last_name: child.lastName,
            email: child.email,
            phoneNumber: child.phoneNumber,
            address: child.address,
            city: child.city,
            state: child.state,
            zipCode: child.zipCode,
            country: child.country,
            source:child.fileName,
            acid:person.acceptedPerson.id
          })),
          number:index+1
        }));
        console.log(this.tableData);
      } catch (error) {
        console.error("Error fetching person list data:", error);
      }
    },
    handleViewDetail(row: any) {
      this.$router.push(`/record/${row.id}/review-conflict`)
    },
  },
}
</script>


