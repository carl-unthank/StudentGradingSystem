<template>
  <div class="max-w-full">
    <!-- Loading State -->
    <div v-if="loading" class="flex flex-col items-center justify-center py-24 text-center">
      <div class="relative">
        <div class="w-16 h-16 border-4 border-blue-100 border-t-blue-600 rounded-full animate-spin mb-6"></div>
        <div class="absolute inset-0 w-16 h-16 border-4 border-transparent border-r-indigo-400 rounded-full animate-spin animation-delay-150"></div>
      </div>
      <p class="text-gray-600 text-lg font-medium">Loading student grades...</p>
      <p class="text-gray-400 text-sm mt-2">Please wait while we fetch the latest data</p>
    </div>

    <div v-else-if="error" class="flex justify-center py-16">
      <div class="text-center p-8 border border-red-200 rounded-2xl bg-gradient-to-br from-red-50 to-rose-50 shadow-lg max-w-md">
        <div class="inline-flex items-center justify-center w-16 h-16 bg-red-100 rounded-full mb-4">
          <svg class="w-8 h-8 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z"></path>
          </svg>
        </div>
        <h3 class="text-xl font-bold text-red-800 mb-2">Error Loading Data</h3>
        <p class="text-red-600 mb-6">{{ error }}</p>
        <button @click="retry" class="px-6 py-3 bg-gradient-to-r from-red-600 to-rose-600 text-white rounded-xl hover:from-red-700 hover:to-rose-700 transition-all duration-200 font-semibold shadow-lg hover:shadow-xl">
          Try Again
        </button>
      </div>
    </div>

    <div v-else-if="students.length > 0" class="space-y-8">
      <SummaryStats :students="students" />

      <StudentFilters
        v-model:status-filter="statusFilter"
        v-model:search-filter="searchFilter"
      />

      <div class="bg-white/80 backdrop-blur-sm rounded-2xl shadow-xl border border-white/20 overflow-hidden">
        <div class="bg-gradient-to-r from-slate-50 to-gray-50 px-6 py-4 border-b border-gray-200">
          <h2 class="text-lg font-bold text-gray-900 flex items-center">
            <svg class="w-5 h-5 mr-2 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01"></path>
            </svg>
            Student Performance Overview
          </h2>
        </div>
        <div class="overflow-x-auto">
          <table class="w-full">
            <thead class="bg-gradient-to-r from-gray-50 to-slate-50">
              <tr>
                <th class="px-6 py-5 text-left text-sm font-bold text-gray-900 uppercase tracking-wider">Student</th>
                <th class="px-6 py-5 text-left text-sm font-bold text-gray-900 uppercase tracking-wider">Grades</th>
                <th class="px-6 py-5 text-center text-sm font-bold text-gray-900 uppercase tracking-wider">Passing Year</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-100">
              <tr
                v-for="student in filteredStudents"
                :key="student.id"
                :class="student.hasPassedYear
                  ? 'hover:bg-gradient-to-r hover:from-green-50 hover:to-emerald-50 transition-all duration-200'
                  : 'bg-gradient-to-r from-red-50 to-rose-50 hover:from-red-100 hover:to-rose-100 transition-all duration-200'"
              >
                <td class="px-6 py-5">
                  <div class="flex items-center">
                    <div class="flex-shrink-0 h-10 w-10">
                      <div class="h-10 w-10 rounded-full bg-gradient-to-r from-blue-500 to-indigo-600 flex items-center justify-center">
                        <span class="text-sm font-bold text-white">{{ student.name.split(' ').map(n => n[0]).join('') }}</span>
                      </div>
                    </div>
                    <div class="ml-4">
                      <div class="text-sm font-bold text-gray-900">{{ student.name }}</div>
                    </div>
                  </div>
                </td>
                <td class="px-6 py-5">
                  <div class="flex flex-wrap gap-2">
                    <span
                      v-for="grade in student.grades"
                      :key="grade.lessonId"
                      :class="grade.isPassing
                        ? 'inline-flex px-3 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800 border border-green-200'
                        : 'inline-flex px-3 py-1 text-xs font-semibold rounded-full bg-red-100 text-red-800 border border-red-200'"
                    >
                      {{ grade.lessonName }}: {{ grade.letterGrade }}
                    </span>
                    <span v-if="student.grades.length === 0" class="text-sm text-gray-500 italic">No grades available</span>
                  </div>
                </td>
                <td class="px-6 py-5 text-center">
                  <span
                    :class="student.hasPassedYear
                      ? 'inline-flex items-center px-4 py-2 text-sm font-bold rounded-full bg-gradient-to-r from-green-500 to-emerald-600 text-white shadow-lg'
                      : 'inline-flex items-center px-4 py-2 text-sm font-bold rounded-full bg-gradient-to-r from-red-500 to-rose-600 text-white shadow-lg'"
                  >
                    <svg v-if="student.hasPassedYear" class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
                    </svg>
                    <svg v-else class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                    </svg>
                    {{ student.hasPassedYear ? 'Passed' : 'Failed' }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <div v-else class="text-center py-24">
      <div class="max-w-md mx-auto">
        <div class="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-r from-gray-100 to-slate-100 rounded-full mb-6">
          <svg class="w-10 h-10 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
          </svg>
        </div>
        <h3 class="text-2xl font-bold text-gray-900 mb-3">No Student Data</h3>
        <p class="text-gray-600 mb-8 leading-relaxed">No student grades are available at this time. Please check back later or contact your administrator.</p>
        <button @click="retry" class="px-8 py-3 bg-gradient-to-r from-blue-600 to-indigo-600 text-white rounded-xl hover:from-blue-700 hover:to-indigo-700 transition-all duration-200 font-semibold shadow-lg hover:shadow-xl">
          Refresh Data
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useStudentGradesStore } from '@/stores/studentGrades'
import type { StudentDetails, StudentGrade } from '@/services/api'
import SummaryStats from './SummaryStats.vue'
import StudentFilters from './StudentFilters.vue'

const store = useStudentGradesStore()
const { students, loading, error } = storeToRefs(store)

const statusFilter = ref<'all' | 'passing' | 'failing'>('all')
const searchFilter = ref('')

const filteredStudents = computed(() => {
  let filtered = students.value

  if (statusFilter.value === 'passing') {
    filtered = filtered.filter(s => s.hasPassedYear)
  } else if (statusFilter.value === 'failing') {
    filtered = filtered.filter(s => !s.hasPassedYear)
  }

  if (searchFilter.value.trim()) {
    const search = searchFilter.value.toLowerCase()
    filtered = filtered.filter(s =>
      s.name.toLowerCase().includes(search)
    )
  }

  return filtered
})

function getGradeForLesson(student: StudentDetails, lessonId: number): StudentGrade | null {
  return student.grades.find(g => g.lessonId === lessonId) || null
}

function retry() {
  store.fetchAllStudents()
}

onMounted(() => {
  if (store.students.length === 0) {
    store.fetchAllStudents()
  }
})
</script>



