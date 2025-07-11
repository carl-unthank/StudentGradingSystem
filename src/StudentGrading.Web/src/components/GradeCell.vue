<template>
  <div class="grade-cell">
    <span
      v-if="grade"
      :class="['grade-badge', grade.isPassing ? 'passing' : 'failing']"
    >
      {{ grade.letterGrade }}
    </span>
    <span v-else class="no-grade">-</span>
  </div>
</template>

<script setup lang="ts">
import type { StudentGrade } from '@/services/api'

interface Props {
  grade: StudentGrade | null
}

defineProps<Props>()
</script>

<style scoped>
.grade-cell {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 2rem;
}

.grade-badge {
  display: inline-block;
  padding: 0.25rem 0.5rem;
  border-radius: 0.25rem;
  font-weight: 600;
  font-size: 0.875rem;
  text-align: center;
  min-width: 2rem;
}

.grade-badge.passing {
  background-color: #10b981;
  color: white;
}

.grade-badge.failing {
  background-color: #ef4444;
  color: white;
}

.no-grade {
  color: #9ca3af;
  font-style: italic;
}
</style>
