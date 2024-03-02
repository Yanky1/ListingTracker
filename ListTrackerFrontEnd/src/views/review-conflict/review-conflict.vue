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
            <RecordCompareTable :records="records" @radioSelected="myRadio" @dtl3="deleteClickConfirm"/>
          </div>
        </div>

        <div class="flex flex-col items-center">
          <ActivityCard :activities="activities" />
          <div class="flex gap-2 mt-[50px]">
            <Button color="white" @click="revertChange">
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
import {  RecordTypeT } from "../../types/record"
import ActivityCard from "../../components/activity/activity-card.vue"
import { ActivityType } from "../../components/activity/activity-item.vue"
import ReplaceConfirmModal from "../../components/modal/confirm-modal.vue"
import ConfirmSuccessModal from "../../components/modal/confirmation-success-modal.vue"
import RecordCompareTable from "../../components/conflict-compare-table/conflict-compare-table.vue"
import { getPersonById,updateConflict ,deleteperson} from "../../services/dashboardService"
import { getLogConflict } from "../../services/activityServices"
import moment from "moment"

interface DataType {
  records: RecordTypeT[]
  activities: ActivityType[]
  showReplaceConfirmModal: boolean
  showSuccessModal: boolean
  successMessage: string,
  sourceTracer:any[]
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
      sourceTracer:[],
      activities: [
        {
          id: "",
          type: "",
          description:
            "",
          updatedAt: "",
        },
      ],
      showReplaceConfirmModal: false,
      showSuccessModal: false,
      successMessage: "",
      records: [
      {
        id: "1",
        first_name: "",
        last_name: "",
        email: "",
        phoneNumber: "",
        address: "",
        city: "",
        state: "",
        zipCode: "",
        country: "",
        source:""
      },
    //  {
    //     id: "2",
    //     first_name: "",
    //     last_name: "",
    //     email: "",
    //     phoneNumber: "",
    //     address: "",
    //     city: "",
    //     state: "",
    //     zipCode: "",
    //     country: "",
    //     source:""
    //   }
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
  created: function () {
    this.getInitInitList();
  },
  methods: {
    async deleteClickConfirm(id:any){
      var data=await deleteperson(id);
      if(data.data.isSuccessful){
        this.successMessage = `<span class="font-semibold">Information has been deleted.`
      this.showSuccessModal = true;
      this.getInitInitList();
      }
    },
    myRadio(row:any,value:any,field:any){
      var vm = this;
      const params = this.$route.params;
      const idS = params.record_id;
    if (vm.sourceTracer.length === 0) {
        vm.sourceTracer.push({
            acceptedPersonId:  idS,
            personId: row,
            fieldName: field,
            fieldValue: value
        });
    } else {
        if (vm.sourceTracer[0].acceptedPersonId === idS) {
            const existingFieldIndex = vm.sourceTracer.findIndex(s => s.fieldName === field);
            if (existingFieldIndex !== -1) {
                vm.sourceTracer[existingFieldIndex].fieldValue = value;
            } else {
                vm.sourceTracer.push({
                  acceptedPersonId:  idS,
                    personId: row,
                    fieldName: field,
                    fieldValue: value
                });
            }
        } else {
            vm.sourceTracer = [{
              acceptedPersonId:  idS,
              personId: row,
                fieldName: field,
                fieldValue: value
            }];
        }
    }
    console.log(vm.sourceTracer);
    },
    async getInitInitList() {
        const params = this.$route.params;
        const id = params.record_id;
        const response = await getPersonById(id.toString());
        const personListData = response.data;
        console.log(personListData); // Check what is inside personListData
        this.records = personListData.map((person:any) => ({
            id: person.id=="00000000-0000-0000-0000-000000000000"?"":person.id,
            first_name: person.firstName,
            last_name: person.lastName,
            email: person.email,
            phoneNumber: person.phoneNumber,
            address: person.address,
            city: person.city,
            state: person.state,
            zipCode: person.zipCode,
            country: person.country,
            source:person.fileName
        }));
      var res=await getLogConflict(id);
      var data=res.data;
      this.activities=data.map((act:any)=>({
           id: act.id,
          type: act.logType,
          description: act.logDescription,
          updatedAt: moment(act.logDate).format('DD-MMM-YYYY'),

      }));
        console.log(this.records); // Check if you receive the correct data
    },
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
    async onReplaceConfirm() {
      if(this.sourceTracer.length>0){
        console.log("handle replacing");
      const response = await updateConflict(this.sourceTracer);
      if(response.data.isSuccessful){
      this.revertChange();
      this.showReplaceConfirmModal = false
      this.successMessage = `<span class="font-semibold">Information has been updated.`
      this.showSuccessModal = true
      }
      }
      else{
        alert("Please select data before submitting");
      }
     
    },
    onClickOverwrite() {
      const record_id = this.$route.params.record_id
      this.$router.push(`/record/${record_id}/overwrite`)
    },
    revertChange(){
      const radioButtons = document.querySelectorAll('input[type="radio"]');
      radioButtons.forEach((radio: any) => {
        radio.checked = false;
      });
      this.sourceTracer=[];
    }
  },
}
</script>
<style lang=""></style>
