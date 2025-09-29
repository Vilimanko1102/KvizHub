import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL_ANSWER;

export interface AnswerCreateDto {
    questionId: number;
    text: string;
    isCorrect: boolean;
}

export interface AnswerDto {
    id: number;
    questionId: number;
    text: string;
    isCorrect: boolean;
}

class AnswerService {
      private getAuthHeaders() {
    const token = localStorage.getItem("token");
    return {
      Authorization: `Bearer ${token}`,
    };
  }

  async deleteAnswer(answerId: number): Promise<void> {
    await axios.delete(`${API_URL}/${answerId}`, {
      headers: this.getAuthHeaders(),
    });
  }

    async createAnswer(dto: AnswerCreateDto): Promise<AnswerDto> {
        const response = await axios.post(`${API_URL}`, dto, {
            headers: this.getAuthHeaders(),
        });
        return response.data;
    }

    async getAnswersByQuestionId(questionId: number) {
    const response = await axios.get(`${API_URL}/question/${questionId}`, {
      headers: this.getAuthHeaders(),
    });
    return response.data;
  }
}

export default new AnswerService();
