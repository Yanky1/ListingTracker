<template>
  <Layout>
    <Topbar>
      <div class="flex justify-between gap-[25px]">
        <div class="w-full max-w-[625px]">
          <SearchInput
            v-model="searchText"
            placeholder="Search categories by any keyword"
          />
        </div>

        <Button
          variant="outlined"
          class="py-[15px] ring-[#226BFF]"
          @click="handleClick"
        >
          <span class="text-nowrap"> Add new category </span>
          <Icon name="addFolder" />
        </Button>
      </div>
    </Topbar>

    <div class="container mx-auto py-[65px]">
      <div
        v-if="filteredCategories.length"
        class="p-6 rounded-lg border w-fit mx-auto border-neutral-500 grid grid-cols-4 gap-x-11 gap-y-6 flex-wrap"
      >
        <CategoryCard
          :key="`category-${index}`"
          v-for="(category, index) of filteredCategories"
          :name="category.categoryName"
          :id="category.id"
          :level="category.level"
          @click="handleCardClick(category.id)"
          @edit="(id) => handleEdit(id, index)"
          @remove="(id) => handleRemove(id)"
        />
      </div>
      <template v-else>
        <div class="text-center">No Categories</div>
      </template>
    </div>

    <CreateCategoryModal
      :open="showAddModal"
      :category="category"
      @create="handleCreate"
      @close="handleClose"
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
import CreateCategoryModal from "../../components/modal/create-category-modal.vue"
import CategoryCard from "../../components/category/category-card.vue"
import { CategoryType } from "../../types/category"
import { getCategoryList, addCategory, updateCategory, deleteCategory } from "../../services/categoryServices"

interface DataType {
  searchText: string
  categories: CategoryType[]
  showAddModal: boolean
  category: CategoryType | null
}

export default {
  components: {
    Header,
    Topbar,
    Layout,
    SearchInput,
    Button,
    Icon,
    CreateCategoryModal,
    CategoryCard,
  },
  data(): DataType {
    return {
      searchText: "",
      category: null,
      categories: [],
      showAddModal: false,
    }
  },
  created: function () {
    this.getInitCategoryList();
  },
  computed: {
    filteredCategories(): CategoryType[] {
      if (!this.searchText) {
        return this.categories;
      } else {
        const searchTextLower = this.searchText.toLowerCase();
        return this.categories.filter(category =>
          category.categoryName.toLowerCase().includes(searchTextLower)
        );
      }
    }
  },
  methods: {
    async getInitCategoryList() {
      try {
        const response = await getCategoryList();
        const categoriesData = response.data.data;
        this.categories = categoriesData.map((category: any) => ({
          id: category.id.toString(),
          categoryName: category.categoryName,
          level: category.categoryLevel
        }));
      } catch (error) {
        console.error("Error fetching category data:", error);
      }
    },
    handleClick() {
      this.category = null
      this.showAddModal = true
    },
    handleClose() {
      this.showAddModal = false
    },
    async handleCreate(category: any) {
      if (category?.id) {
        var response = await updateCategory(category);
        if (response.data.isSuccessful) {
          var categoryResult = response.data.data;
          this.categories = this.categories.map((ca) => {
            if (ca.id === category.id) {
              return { ...ca, categoryName: category.name, level: category.level }
            } else {
              return ca
            }
          })
        }

      } else {
        var response = await addCategory(category);
        if (response.data.isSuccessful) {
          var categoryResult = response.data.data;
          this.categories.push({
            id: categoryResult.id,
            categoryName: categoryResult.categoryName,
            level: categoryResult.categoryLevel
          })
        }

      }
      this.showAddModal = false
    },
    async handleRemove(id: string) {
      var response = await deleteCategory(id);
      if (response.data == undefined) {
        alert(response.response.data);
      }
      else if (response.data.isSuccessful) {
        this.categories = this.categories.filter((category) => category.id !== id)
      }
    },
    handleEdit(id: string, index: number) {
      this.showAddModal = true
      this.category = this.categories[index]
      console.log(id);
    },
    handleCardClick(id: string) {
      this.$router.push(`/categories/${id}/files`)
    },
  },
}
</script>
