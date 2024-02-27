<template>
  <component
    :is="iconComponent"
    :class="['inline-block min-w-6 min-h-6', _class]"
  />
</template>

<script lang="ts">
import SearchIcon from "./icon-search.vue"
import ChevronDownIcon from "./icon-chevron-down.vue"
import FileOpenIcon from "./icon-file-open.vue"
import TimerIcon from "./icon-timer.vue"
import UserGroupIcon from "./icon-user-group.vue"
import IconDownload from "./icon-download.vue"
import IconBarsFilter from "./icon-bars-filter.vue"
import IconCircleInfo from "./icon-circle-info.vue"
import IconCheckbox from "./icon-checkbox.vue"
import IconCircleCheckbox from "./icon-circle-checkbox.vue"
import IconCircleCheckbox2 from "./icon-circle-checkbox-2.vue"
import IconCloudUpload from "./icon-cloud-upload.vue"
import IconAddFolder from "./icon-add-folder.vue"
import IconFolder from "./icon-folder.vue"
import IconPencil from "./icon-pencil.vue"
import IconTrash from "./icon-trash.vue"
import IconBackSpace from "./icon-back-space.vue"
import IconUndo from "./icon-undo.vue"
import IconWarning from "./icon-warning.vue"
import IconVerified from "./icon-verified.vue"
import IconUnverified from "./icon-unverified.vue"
import IconChevronUpDown from "./icon-chevron-up-down.vue"

const icons: any = {
  search: SearchIcon,
  "chevron-down": ChevronDownIcon,
  "file-open": FileOpenIcon,
  timer: TimerIcon,
  "user-group": UserGroupIcon,
  download: IconDownload,
  "bars-filter": IconBarsFilter,
  "circle-info": IconCircleInfo,
  checkbox: IconCheckbox,
  circleCheckbox: IconCircleCheckbox,
  "circle-checkbox-2": IconCircleCheckbox2,
  cloudUpload: IconCloudUpload,
  addFolder: IconAddFolder,
  folder: IconFolder,
  pencil: IconPencil,
  trash: IconTrash,
  back: IconBackSpace,
  undo: IconUndo,
  warning: IconWarning,
  verified: IconVerified,
  unverified: IconUnverified,
  chevronUpDown: IconChevronUpDown,
}

export default {
  props: {
    class: String,
    name: {
      type: String,
      required: true,
      validator(value: any) {
        return Object.prototype.hasOwnProperty.call(icons, value)
      },
    },
  },

  computed: {
    _class() {
      return this.class
    },
    iconComponent() {
      return icons[this.name]
    },
  },
}
</script>
