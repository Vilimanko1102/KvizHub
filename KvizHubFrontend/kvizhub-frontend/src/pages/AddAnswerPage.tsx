import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, Button, Alert } from "react-bootstrap";
import AnswerService, { AnswerCreateDto } from "../services/AnswerService";
import QuestionService, { QuestionDto } from "../services/QuestionService";

const AddAnswerPage: React.FC = () => {
  const { questionId } = useParams<{ questionId: string }>();
  const navigate = useNavigate();

  const [question, setQuestion] = useState<QuestionDto | null>(null);
  const [answers, setAnswers] = useState<{ text: string; isCorrect: boolean }[]>(
    [
      { text: "", isCorrect: false },
      { text: "", isCorrect: false },
      { text: "", isCorrect: false },
      { text: "", isCorrect: false },
    ]
  );
  const [fillInAnswer, setFillInAnswer] = useState("");
  const [trueFalseValue, setTrueFalseValue] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!questionId) return;
    QuestionService.getQuestionById(Number(questionId))
      .then((q) => setQuestion(q))
      .catch(() => setError("Failed to load question"));
  }, [questionId]);

  const handleAnswerChange = (index: number, field: "text" | "isCorrect", value: string | boolean) => {
    const newAnswers = [...answers];
    if (field === "text") newAnswers[index].text = value as string;
    else newAnswers[index].isCorrect = value as boolean;
    setAnswers(newAnswers);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!question) return;

    try {
      if (question.type === "SingleChoice" || question.type === "MultipleChoice") {
        for (const a of answers) {
          await AnswerService.createAnswer({
            questionId: question.id,
            text: a.text,
            isCorrect: a.isCorrect,
          });
        }
      } else if (question.type === "FillIn") {
        await AnswerService.createAnswer({
          questionId: question.id,
          text: fillInAnswer,
          isCorrect: true,
        });
      } else if (question.type === "TrueFalse") {
        // Kreiramo dva odgovora u bazi, True i False
        await AnswerService.createAnswer({
          questionId: question.id,
          text: "True",
          isCorrect: trueFalseValue,
        });
        await AnswerService.createAnswer({
          questionId: question.id,
          text: "False",
          isCorrect: !trueFalseValue,
        });
      }

      navigate(`/quiz/${question.quizId}`);
    } catch (err: any) {
      console.error(err);
      setError(err.message || "Failed to save answers");
    }
  };

  if (!question) return <p>Loading question...</p>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Add Answers for: {question.text}</h2>
      {error && <Alert variant="danger">{error}</Alert>}

      <Form onSubmit={handleSubmit}>
        {question.type === "SingleChoice" || question.type === "MultipleChoice" ? (
          <>
            {answers.map((a, index) => (
              <div key={index} className="mb-3 p-2 border rounded">
                <Form.Group className="mb-2">
                  <Form.Label>Answer {index + 1}</Form.Label>
                  <Form.Control
                    type="text"
                    value={a.text}
                    onChange={(e) =>
                      handleAnswerChange(index, "text", e.target.value)
                    }
                    required
                  />
                </Form.Group>
                <Form.Check
                  type={question.type === "SingleChoice" ? "radio" : "checkbox"}
                  name="isCorrect"
                  label="Correct?"
                  checked={a.isCorrect}
                  onChange={(e) => {
                    if (question.type === "SingleChoice") {
                      const newAnswers = answers.map((ans, i) => ({
                        ...ans,
                        isCorrect: i === index,
                      }));
                      setAnswers(newAnswers);
                    } else {
                      handleAnswerChange(index, "isCorrect", e.target.checked);
                    }
                  }}
                />
              </div>
            ))}
          </>
        ) : question.type === "FillIn" ? (
          <Form.Group className="mb-3">
            <Form.Label>Correct Answer</Form.Label>
            <Form.Control
              type="text"
              value={fillInAnswer}
              onChange={(e) => setFillInAnswer(e.target.value)}
              required
            />
          </Form.Group>
        ) : question.type === "TrueFalse" ? (
          <Form.Group className="mb-3">
            <Form.Label>Correct Answer</Form.Label>
            <Form.Select
              value={trueFalseValue ? "true" : "false"}
              onChange={(e) => setTrueFalseValue(e.target.value === "true")}
            >
              <option value="true">True</option>
              <option value="false">False</option>
            </Form.Select>
          </Form.Group>
        ) : (
          <Alert variant="warning">Unknown question type</Alert>
        )}

        <Button variant="primary" type="submit">
          Save Answers
        </Button>
      </Form>
    </div>
  );
};

export default AddAnswerPage;
