<script lang="ts">
import Button from "../button/button.vue"
import NavLink from "./nav/nav-link.vue"
import BaseIcon from "../icons/base-icon.vue"

export default {
  components: {
    Button,
    NavLink,
    BaseIcon,
  },
}
</script>

<template>
  <header class="py-[22px] fixed min-h-[85px] w-full bg-white z-50">
    <div class="container mx-auto flex gap-[65px] items-center">
      <h3 class="text-[23px] text-primary-950 font-semibold">
        Record conflict dashboard
      </h3>

      <ul class="flex gap-[38px] items-center">
        <li>
          <NavLink to="/record"
            ><BaseIcon name="user-group" /> Student record</NavLink
          >
        </li>

        <li>
          <NavLink to="/categories"
            ><BaseIcon name="file-open" /> File categories</NavLink
          >
        </li>

        <li>
          <NavLink to="/activities"
            ><BaseIcon name="timer" /> Activities</NavLink
          >
        </li>
      </ul>
    </div>
  </header>
</template>
