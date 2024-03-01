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
            <!-- {{headCell.field}} -->
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
                <Radio :id="`${row.id}-${index}`"  :name="`${row.number}${headCell.field}`" class="uncheckRadio" @click="handleRadiodata(row.id,row.acid,headCell.field,row[headCell.field])" />
                <label
                  :for="`${row.id}-${index}`"
                  class="cursor-pointer line-clamp-2"
                  >{{ row[headCell.field] }}</label
                >
              </span>
            </template>
            <template v-else-if="typeof row[headCell.field] === 'string'">
              <Radio :id="`${row.id}-${index}`" :label="row[headCell.field]" :name="`${row.number}${headCell.field}`" class="uncheckRadio" @click="handleRadiodata(row.id,row.acid,headCell.field,row[headCell.field])"/>
            </template>
          </div>
        </td>

        <td v-if="tableActions.length > 0">
          <TableActions :tableActions="tableActions" @click="handleClick(row)"/>
        </td>
      </tr>
    </tbody>
  </table>

  <div class="flex">
    <div class="flex gap-2 py-4 px-4">
      <Button color="white" @click="handleRevert" >
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
import { updateConflict,deleteperson } from "../../services/dashboardService"


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
      sourceTracer:[] as any[],
    }
  },
  methods: {
    async handleClick(row:any){
      var data=await deleteperson(row.id);
      if(data.data.isSuccessful){
        this.successMessage = `<span class="font-semibold">Information has been deleted.`
      this.showSuccessModal = true
      }
    },
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
      window.location.reload();
      this.showSuccessModal = false
    },
    async onReplaceConfirm() {
      if(this.sourceTracer.length>0){
        console.log("handle replacing");
      const response = await updateConflict(this.sourceTracer);
      if(response.data.isSuccessful){
      this.handleRevert();
      this.showReplaceConfirmModal = false
      this.successMessage = `<span class="font-semibold">Information has been updated.`
      this.showSuccessModal = true
      }
      }
      else{
        alert("Please select data before submitting");
      }
     

    },
    handleRevert(){
      const radioButtons = document.querySelectorAll('input[type="radio"]');
      radioButtons.forEach((radio: any) => {
        radio.checked = false;
      });
      this.sourceTracer=[];
    },
    handleRadiodata(idR:any, personIdR:any, fildNameR:any, fieldvalueR:any) {
    var vm = this;

    if (vm.sourceTracer.length === 0) {
        vm.sourceTracer.push({
            acceptedPersonId:  personIdR,
            personId: idR,
            fieldName: fildNameR,
            fieldValue: fieldvalueR
        });
    } else {
        if (vm.sourceTracer[0].acceptedPersonId === personIdR) {
            const existingFieldIndex = vm.sourceTracer.findIndex(s => s.fieldName === fildNameR);
            if (existingFieldIndex !== -1) {
                vm.sourceTracer[existingFieldIndex].fieldValue = fieldvalueR;
            } else {
                vm.sourceTracer.push({
                  acceptedPersonId:  personIdR,
                    personId: idR,
                    fieldName: fildNameR,
                    fieldValue: fieldvalueR
                });
            }
        } else {
            vm.sourceTracer = [{
              acceptedPersonId:  personIdR,
              personId: idR,
                fieldName: fildNameR,
                fieldValue: fieldvalueR
            }];
        }
    }

    console.log(vm.sourceTracer);
}

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
