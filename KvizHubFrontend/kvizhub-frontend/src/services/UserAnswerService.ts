import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL_USERANSWER;

export interface UserAnswerDto {
  id: number;
  questionId: number;
  selectedAnswerIdsCsv?: string; // CSV string iz baze
  textAnswer?: string;
  isCorrect: boolean;
}

class UserAnswerService {
  private getAuthHeaders() {
    const token = localStorage.getItem("token");
    return token ? { Authorization: `Bearer ${token}` } : {};
  }

  async getByAttemptId(attemptId: number): Promise<UserAnswerDto[]> {
    const response = await axios.get(`${API_URL}/quizAttempt/${attemptId}`, {
      headers: this.getAuthHeaders(),
    });
    console.log(response.data)
    return response.data;
  }
}

export default new UserAnswerService();
