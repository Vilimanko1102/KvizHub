// src/pages/UserProfilePage.tsx
import React, { useEffect, useState } from "react";
import { Container, Card, ListGroup, Spinner, Alert } from "react-bootstrap";
import UserService from "../services/authService";
import QuizAttemptService from "../services/QuizAttemptService";

interface UserDto {
  id: number;
  username: string;
  email: string;
  role: string;
}

interface QuizAttemptDto {
  id: number;
  quizTitle: string;
  score: number;
  percentage: number;
  finishedAt: string;
  timeSpent: number;
}

const UserProfilePage: React.FC = () => {
  const [user, setUser] = useState<UserDto | null>(null);
  const [attempts, setAttempts] = useState<QuizAttemptDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchUserData = async () => {
    try {
      const userData = await UserService.getUserById(Number(localStorage.getItem("userId"))); // npr. /api/users/me
      setUser(userData);

      const attemptsData = await QuizAttemptService.getAttemptsByUserId(
        userData.id
      );
      setAttempts(attemptsData);
    } catch (err) {
      console.error("Error fetching user data:", err);
      setError("Failed to load user profile.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUserData();
  }, []);

  if (loading)
    return (
      <Container className="mt-4 text-center">
        <Spinner animation="border" />
      </Container>
    );

  if (error) return <Alert variant="danger">{error}</Alert>;
  if (!user) return <Alert variant="warning">User not found.</Alert>;

  return (
    <Container className="mt-4">
      <Card className="mb-4 shadow-sm">
        <Card.Body>
          <Card.Title>User Profile</Card.Title>
          <p>
            <strong>Username:</strong> {user.username}
          </p>
          <p>
            <strong>Email:</strong> {user.email}
          </p>
          <p>
            <strong>Role:</strong> {user.role}
          </p>
        </Card.Body>
      </Card>

      <h4>Quiz Attempts</h4>
      {attempts.length === 0 ? (
        <Alert variant="info">No quiz attempts found.</Alert>
      ) : (
        <ListGroup>
          {attempts.map((attempt) => (
            <ListGroup.Item key={attempt.id} className="d-flex justify-content-between">
              <div>
                <strong>{attempt.quizTitle}</strong> <br />
                <span>Score: {attempt.score}</span><br />
                 Percentage: {attempt.percentage.toFixed(2)}%<br/>
                 Time spent: {attempt.timeSpent} seconds<br/>
              </div>
              <small className="text-muted">
                Finished at:<br/>
                {new Date(attempt.finishedAt).toLocaleString()}
              </small>
            </ListGroup.Item>
          ))}
        </ListGroup>
      )}
    </Container>
  );
};

export default UserProfilePage;
