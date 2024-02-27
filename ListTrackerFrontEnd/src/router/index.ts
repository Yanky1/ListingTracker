import { createWebHistory, createRouter } from "vue-router"
import DashboardVue from "../views/dashboard/dashboard.vue"
import ActivitiesVue from "../views/activities/activities.vue"
import CategoriesVue from "../views/categories/categories.vue"
import CategoryFilesVue from "../views/files/files.vue"
import reviewConflictVue from "../views/review-conflict/review-conflict.vue"
import overwriteProfileVue from "../views/overwrite-profile/overwrite-profile.vue"

const routes = [
  {
    path: "/",
    name: "Home",
    redirect: "record",
  },
  {
    path: "/record",
    children: [
      {
        path: "",
        component: DashboardVue,
      },
      {
        path: ":record_id/review-conflict",
        component: reviewConflictVue,
      },
      {
        path: ":record_id/overwrite",
        component: overwriteProfileVue,
      },
    ],
  },
  {
    path: "/activities",
    name: "Activities",
    component: ActivitiesVue,
  },
  {
    path: "/categories",
    children: [
      {
        path: "",
        component: CategoriesVue,
      },
      {
        path: ":category_id/files",
        component: CategoryFilesVue,
      },
    ],
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
