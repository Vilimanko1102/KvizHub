// EditQuizPage.tsx
import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuizService from "../services/QuizService";
import { Container, Form, Button, Spinner, Alert } from "react-bootstrap";
import { QuizFormState } from "../models/Quiz";



export default function EditQuizPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState<QuizFormState>({
    title: "",
    description: "",
    category: "",
    difficulty: "Easy",
    timeLimit: 5,
    isPlayable: false,
  });
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchQuiz = async () => {
      if (!id) return;
      try {
        const data = await QuizService.getQuizById(Number(id));
        setQuiz({
          title: data.title,
          description: data.description,
          category: data.category,
          difficulty: data.difficulty,
          timeLimit: data.timeLimit,
          isPlayable: data.isPlayable,
        });
      } catch (err) {
        console.error(err);
        setError("Failed to load quiz.");
      } finally {
        setLoading(false);
      }
    };

    fetchQuiz();
  }, [id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
  const { name, type, value } = e.target;

  // checkbox
  if (type === "checkbox") {
    const target = e.target as HTMLInputElement; // cast za checkbox
    setQuiz(prev => ({
      ...prev,
      [name]: target.checked,
    }));
  } else if (type === "number") {
    setQuiz(prev => ({
      ...prev,
      [name]: Number(value),
    }));
  } else {
    setQuiz(prev => ({
      ...prev,
      [name]: value,
    }));
  }
};


  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError(null);
    try {
      await QuizService.updateQuiz(Number(id), quiz); // pretpostavljamo da postoji updateQuiz metod
      navigate("/quizzes");
    } catch (err) {
      console.error(err);
      setError("Failed to update quiz.");
    } finally {
      setSaving(false);
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
    <Container className="mt-4">
      <h2>Edit Quiz</h2>
      {error && <Alert variant="danger">{error}</Alert>}

      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3" controlId="title">
          <Form.Label>Title</Form.Label>
          <Form.Control
            type="text"
            name="title"
            value={quiz.title}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3" controlId="description">
          <Form.Label>Description</Form.Label>
          <Form.Control
            as="textarea"
            name="description"
            value={quiz.description}
            onChange={handleChange}
            rows={3}
          />
        </Form.Group>

        <Form.Group className="mb-3" controlId="category">
          <Form.Label>Category</Form.Label>
          <Form.Control
            type="text"
            name="category"
            value={quiz.category}
            onChange={handleChange}
          />
        </Form.Group>

        <Form.Group className="mb-3" controlId="difficulty">
          <Form.Label>Difficulty</Form.Label>
          <Form.Select
            name="difficulty"
            value={quiz.difficulty}
            onChange={handleChange}
          >
            <option value="Easy">Easy</option>
            <option value="Medium">Medium</option>
            <option value="Hard">Hard</option>
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3" controlId="timeLimit">
          <Form.Label>Time Limit (minutes)</Form.Label>
          <Form.Control
            type="number"
            name="timeLimit"
            value={quiz.timeLimit}
            onChange={handleChange}
            min={1}
            required
          />
        </Form.Group>

        <Button variant="primary" type="submit" disabled={saving}>
          {saving ? "Saving..." : "Save Changes"}
        </Button>
      </Form>
    </Container>
  );
}
