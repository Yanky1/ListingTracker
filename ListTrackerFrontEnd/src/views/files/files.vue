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
import {uploadFile,getFiles,deleteFile} from "../../services/fileServices"
import { apiUrl } from "../../appConfig";
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
          callback: (item:any) =>this.onRemove(item),
        },
        {
          type: "button",
          label: "Import file",
          icon: "download",
          class: "w-full bg-purple-500 ring-1 ring-primary-1000",
          callback: (item:any) => this.onImport(item),
        },
      ],
    }
  },
  created: function () {
        this.getInitFileList();
  },
  methods: {
        async getInitFileList() {
      try {
        const params = this.$route.params;
        const categoryId = params.category_id;
        const response = await getFiles(categoryId[0]);
        const filessData = response.data;
        // Assuming categoriesData is an array of objects from the response

        // Update the categories array with the response data
        this.files = filessData.map((ft: any) => ({
          id: ft.id.toString(), // Ensure id is a string
          name: ft.fileDescription,
          format:ft.fileExtension,
          size:ft.fileSize

        }));
      } catch (error) {
        console.error("Error fetching category data:", error);
      }
    },
    handleBack() {
      this.$router.push("/categories")
    },
    closeModal() {
      this.showUploadModal = false
    },
    openModal() {
      this.showUploadModal = true
    },
    fileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onload = () => {
            if (reader.result && typeof reader.result === 'string') {
                // Convert the ArrayBuffer to a Base64 string
                const base64String = reader.result.split(',')[1];
                resolve(base64String);
            } else {
                reject(new Error('File reading error: FileReader result is null or not a string'));
            }
        };

        reader.onerror = (error) => {
            reject(error);
        };

        // Read the file as a Data URL
        reader.readAsDataURL(file);
    });
  },
   async handleFileChange(file: File) {
      const filename = file.name
      const lastDotIndex = filename.lastIndexOf(".")
      const name = filename.substring(0, lastDotIndex)
      const format = filename.substring(lastDotIndex + 1)
      const params = this.$route.params;
      const categoryId = params.category_id;
      const base64String = await this.fileToBase64(file);
      var fileData={
      id: uuidv4(),
      fileName: filename,
      filePath: '',
      fileExtension: format,
      fileSize: `${(file.size / (1024 * 2)).toFixed(2)} MB`,
      fileContentType: file.type,
      fileDescription: name,
      isDeleted: false,
      dateUploaded: new Date(),
      categoryId: +categoryId,
      file: base64String,
    };
      const response = await uploadFile(fileData);
    if (response.isSuccessful){
      this.files.push({
        id: fileData.id,
        name,
        format,
        size: fileData.fileSize,
        created_at: `${fileData.dateUploaded.getTime()}`,
        last_modified: file.lastModified,
      })
    }
    else{
      alert(response.message)
    }
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
    async onRemove(item:any) {
      console.log(item);
      var response=await deleteFile(item.id);
      if(response.isSuccessful){
        this.files = this.files.filter((file) => file.id !== item.id)
      }
    },
    async onImport(item: any) {
      try {
              const response = await fetch(apiUrl+`/downloadFile?id=${item.id}`, {
                method: 'POST'
              });

              if (!response.ok) {
                throw new Error('Failed to download file');
              }

              const blob = await response.blob();
              const url = window.URL.createObjectURL(blob);
              const a = document.createElement('a');
              a.href = url;
              a.download = item.name; 
              document.body.appendChild(a);
              a.click();
              window.URL.revokeObjectURL(url);
            } catch (error) {
              console.error('Error downloading file:', error);
            }
    }

  },
}
</script>
