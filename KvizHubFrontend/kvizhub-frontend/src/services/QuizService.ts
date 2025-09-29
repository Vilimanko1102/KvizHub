import axios from "axios";
import { QuizCreateDto, Quiz, QuizUpdateDto } from "../models/Quiz";
import { AnswerDto } from "./AnswerService";

export interface QuizWithQuestionsDto {
  id: number;
  title: string;
  description: string;
  category: string;
  difficulty: string;
  timeLimit: number;
  questionCount: number;
  isPlayable: boolean;
  questions: QuestionDtoWithAnswers[];
}

export interface QuestionDtoWithAnswers {
  id: number;
  quizId: number;
  text: string;
  type: string;
  points: number;
  answers: AnswerDto[];
}


const API_URL = process.env.REACT_APP_API_URL_QUIZ; // prilagodi ako ti je bek na drugom portu ili ruti

class QuizService {

  private getAuthHeaders() {
    const token = localStorage.getItem("token");
    return token ? { Authorization: `Bearer ${token}` } : {};
  }

  async getAllQuizzes(): Promise<Quiz[]> {
    const response = await axios.get(`${API_URL}`);
    return response.data;
  }

  async getQuizById(id: number): Promise<Quiz> {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  }

  async createQuiz(quizData: QuizCreateDto, token: string): Promise<Quiz> {
    console.log(quizData)
    const response = await axios.post(`${API_URL}`, quizData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  }

  async updateQuiz(id: number, quizData: QuizUpdateDto): Promise<Quiz> {
    const response = await axios.put(`${API_URL}/${id}`, quizData, {
      headers: this.getAuthHeaders(),
    });
    return response.data;
  }

  async deleteQuiz(id: number) {
    const response = await axios.delete(`${API_URL}/${id}`, {
      headers: this.getAuthHeaders(),
    });
    return response.data;
  }

  async getQuizByIdWithQuestions(id: number): Promise<QuizWithQuestionsDto> {
    const response = await fetch(`${API_URL}/${id}/full`);
    return response.json();
}
}

export default new QuizService();
