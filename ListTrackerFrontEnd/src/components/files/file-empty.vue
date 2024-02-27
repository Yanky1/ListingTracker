<template lang="">
  <div class="flex flex-col gap-5 w-full max-w-[500px] mx-auto">
    <img
      src="/src/assets/images/no-results.png"
      class="w-[200px] mx-auto"
      alt="no result"
    />
    <Button @click="$emit('click')" class="bg-[#226BFF]"
      >Upload new file <Icon name="cloudUpload"
    /></Button>
  </div>
</template>
<script lang="ts">
import Button from "../button/button.vue"
import Icon from "../icons/base-icon.vue"

export default {
  components: {
    Button,
    Icon,
  },
  $emit: ["click"],
}
</script>
<style lang=""></style>
