<template>
  <div class="grid grid-cols-2 md:grid-cols-4 gap-6 mb-8">
    <div class="bg-white/80 backdrop-blur-sm p-6 rounded-2xl shadow-lg border border-white/20 text-center hover:shadow-xl transition-all duration-300 hover:-translate-y-1">
      <div class="inline-flex items-center justify-center w-12 h-12 bg-gradient-to-r from-slate-500 to-slate-600 rounded-xl mb-4">
        <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z"></path>
        </svg>
      </div>
      <h3 class="text-3xl font-bold text-gray-900 mb-2">{{ totalStudents }}</h3>
      <p class="text-sm text-gray-600 font-semibold uppercase tracking-wide">Total Students</p>
    </div>

    <div class="bg-white/80 backdrop-blur-sm p-6 rounded-2xl shadow-lg border border-white/20 text-center hover:shadow-xl transition-all duration-300 hover:-translate-y-1">
      <div class="inline-flex items-center justify-center w-12 h-12 bg-gradient-to-r from-emerald-500 to-green-600 rounded-xl mb-4">
        <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
        </svg>
      </div>
      <h3 class="text-3xl font-bold text-emerald-600 mb-2">{{ passingStudents }}</h3>
      <p class="text-sm text-gray-600 font-semibold uppercase tracking-wide">Passing</p>
    </div>

    <div class="bg-white/80 backdrop-blur-sm p-6 rounded-2xl shadow-lg border border-white/20 text-center hover:shadow-xl transition-all duration-300 hover:-translate-y-1">
      <div class="inline-flex items-center justify-center w-12 h-12 bg-gradient-to-r from-red-500 to-rose-600 rounded-xl mb-4">
        <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
        </svg>
      </div>
      <h3 class="text-3xl font-bold text-red-600 mb-2">{{ failingStudents }}</h3>
      <p class="text-sm text-gray-600 font-semibold uppercase tracking-wide">Failing</p>
    </div>

    <div class="bg-white/80 backdrop-blur-sm p-6 rounded-2xl shadow-lg border border-white/20 text-center hover:shadow-xl transition-all duration-300 hover:-translate-y-1">
      <div class="inline-flex items-center justify-center w-12 h-12 bg-gradient-to-r from-blue-500 to-indigo-600 rounded-xl mb-4">
        <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path>
        </svg>
      </div>
      <h3 class="text-3xl font-bold text-blue-600 mb-2">{{ passRate }}%</h3>
      <p class="text-sm text-gray-600 font-semibold uppercase tracking-wide">Pass Rate</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type { StudentDetails } from '@/services/api'

interface Props {
  students: StudentDetails[]
}

const props = defineProps<Props>()

const totalStudents = computed(() => props.students.length)
const passingStudents = computed(() => props.students.filter(s => s.hasPassedYear).length)
const failingStudents = computed(() => props.students.filter(s => !s.hasPassedYear).length)
const passRate = computed(() => {
  if (totalStudents.value === 0) return 0
  return Math.round((passingStudents.value / totalStudents.value) * 100)
})
</script>
