<template lang="">
  <td :class="['h-10 p-4', _class]">
    <div class="flex flex-col gap-2">
      <div class="flex gap-1 justify-between">
        <span class="text-red font-bold text-sm">{{ `#${index + 1}` }}</span>

        <span class="flex gap-1 items-center text-xxs">
          <Icon
            name="file-open"
            class="min-w-[15px] w-[15px] min-h-[15px] h-[15px] text-primary-950"
          />
          <span class="text-black leading-normal">
            From:
            <span class="text-[#6B6969]">{{ rows.source[index] }}</span>
          </span>
        </span>
      </div>
      <div class="p-[13px] border border-[#3F444D] rounded text-sm">
        {{ value }}
      </div>
      <div class="text-xs">
        <Radio :id="`${field}-${index}`" :label="label" :name="field" @click="test(rows.id[index],value,field)"/>
      </div>
    </div>
  </td>
</template>

<script lang="ts">
import Icon from "../../components/icons/base-icon.vue"
import Radio from "../../components/radio/radio.vue"

export default {
  props: {
    class: String,
    index: { type: Number, required: true },
    value: { type: String, required: true },
    rows: { type: Object, required: true },
    field: { type: String, required: true },
  },
  components: {
    Icon,
    Radio,
  },
  computed: {
    _class() {
      return this.class
    },
    label() {
      return `This is the main <span class="text-primary font-bold">${this.field}</span>`
    },
  },
  methods:{
    test(row:any,value:any,field:any){
      this.$emit('radioS',row,value,field.replace(' ','_'));
      //console.log(row,value,field.replace(' ',''));
    }
  }

  
}
</script>
<style lang="scss"></style>
