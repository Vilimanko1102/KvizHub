import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import QuizService from "../services/QuizService";
import { QuizCreateDto } from "../models/Quiz";

const CreateQuizPage: React.FC = () => {
  const navigate = useNavigate();
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [category, setCategory] = useState("");
  const [difficulty, setDifficulty] = useState<"Easy" | "Medium" | "Hard">("Easy");
  const [timeLimit, setTimeLimit] = useState<number>(10);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const createdBy = localStorage["userId"]

    const token = localStorage.getItem("token"); // ili iz AuthContext
    if (!token) {
      setError("Niste ulogovani.");
      return;
    }

    const quizData: QuizCreateDto = {
      title,
      description,
      category,
      difficulty,
      timeLimit,
      createdBy
    };

    try {
      await QuizService.createQuiz(quizData, token);
      navigate("/quizzes"); // nakon kreiranja vrati na listu kvizova
    } catch (err) {
      setError("Greška pri kreiranju kviza.");
      console.error(err);
    }
  };

  return (
    <div className="container mt-4">
      <h2>Create New Quiz</h2>
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Title</label>
          <input
            type="text"
            className="form-control"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Category</label>
          <input
            type="text"
            className="form-control"
            value={category}
            onChange={(e) => setCategory(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Difficulty</label>
          <select
            className="form-select"
            value={difficulty}
            onChange={(e) => setDifficulty(e.target.value as "Easy" | "Medium" | "Hard")}
          >
            <option value="Easy">Easy</option>
            <option value="Medium">Medium</option>
            <option value="Hard">Hard</option>
          </select>
        </div>

        <div className="mb-3">
          <label className="form-label">Time Limit (minutes)</label>
          <input
            type="number"
            className="form-control"
            value={timeLimit}
            min={1}
            onChange={(e) => setTimeLimit(parseInt(e.target.value))}
            required
          />
        </div>

        <button type="submit" className="btn btn-primary">
          Create Quiz
        </button>
      </form>
    </div>
  );
};

export default CreateQuizPage;
