import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { apiService, ApiError, type StudentDetails, type StudentGrade } from '@/services/api'

export const useStudentGradesStore = defineStore('studentGrades', () => {
  // State
  const students = ref<StudentDetails[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const lastFetchTime = ref<Date | null>(null)

  // Getters
  const totalStudents = computed(() => students.value.length)
  
  const passingStudents = computed(() => 
    students.value.filter(student => student.hasPassedYear)
  )
  
  const failingStudents = computed(() => 
    students.value.filter(student => !student.hasPassedYear)
  )
  
  const passRate = computed(() => {
    if (totalStudents.value === 0) return 0
    return Math.round((passingStudents.value.length / totalStudents.value) * 100)
  })

  // Get all unique lessons from all students
  const allLessons = computed(() => {
    const lessonMap = new Map<number, string>()
    
    students.value.forEach(student => {
      student.grades.forEach(grade => {
        lessonMap.set(grade.lessonId, grade.lessonName)
      })
    })
    
    return Array.from(lessonMap.entries())
      .map(([id, name]) => ({ id, name }))
      .sort((a, b) => a.name.localeCompare(b.name))
  })

  // Get student by ID
  const getStudentById = computed(() => {
    return (id: number) => students.value.find(student => student.id === id)
  })

  // Get grades for a specific lesson across all students
  const getGradesByLesson = computed(() => {
    return (lessonId: number) => {
      const grades: Array<{ student: StudentDetails; grade: StudentGrade | null }> = []
      
      students.value.forEach(student => {
        const grade = student.grades.find(g => g.lessonId === lessonId) || null
        grades.push({ student, grade })
      })
      
      return grades
    }
  })

  // Actions
  async function fetchAllStudents() {
    loading.value = true
    error.value = null
    
    try {
      const data = await apiService.getAllStudentsWithGrades()
      students.value = data
      lastFetchTime.value = new Date()
    } catch (err) {
      if (err instanceof ApiError) {
        error.value = `Failed to fetch students: ${err.message}`
      } else {
        error.value = 'An unexpected error occurred while fetching students'
      }
      console.error('Error fetching students:', err)
    } finally {
      loading.value = false
    }
  }

  async function fetchStudentById(id: number) {
    loading.value = true
    error.value = null
    
    try {
      const student = await apiService.getStudentGrades(id)
      
      // Update or add the student in the store
      const existingIndex = students.value.findIndex(s => s.id === id)
      if (existingIndex >= 0) {
        students.value[existingIndex] = student
      } else {
        students.value.push(student)
      }
      
      return student
    } catch (err) {
      if (err instanceof ApiError) {
        error.value = `Failed to fetch student ${id}: ${err.message}`
      } else {
        error.value = 'An unexpected error occurred while fetching student'
      }
      console.error('Error fetching student:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  function clearError() {
    error.value = null
  }

  function reset() {
    students.value = []
    loading.value = false
    error.value = null
    lastFetchTime.value = null
  }

  return {
    // State
    students,
    loading,
    error,
    lastFetchTime,
    
    // Getters
    totalStudents,
    passingStudents,
    failingStudents,
    passRate,
    allLessons,
    getStudentById,
    getGradesByLesson,
    
    // Actions
    fetchAllStudents,
    fetchStudentById,
    clearError,
    reset
  }
})
