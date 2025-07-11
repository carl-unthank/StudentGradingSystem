<template>
  <div class="bg-white/80 backdrop-blur-sm rounded-2xl shadow-lg border border-white/20 p-6 mb-8">
    <div class="flex flex-col sm:flex-row gap-6">
      <div class="flex flex-col sm:w-64">
        <label for="statusFilter" class="text-sm font-semibold text-gray-700 mb-3 flex items-center">
          <svg class="w-4 h-4 mr-2 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z"></path>
          </svg>
          Filter by Status
        </label>
        <select
          id="statusFilter"
          :value="statusFilter"
          @change="$emit('update:statusFilter', ($event.target as HTMLSelectElement).value)"
          class="px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 bg-white/90 backdrop-blur-sm shadow-sm transition-all duration-200 hover:shadow-md font-medium"
        >
          <option value="all">All Students</option>
          <option value="passing">Passing Only</option>
          <option value="failing">Failing Only</option>
        </select>
      </div>

      <div class="flex flex-col flex-1">
        <label for="searchFilter" class="text-sm font-semibold text-gray-700 mb-3 flex items-center">
          <svg class="w-4 h-4 mr-2 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
          </svg>
          Search Students
        </label>
        <input
          id="searchFilter"
          :value="searchFilter"
          @input="$emit('update:searchFilter', ($event.target as HTMLInputElement).value)"
          type="text"
          placeholder="Search by name..."
          class="px-4 py-3 border border-gray-200 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 bg-white/90 backdrop-blur-sm shadow-sm transition-all duration-200 hover:shadow-md font-medium placeholder-gray-400"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  statusFilter: 'all' | 'passing' | 'failing'
  searchFilter: string
}

defineProps<Props>()

defineEmits<{
  'update:statusFilter': [value: 'all' | 'passing' | 'failing']
  'update:searchFilter': [value: string]
}>()
</script>
