import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuestionService from "../services/QuestionService";
import { Button, Spinner } from "react-bootstrap";
import { Quiz } from "../models/Quiz";
import QuizService from "../services/QuizService";
import { QuestionDto } from "../models/Question";





const QuizPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [questions, setQuestions] = useState<QuestionDto[]>([]);
  const [quiz, setQuiz] = useState<Quiz>();
  const [loading, setLoading] = useState(true);

  const isAdmin = localStorage.getItem("userRole") === "Admin";

  const fetchQuestions = async () => {
    if (id) {
      try {
        const data = await QuestionService.getQuestionsByQuizId(Number(id));
        setQuestions(data);
        const data1 = await QuizService.getQuizById(Number(id));
        console.log(data1)
        setQuiz(data1);
      } catch (err) {
        console.error("Error loading questions", err);
      }
      finally{
        setLoading(false)
      }
    }
  };

  useEffect(() => {
    fetchQuestions();
  }, [id]);

  const handleAddAnswer = (questionId: number) => {
    navigate(`/admin/question/${questionId}/add-answer`);
  };

  const handleUpdateQuestion = (questionId: number) => {
    navigate(`/admin/question/${questionId}/edit`);
  };

  const handleDeleteQuestion = async (questionId: number) => {
    if (window.confirm("Are you sure you want to delete this question?")) {
      try {
        await QuestionService.deleteQuestion(questionId);
        alert("Question deleted successfully!");
        fetchQuestions(); // osveži listu pitanja
      } catch (err) {
        console.error("Failed to delete question", err);
        alert("Failed to delete question.");
      }
    }
  };

    if (loading) {
    return (
      <div className="d-flex justify-content-center mt-5">
        <Spinner animation="border" />
      </div>
    );
  }

  return (
    <div className="p-3">
      {!quiz}
      <h1>{quiz?.title}</h1>

      {questions.length === 0 && <p>No questions found for this quiz.</p>}

      {questions.map((q) => (
        <div
          key={q.id}
          className="border rounded p-3 mb-3 d-flex justify-content-between align-items-start"
        >
          <div>
            <strong>{q.text}</strong>
            <div className="text-muted">
              Type: {q.type} | Points: {q.points}
            </div>

            {q.answers && q.answers.length > 0 ? (
              <ul>
                {q.answers.map((a) => (
                  <li key={a.id}>
                    {a.text} {a.isCorrect && <strong>(✔)</strong>}
                  </li>
                ))}
              </ul>
            ) : (
              <p className="text-warning">Ovo pitanje nema odgovore.</p>
            )}
          </div>

          {isAdmin && (
            <div className="d-flex flex-column gap-2">
              <Button onClick={() => handleUpdateQuestion(q.id)} variant="warning">
                Edit
              </Button>
              <Button onClick={() => handleDeleteQuestion(q.id)} variant="danger">
                Delete
              </Button>
              {(!q.answers || q.answers.length === 0) && (
                <Button onClick={() => handleAddAnswer(q.id)} variant="info">
                  Dodaj odgovor
                </Button>
              )}
            </div>
          )}
        </div>
      ))}
    </div>
  );
};

export default QuizPage;
