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
        <!-- Input field for first name -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-first-name" class="text-sm font-medium text-gray-700">First name</label>
          <Input id="overwrite-first-name" placeholder="First Name" type="text" fullWidth v-model="recordData.first_name" />
        </div>

        <!-- Input field for last name -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-last-name" class="text-sm font-medium text-gray-700">Last name</label>
          <Input id="overwrite-last-name" placeholder="Last Name" type="text" fullWidth v-model="recordData.last_name" />
        </div>

        <!-- Input field for email -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-email" class="text-sm font-medium text-gray-700">Email</label>
          <Input id="overwrite-email" placeholder="Email" type="email" fullWidth v-model="recordData.email" />
        </div>

        <!-- Input field for phone number -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-phone" class="text-sm font-medium text-gray-700">Phone number</label>
          <Input id="overwrite-phone" type="text" placeholder="Phone Number" fullWidth v-model="recordData.phoneNumber" />
        </div>

        <!-- Input field for address -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-address" class="text-sm font-medium text-gray-700">Address</label>
          <Input id="overwrite-address" type="text" placeholder="Address" fullWidth v-model="recordData.address" />
        </div>

        <!-- Input field for city -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-city" class="text-sm font-medium text-gray-700">City</label>
          <Input id="overwrite-city" type="text" placeholder="City" fullWidth v-model="recordData.city" />
        </div>

        <!-- Input field for state -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-state" class="text-sm font-medium text-gray-700">State</label>
          <Input id="overwrite-state" type="text" placeholder="State" fullWidth v-model="recordData.state" />
        </div>

        <!-- Input field for zip code -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-zip" class="text-sm font-medium text-gray-700">Zip Code</label>
          <Input id="overwrite-zip" type="text" placeholder="Zip Code" fullWidth v-model="recordData.zipCode" />
        </div>

        <!-- Input field for country -->
        <div class="flex flex-col gap-1.5">
          <label for="overwrite-country" class="text-sm font-medium text-gray-700">Country</label>
          <Input id="overwrite-country" type="text" placeholder="Country" fullWidth v-model="recordData.country" />
        </div>

        <!-- Input field for source -->
        <!-- <div class="flex flex-col gap-1.5">
          <label for="overwrite-source" class="text-sm font-medium text-gray-700">Source</label>
          <Input id="overwrite-source" type="text" placeholder="Source" fullWidthv-model="recordData.source" />
        </div> -->


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
      confirmButton="Save changes"
    />
    <ConfirmSuccessModal
      :open="showSuccessModal"
      :message="successMessage"
      @close="closeSuccessModal"
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
import Dropdown from "../../components/dropdown/dropdown.vue"
import Select from "../../components/select/select.vue"
import ConfirmModal from "../../components/modal/confirm-modal.vue"
import { RecordTypeT } from "../../types/record"
import ConfirmSuccessModal from "../../components/modal/confirmation-success-modal.vue"
import { getAccecptedPerson,updateAccecptedPerson } from "../../services/overrrideServiece"

interface DataType {
  showRevertModal: boolean
  showSaveConfirmModal: boolean
  recordData:RecordTypeT,
  showSuccessModal: boolean
  successMessage: string,
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
    ConfirmSuccessModal,
  },
  data(): DataType {
    return {
      showRevertModal: false,
      showSaveConfirmModal: false,
      recordData:{
        address:"",
        city:"",
        country:"",
        email:"",
        first_name:"",
        last_name:"",
        phoneNumber:"",
        state:"",
        source:"",
      zipCode:"",
      id:""
      },
      showSuccessModal: false,
      successMessage: "",
    }
  },
  created: function () {
    this.getInitList();

  },
  methods: {
    closeSuccessModal() {
      this.showSuccessModal = false
    },
   async getInitList(){
      const params = this.$route.params;
      const idS = params.record_id;
      var dataResult=await getAccecptedPerson(idS.toString());
      var data=dataResult.data;
      this.recordData={
            id: data.id,
            first_name: data.firstName,
            last_name: data.lastName,
            email: data.email,
            phoneNumber: data.phoneNumber,
            address: data.address,
            city: data.city,
            state: data.state,
            zipCode: data.zipCode,
            country: data.country,
            source:"",

      };
      console.log(this.recordData);
    },
    onBack() {
      const record_id = this.$route.params.record_id
      this.$router.push(`/record/${record_id}/review-conflict`)
    },
    onRevertChanges() {
      console.log("onRevertChanges")
    },
    async onSave() {
     var res=await updateAccecptedPerson(
     {
      address:this.recordData.address,
        city:this.recordData.city,
        country:this.recordData.country,
        email:this.recordData.email,
        firstName:this.recordData.first_name,
        lastName:this.recordData.last_name,
        phoneNumber:this.recordData.phoneNumber,
        state:this.recordData.state,
      zipCode:this.recordData.zipCode,
      id:this.recordData.id
     } 
     );
     if(res.data.isSuccessful){
     this.showSaveConfirmModal=false;
      this.successMessage = `<span class="font-semibold">Information has been updated.`
      this.showSuccessModal = true;
     }
     else{
      this.showSaveConfirmModal=false;
      alert("Save Failed");
     }
      console.log("onSave")
    },
  },
}
</script>
