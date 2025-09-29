// QuizListPage.tsx
import { useEffect, useState } from "react";
import QuizService from "../services/QuizService";
import { Quiz } from "../models/Quiz";
import QuizCard from "../components/QuizCard";
import { Container, Row, Col, Button, Spinner, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function QuizListPage() {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [filteredQuizzes, setFilteredQuizzes] = useState<Quiz[]>([]);
  const [loading, setLoading] = useState(true);

  // filter state
  const [selectedCategory, setSelectedCategory] = useState<string>("");
  const [selectedDifficulty, setSelectedDifficulty] = useState<string>("");
  const [searchTerm, setSearchTerm] = useState<string>("");

  const navigate = useNavigate();
  const { user } = useAuth();

  const isAdmin = localStorage["userRole"] === "Admin";

  useEffect(() => {
    const fetchQuizzes = async () => {
      try {
        const data = await QuizService.getAllQuizzes();
        setQuizzes(data);
        setFilteredQuizzes(data);
      } catch (error) {
        console.error("Greška pri učitavanju kvizova:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchQuizzes();
  }, []);

  const onDeleted = async () =>{
    const updatedQuizzes = await QuizService.getAllQuizzes();
    setQuizzes(updatedQuizzes); 
  }

  const handleAddQuiz = () => {
    navigate("/quizzes/create");
  };

  const applyFilters = () => {
    let result = quizzes;

    if (selectedCategory) {
      result = result.filter((q) => q.category === selectedCategory);
    }

    if (selectedDifficulty) {
      result = result.filter((q) => q.difficulty === selectedDifficulty);
    }

    if (searchTerm.trim()) {
      const term = searchTerm.toLowerCase();
      result = result.filter(
        (q) =>
          q.title.toLowerCase().includes(term) ||
          q.description.toLowerCase().includes(term)
      );
    }

    setFilteredQuizzes(result);
  };

  const handleResetFilters = () => {
    setSelectedCategory("");
    setSelectedDifficulty("");
    setSearchTerm("");
    setFilteredQuizzes(quizzes);
  };

  // automatski filtriraj kada se promeni filter ili search
  useEffect(() => {
    applyFilters();
  }, [selectedCategory, selectedDifficulty, searchTerm, quizzes]);

  if (loading) {
    return (
      <div className="d-flex justify-content-center mt-5">
        <Spinner animation="border" />
      </div>
    );
  }

  const categories = Array.from(new Set(quizzes.map((q) => q.category)));

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

      {/* Filter forma */}
      <Form className="mb-4 d-flex flex-wrap gap-3 align-items-end">
        <Form.Group controlId="search" style={{ minWidth: "200px" }}>
          <Form.Label>Pretraga</Form.Label>
          <Form.Control
            type="text"
            placeholder="Pretraži po naslovu ili opisu..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </Form.Group>

        <Form.Group controlId="categoryFilter">
          <Form.Label>Kategorija</Form.Label>
          <Form.Select
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
          >
            <option value="">Sve kategorije</option>
            {categories.map((cat) => (
              <option key={cat} value={cat}>
                {cat}
              </option>
            ))}
          </Form.Select>
        </Form.Group>

        <Form.Group controlId="difficultyFilter">
          <Form.Label>Težina</Form.Label>
          <Form.Select
            value={selectedDifficulty}
            onChange={(e) => setSelectedDifficulty(e.target.value)}
          >
            <option value="">Sve težine</option>
            <option value="Easy">Easy</option>
            <option value="Medium">Medium</option>
            <option value="Hard">Hard</option>
          </Form.Select>
        </Form.Group>

        <Button variant="outline-secondary" onClick={handleResetFilters}>
          Resetuj
        </Button>
      </Form>

      <Row xs={1} sm={2} md={3} className="g-4">
        {filteredQuizzes.length > 0 ? (
          filteredQuizzes.map((quiz) => (
            <Col key={quiz.id}>
              <QuizCard
                id={quiz.id}
                title={quiz.title}
                category={quiz.category}
                description={quiz.description}
                questionCount={quiz.questionCount}
                difficulty={quiz.difficulty}
                timeLimit={quiz.timeLimit}
                isPlayable={quiz.isPlayable}
                onDeleted={onDeleted}
              />
            </Col>
          ))
        ) : (
          <p className="text-center text-muted">
            Nema kvizova koji odgovaraju pretrazi ili filterima.
          </p>
        )}
      </Row>
    </Container>
  );
}
