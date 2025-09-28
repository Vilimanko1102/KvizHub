import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuestionService from "../services/QuestionService";
import { Button } from "react-bootstrap";

interface QuestionDto {
  id: number;
  text: string;
  type: string;
  points: number;
  answers?: AnswerDto[];
}

interface AnswerDto {
  id: number;
  text: string;
  isCorrect: boolean;
}

const QuizPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [questions, setQuestions] = useState<QuestionDto[]>([]);

  const isAdmin = localStorage.getItem("userRole") === "Admin";

  useEffect(() => {
    if (id) {
      QuestionService.getQuestionsByQuizId(Number(id))
        .then(setQuestions)
        .catch((err) => console.error("Error loading questions", err));
    }
  }, [id]);

  const handleAddAnswer = (questionId: number) => {
    navigate(`/admin/question/${questionId}/add-answer`);
  };

  return (
    <div className="p-3">
      <h1>Quiz #{id}</h1>

      {questions.length === 0 && <p>No questions found for this quiz.</p>}

      {questions.map((q) => (
        <div
          key={q.id}
          className="border rounded p-3 mb-3 d-flex justify-content-between"
        >
          <div>
            <strong>{q.text}</strong>
            <div className="text-muted">
              Type: {q.type} | Points: {q.points}
            </div>

            {/* Prikaži sve odgovore ako postoje */}
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

          {/* Samo admin vidi dugme */}
          {isAdmin && (!q.answers || q.answers.length === 0) && (
            <Button onClick={() => handleAddAnswer(q.id)}>Dodaj odgovor</Button>
          )}
        </div>
      ))}
    </div>
  );
};

export default QuizPage;
