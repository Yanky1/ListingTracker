<template>
  <Layout>
    <Topbar>
      <div class="flex justify-between gap-[25px] w-full">
        <div class="flex gap-[18px] items-center w-full">
          <span
            class="flex gap-2.5 text-black items-center cursor-pointer"
            @click="handleBack"
          >
            <Icon name="back" />
            <span class="text-lg font-medium">Categories</span>
          </span>
          <div class="w-full max-w-[625px]">
            <SearchInput
              v-model="searchText"
              placeholder="Search categories by any keyword"
            />
          </div>
        </div>
        <Button
          variant="outlined"
          class="py-[15px] ring-[#226BFF]"
          @click="openModal"
        >
          <span class="text-nowrap"> Upload new file </span>
          <Icon name="cloudUpload" />
        </Button>
      </div>
    </Topbar>

    <div class="py-[30px] flex justify-center container mx-auto">
      <FileEmpty v-if="!files.length" @click="openModal" />
      <template v-else>
        <Table
          :headCells="headCells"
          :data="getData()"
          :tableActions="tableActions"
        />
      </template>
    </div>
    <FileUploadModal
      :open="showUploadModal"
      @change="handleFileChange"
      @close="closeModal"
    />
  </Layout>
</template>

<script lang="ts">
import Header from "../../components/header/header.vue"
import Layout from "../../components/layout/dash-layout/dash-layout.vue"
import Topbar from "../../components/top-bar/top-bar.vue"
import SearchInput from "../../components/input/search-input.vue"
import Button from "../../components/button/button.vue"
import Icon from "../../components/icons/base-icon.vue"
import { FileType } from "../../types/category.ts"
import FileEmpty from "../../components/files/file-empty.vue"
import FileUploadModal from "../../components/modal/file-upload-modal.vue"
import { v4 as uuidv4 } from "uuid"
import { HeadCellType } from "../../components/table/table-header/table-header.vue"
import Table, { TableActionType } from "../../components/table/table.vue"
import dayjs from "dayjs"

interface DataType {
  headCells: HeadCellType[]
  searchText: string
  files: FileType[]
  showUploadModal: boolean
  tableActions: TableActionType[]
}

export default {
  components: {
    Header,
    Topbar,
    Layout,
    SearchInput,
    Button,
    Icon,
    FileEmpty,
    FileUploadModal,
    Table,
  },
  data(): DataType {
    return {
      searchText: "",
      files: [],
      showUploadModal: false,
      headCells: [
        {
          field: "sn",
          name: "SN",
        },
        {
          field: "name",
          name: "File name",
        },
        {
          field: "format",
          name: "File Format",
        },
        {
          field: "size",
          name: "Size",
        },
        {
          field: "created_at",
          name: "Date uploaded",
        },
        {
          field: "last_modified",
          name: "Last modified",
        },
        {
          field: "Actions",
          name: "Action",
        },
      ],
      tableActions: [
        {
          type: "iconButton",
          icon: "trash",
          class: "text-red",
          callback: this.onRemove,
        },
        {
          type: "button",
          label: "Import file",
          icon: "download",
          class: "w-full bg-purple-500 ring-1 ring-primary-1000",
          callback: this.onImport,
        },
      ],
    }
  },
  methods: {
    handleBack() {
      this.$router.push("/categories")
    },
    closeModal() {
      this.showUploadModal = false
    },
    openModal() {
      this.showUploadModal = true
    },
    handleFileChange(file: File) {
      const filename = file.name
      const lastDotIndex = filename.lastIndexOf(".")
      const name = filename.substring(0, lastDotIndex)
      const format = filename.substring(lastDotIndex + 1)

      this.files.push({
        id: uuidv4(),
        name,
        format,
        size: `${(file.size / (1024 * 2)).toFixed(2)} MB`,
        created_at: `${new Date().getTime()}`,
        last_modified: file.lastModified,
      })
      this.closeModal()
    },
    getData() {
      return this.files.map((row, index) => {
        return {
          ...row,
          sn: index + 1,
          created_at: dayjs(row.created_at).format("MM/DD/YYYY"),
          last_modified: dayjs(row.last_modified).format("MM/DD/YYYY"),
        }
      })
    },
    onRemove() {
      console.log("onRemove")
    },
    onImport() {
      console.log("onImport")
    },
  },
}
</script>
