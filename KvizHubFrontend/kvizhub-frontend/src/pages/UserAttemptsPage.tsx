import React, { useEffect, useState } from "react";
import { Table, Button, Spinner, Alert } from "react-bootstrap";
import QuizAttemptService from "../services/QuizAttemptService";
import { useNavigate } from "react-router-dom";

interface QuizAttemptDto {
  id: number;
  quizTitle: string;
  score: number;
  percentage: number;
  finishedAt: string;
  timeSpent: number;
}

const UserAttemptsPage: React.FC = () => {
  const [attempts, setAttempts] = useState<QuizAttemptDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const navigate = useNavigate();


  useEffect(() => {
    const fetchAttempts = async () => {
      try {
        setLoading(true);
        const data = await QuizAttemptService.getAttemptsByUserId(Number(localStorage.getItem("userId")));
        setAttempts(data);
        console.log(data)
      } catch (err) {
        console.error(err);
        setError("Failed to load your attempts.");
      } finally {
        setLoading(false);
      }
    };

    fetchAttempts();
  }, []);

  if (loading) return <Spinner animation="border" className="mt-4" />;
  if (error) return <Alert variant="danger" className="mt-4">{error}</Alert>;
  if (attempts.length === 0) return <Alert variant="info" className="mt-4">You haven't attempted any quizzes yet.</Alert>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4 text-center">Moji pokušaji</h2>
      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>#</th>
            <th>Quiz</th>
            <th>Score</th>
            <th>Percentage</th>
            <th>Time spent</th>
            <th>Completed At</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {attempts.map((attempt, index) => (
            <tr key={attempt.id}>
              <td>{index + 1}</td>
              <td>{attempt.quizTitle}</td>
              <td>{attempt.score}</td>
              <td>{attempt.percentage.toFixed(2)}%</td>
              <td>{attempt.timeSpent} sec</td>
              <td>{new Date(attempt.finishedAt).toLocaleString()}</td>
              <td>
                <Button
                  variant="info"
                  size="sm"
                  onClick={() => navigate(`/quiz/${attempt.id}/details`)}
                >
                  Prikaži detalje
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default UserAttemptsPage;
