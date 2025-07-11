
export const API_BASE_URL = 'http://localhost:5089/api'

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

// API Error handling
export class ApiError extends Error {
  constructor(
    message: string,
    public status?: number,
    public response?: Response
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

class ApiService {
  private baseUrl: string

  constructor(baseUrl: string = API_BASE_URL) {
    this.baseUrl = baseUrl
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      const errorMessage = `HTTP ${response.status}: ${response.statusText}`
      throw new ApiError(errorMessage, response.status, response)
    }

    const contentType = response.headers.get('content-type')
    if (contentType && contentType.includes('application/json')) {
      return response.json()
    }

    throw new ApiError('Invalid response format - expected JSON')
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`
    
    const defaultOptions: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
    }

    try {
      const response = await fetch(url, { ...defaultOptions, ...options })
      return this.handleResponse<T>(response)
    } catch (error) {
      if (error instanceof ApiError) {
        throw error
      }
      throw new ApiError(`Network error: ${error instanceof Error ? error.message : 'Unknown error'}`)
    }
  }

  // Get all students with their grades
  async getAllStudentsWithGrades(): Promise<StudentDetails[]> {
    return this.request<StudentDetails[]>('/students')
  }

  // Get specific student grades
  async getStudentGrades(studentId: number): Promise<StudentDetails> {
    return this.request<StudentDetails>(`/students/${studentId}`)
  }
}

// Export singleton instance
export const apiService = new ApiService()
