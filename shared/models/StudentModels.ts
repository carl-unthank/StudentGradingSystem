// Shared types that match the C# DTOs exactly
// These should be kept in sync with the C# DTOs in StudentGrading.Api/Models/DTOs.cs

export interface StudentGrade {
  studentId: number
  lessonId: number
  lessonName: string
  letterGrade: string
  isPassing: boolean
}

export interface StudentDetails {
  id: number
  name: string
  grades: StudentGrade[]
  hasPassedYear: boolean
}
