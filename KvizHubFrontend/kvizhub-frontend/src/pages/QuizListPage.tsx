// QuizListPage.tsx
import { useEffect, useState } from "react";
import QuizService, { Quiz } from "../services/QuizService";
import QuizCard from "../components/QuizCard";
import { Container, Row, Col, Button, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext"; // uzimamo user iz konteksta

export default function QuizListPage() {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const { user } = useAuth();

  const isAdmin = localStorage["userRole"] === "Admin";

  useEffect(() => {
    const fetchQuizzes = async () => {
      try {
        console.log(localStorage["userRole"])
        console.log(isAdmin)
        const data = await QuizService.getAllQuizzes();
        setQuizzes(data);
      } catch (error) {
        console.error("Greška pri učitavanju kvizova:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchQuizzes();
  }, []);

  const handleAddQuiz = () => {
    navigate("/quizzes/create");
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
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Lista kvizova</h1>
        {isAdmin && (
          <Button variant="primary" onClick={handleAddQuiz}>
            + Dodaj kviz
          </Button>
        )}
      </div>

      <Row xs={1} sm={2} md={3} className="g-4">
        {quizzes.map((quiz) => (
          <Col key={quiz.id}>
            <QuizCard
              id={quiz.id}
              title={quiz.title}
              description={quiz.description}
              questionCount={10} // placeholder
              difficulty={
                quiz.difficulty === "EASY"
                  ? "Easy"
                  : quiz.difficulty === "MEDIUM"
                  ? "Medium"
                  : "Hard"
              }
              timeLimit={5} // placeholder
            />
          </Col>
        ))}
      </Row>
    </Container>
  );
}
