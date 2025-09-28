import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuestionService, { QuestionDto } from "../services/QuestionService";
import AnswerService, { AnswerCreateDto } from "../services/AnswerService";
import { Button, Form, Alert } from "react-bootstrap";

const AddAnswerPage: React.FC = () => {
  const { questionId } = useParams<{ questionId: string }>();
  const navigate = useNavigate();
  const [question, setQuestion] = useState<QuestionDto | null>(null);

  // State za razlicite tipove pitanja
  const [answers, setAnswers] = useState<{ text: string; isCorrect: boolean }[]>([
    { text: "", isCorrect: false },
    { text: "", isCorrect: false },
    { text: "", isCorrect: false },
    { text: "", isCorrect: false },
  ]);
  const [textAnswer, setTextAnswer] = useState("");
  const [isCorrect, setIsCorrect] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (questionId) {
      QuestionService.getQuestionById(Number(questionId))
        .then(setQuestion)
        .catch(() => setError("Failed to load question"));
    }
  }, [questionId]);

  const handleAnswerChange = (index: number, field: "text" | "isCorrect", value: any) => {
    setAnswers((prev) => {
      const updated = [...prev];
      if (field === "isCorrect" && question?.type === "SingleChoice") {
        // Samo jedan tačan odgovor kod SingleChoice
        updated.forEach((a, i) => (a.isCorrect = i === index));
      } else {
        updated[index] = { ...updated[index], [field]: value };
      }
      return updated;
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!questionId) return;

    try {
      if (question?.type === "MultipleChoice" || question?.type === "SingleChoice") {
        for (const a of answers) {
          if (!a.text.trim()) continue;
          const dto: AnswerCreateDto = {
            questionId: Number(questionId),
            text: a.text,
            isCorrect: a.isCorrect,
          };
          await AnswerService.createAnswer(dto);
        }
      } else {
        const dto: AnswerCreateDto = {
          questionId: Number(questionId),
          text: textAnswer,
          isCorrect,
        };
        await AnswerService.createAnswer(dto);
      }
      navigate(`/quiz/${question?.quizId}`);
    } catch (err: any) {
      setError(err.message || "Failed to add answer");
    }
  };

  if (!question) return <p>Loading question...</p>;

  const renderFormFields = () => {
    switch (question.type) {
      case "MultipleChoice":
      case "SingleChoice":
        return (
          <>
            {answers.map((a, i) => (
              <div key={i} className="mb-3 border p-2 rounded">
                <Form.Group className="mb-2">
                  <Form.Label>Answer {i + 1}</Form.Label>
                  <Form.Control
                    type="text"
                    value={a.text}
                    onChange={(e) => handleAnswerChange(i, "text", e.target.value)}
                  />
                </Form.Group>
                <Form.Check
                  type={question.type === "SingleChoice" ? "radio" : "checkbox"}
                  name="correctAnswer"
                  label="Correct"
                  checked={a.isCorrect}
                  onChange={(e) => handleAnswerChange(i, "isCorrect", e.target.checked)}
                />
              </div>
            ))}
          </>
        );
      case "FillIn":
        return (
          <Form.Group className="mb-3">
            <Form.Label>Correct Answer</Form.Label>
            <Form.Control
              type="text"
              value={textAnswer}
              onChange={(e) => setTextAnswer(e.target.value)}
            />
          </Form.Group>
        );
      case "TrueFalse":
        return (
          <Form.Group className="mb-3">
            <Form.Label>Answer</Form.Label>
            <div>
              <Form.Check
                type="radio"
                label="True"
                name="truefalse"
                checked={isCorrect === true}
                onChange={() => setIsCorrect(true)}
              />
              <Form.Check
                type="radio"
                label="False"
                name="truefalse"
                checked={isCorrect === false}
                onChange={() => setIsCorrect(false)}
              />
            </div>
          </Form.Group>
        );
      default:
        return <Alert variant="warning">Unknown question type</Alert>;
    }
  };

  return (
    <div className="p-3">
      <h1>Add Answer for: {question.text}</h1>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form onSubmit={handleSubmit}>
        {renderFormFields()}
        <Button type="submit" className="mt-3">
          Save Answer
        </Button>
      </Form>
    </div>
  );
};

export default AddAnswerPage;
