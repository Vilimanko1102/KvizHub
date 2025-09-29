import axios from "axios";
import { AnswerDto } from "./AnswerService";

const API_URL = process.env.REACT_APP_API_URL_QUESTION;

export interface QuestionWithAnswersDto {
    id: number;
    quizId: number;
    text: string;
    type: string;
    points: number;
    answers: AnswerDto[];
}

export interface QuestionCreateDto {
    quizId: number;
    text: string;
    type: string;
    points: number;
}

export interface QuestionDto {
    id: number;
    quizId: number;
    text: string;
    type: string;
    points: number;
}

class QuestionService {
    private getAuthHeaders() {
        const token = localStorage.getItem("token");
        return token ? { Authorization: `Bearer ${token}` } : {};
    }

    async createQuestion(dto: QuestionCreateDto): Promise<QuestionDto> {
        console.log(dto)
        const response = await axios.post(`${API_URL}`, dto, {
            headers: this.getAuthHeaders(),
        });
        return response.data;
    }

    async getQuestionsByQuizId(quizId: number): Promise<QuestionDto[]> {
        const response = await axios.get(`${API_URL}/quiz/${quizId}`, {
            headers: this.getAuthHeaders(),
        });
        return response.data;
    }

    async getQuestionById(id: number): Promise<QuestionDto> {
        const response = await axios.get(`${API_URL}/${id}`, {
            headers: this.getAuthHeaders(),
        });
        return response.data;
    }
}

export default new QuestionService();
