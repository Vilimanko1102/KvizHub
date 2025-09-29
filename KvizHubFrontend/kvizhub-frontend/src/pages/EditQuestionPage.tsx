import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuestionService from "../services/QuestionService";
import { Form, Button, Container, Alert } from "react-bootstrap";
import AnswerService from "../services/AnswerService";

export interface QuestionUpdateDto {
  id: number;
  text: string;
  type: "SingleChoice" | "MultipleChoice" | "FillIn";
  points: number;
  quizId: number;
}

const EditQuestionPage: React.FC = () => {
  const { questionId } = useParams<{ questionId: string }>();
  const navigate = useNavigate();
  const [question, setQuestion] = useState<QuestionUpdateDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!questionId) {
      setLoading(false);
      return;
    }

    QuestionService.getQuestionById(Number(questionId))
      .then((q: any) => {
        // mapiramo odgovor u QuestionUpdateDto oblik
        const mapped: QuestionUpdateDto = {
          id: q.id,
          text: q.text ?? "",
          type: q.type as "SingleChoice" | "MultipleChoice" | "FillIn",
          points: q.points ?? 1,
          quizId: q.quizId ?? 0,
        };
        setQuestion(mapped);
      })
      .catch((err) => {
        console.error(err);
        setError("Failed to load question");
      })
      .finally(() => setLoading(false));
  }, [questionId]);

  // koristimo React.ChangeEvent<any> zbog react-bootstrap Form.Control tipova
  const handleChange = (e: React.ChangeEvent<any>) => {
    const { name, value } = e.target;
    setQuestion((prev) =>
      prev ? { ...prev, [name]: name === "points" ? Number(value) : value } : prev
    );
  };

  const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  if (!question) return;

  try {
    // 1. Update pitanja
    await QuestionService.updateQuestion(question.id, question);

    // 2. Učitaj sve odgovore za to pitanje
    const answers = await AnswerService.getAnswersByQuestionId(question.id);

    // 3. Obrisi sve odgovore paralelno
    await Promise.all(answers.map((a: any) => AnswerService.deleteAnswer(a.id)));

    alert("Question updated and old answers deleted!");
    navigate(`/admin/question/${question.id}/add-answer`);
  } catch (err) {
    console.error(err);
    setError("Failed to update question");
  }
};

  if (loading) return <p>Loading...</p>;
  if (!question) return <p>Question not found.</p>;

  return (
    <Container className="mt-4">
      <h2>Edit Question</h2>
      {error && <Alert variant="danger">{error}</Alert>}

      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3" controlId="text">
          <Form.Label>Question Text</Form.Label>
          <Form.Control
            type="text"
            name="text"
            value={question.text}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3" controlId="type">
          <Form.Label>Type</Form.Label>
          <Form.Select
            name="type"
            value={question.type}
            onChange={handleChange}
            required
          >
            <option value="SingleChoice">Single Choice</option>
            <option value="MultipleChoice">Multiple Choice</option>
            <option value="FillIn">Fill In</option>
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3" controlId="points">
          <Form.Label>Points</Form.Label>
          <Form.Control
            type="number"
            name="points"
            value={question.points}
            onChange={handleChange}
            required
            min={1}
          />
        </Form.Group>

        <Button variant="primary" type="submit">
          Update Question
        </Button>
      </Form>
    </Container>
  );
};

export default EditQuestionPage;
