
import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL_QUIZATTEMPT;

export interface UserAnswerSubmitDto {
  questionId: number;
  selectedAnswerIds?: number[];
  textAnswer?: string;
}

export interface QuizAttemptSubmitDto {
  quizId: number;
  userId: number;
  userAnswers: UserAnswerSubmitDto[];
  timeSpent: number
}

export interface UserAnswerDto {
  id: number;
  questionId: number;
  selectedAnswerIds?: number[] | string;
  textAnswer?: string;
  isCorrect: boolean;
}

export interface QuizAttemptDto {
  id: number;
  quizId: number;
  userId: number;
  score: number;
  percentage: number;
  timeSpent: number;
  startedAt: string;
  finishedAt: string;
  quizTitle: string;
  userAnswers?: UserAnswerDto[];
}

class QuizAttemptService {
  private getAuthHeaders() {
    const token = localStorage.getItem("token");
    return token ? { Authorization: `Bearer ${token}` } : {};
  }

 async submitQuizAnswers(dto: QuizAttemptSubmitDto): Promise<QuizAttemptDto> {
  const response = await axios.post(`${API_URL}`, dto, { headers: this.getAuthHeaders() });
  console.log(response.data)
  return response.data; // backend treba da vrati ceo QuizAttemptDto
}

  async getAttemptById(attemptId: number) {
  const response = await axios.get(`${API_URL}/${attemptId}`, {
    headers: this.getAuthHeaders(),
  });
  return response.data;
}

  async getAttemptsByUserId(userId: number) {
    const response = await axios.get(`${API_URL}/user/${userId}`, {
      headers: this.getAuthHeaders(),
    });
    return response.data;
  }

  async getAttemptsByQuizId(quizId: number) {
    const response = await axios.get(`${API_URL}/quiz/${quizId}`, {
      headers: this.getAuthHeaders(),
    });
    return response.data;
  }
}

export default new QuizAttemptService();
