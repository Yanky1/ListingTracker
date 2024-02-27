<template lang="">
  <Header />
  <main class="pt-[85px]">
    <slot />
  </main>
</template>
<script lang="ts">
import Header from "../../header/header.vue"

export default {
  components: {
    Header,
  },
}
</script>
<style lang=""></style>
