import axios from "axios";
import { QuizCreateDto, Quiz, QuizUpdateDto } from "../models/Quiz";



const API_URL = process.env.REACT_APP_API_URL_QUIZ; // prilagodi ako ti je bek na drugom portu ili ruti

class QuizService {
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

  async updateQuiz(id: number, quizData: QuizUpdateDto, token: string): Promise<Quiz> {
    const response = await axios.put(`${API_URL}/${id}`, quizData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  }

  async deleteQuiz(id: number, token: string): Promise<void> {
    await axios.delete(`${API_URL}/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}

export default new QuizService();
