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
        return token ? { Authorization: `Bearer ${token}` } : {};
    }

    async createAnswer(dto: AnswerCreateDto): Promise<AnswerDto> {
        const response = await axios.post(`${API_URL}`, dto, {
            headers: this.getAuthHeaders(),
        });
        return response.data;
    }
}

export default new AnswerService();
