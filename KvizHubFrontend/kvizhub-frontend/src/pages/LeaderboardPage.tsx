import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Table, Spinner, Alert, Form, Row, Col, Button } from "react-bootstrap";
import QuizAttemptService from "../services/QuizAttemptService";
import UserService from "../services/authService";
import { User } from "../models/User";
import { Quiz } from "../models/Quiz";
import QuizService from "../services/QuizService";

interface QuizAttemptDto {
  id: number;
  userId: number;
  score: number;
  timeSpent: number;
  percentage: number;
  finishedAt: string;
  quizTitle: string;
}

const LeaderboardPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [attempts, setAttempts] = useState<QuizAttemptDto[]>([]);
  const [quiz, setQuiz] = useState<Quiz>();
  const [filteredAttempts, setFilteredAttempts] = useState<QuizAttemptDto[]>([]);
  const [userMap, setUserMap] = useState<Record<number, string>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [quizFilter, setQuizFilter] = useState("");
  const [timeFilter, setTimeFilter] = useState("all");

  useEffect(() => {
    const fetchAttempts = async () => {
      if (!id) return;
      try {
        setLoading(true);
        const data = (await QuizAttemptService.getAttemptsByQuizId(Number(id))) as QuizAttemptDto[];
        const data1 = (await QuizService.getQuizById(Number(id))) as Quiz;
        console.log(data1)
        setQuiz(data1)
        // sortiraj pokušaje
        const sorted = [...data].sort((a, b) => {
          if (b.percentage === a.percentage) {
            return a.timeSpent - b.timeSpent;
          }
          return b.percentage - a.percentage;
        });

        setAttempts(sorted);
        setFilteredAttempts(sorted);

        const uniqueUserIds = Array.from(new Set(sorted.map((a) => a.userId)));
        const users = await Promise.all(uniqueUserIds.map((uid) => UserService.getUserById(uid)));
        const map: Record<number, string> = {};
        users.forEach((u) => {
          map[u.id] = u.username;
        });
        setUserMap(map);
      } catch (err) {
        console.error(err);
        setError("Failed to load leaderboard data.");
      } finally {
        setLoading(false);
      }
    };

    fetchAttempts();
  }, [id]);

  // Filter funkcija
  useEffect(() => {
    const now = new Date();
    let filtered = [...attempts];

    if (quizFilter.trim()) {
      filtered = filtered.filter((a) =>
        a.quizTitle.toLowerCase().includes(quizFilter.toLowerCase())
      );
    }

    if (timeFilter !== "all") {
      filtered = filtered.filter((a) => {
        const finished = new Date(a.finishedAt);
        switch (timeFilter) {
          case "today":
            return (
              finished.getFullYear() === now.getFullYear() &&
              finished.getMonth() === now.getMonth() &&
              finished.getDate() === now.getDate()
            );
          case "week": {
            const firstDayOfWeek = new Date(now);
            firstDayOfWeek.setDate(now.getDate() - now.getDay());
            return finished >= firstDayOfWeek;
          }
          case "month":
            return (
              finished.getFullYear() === now.getFullYear() &&
              finished.getMonth() === now.getMonth()
            );
        }
        return true;
      });
    }

    setFilteredAttempts(filtered);
  }, [quizFilter, timeFilter, attempts]);

  if (loading) return <Spinner animation="border" className="mt-4" />;

  if (error) return <Alert variant="danger" className="mt-4">{error}</Alert>;

  if (filteredAttempts.length === 0)
    return <Alert variant="info" className="mt-4">No attempts found.</Alert>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4 text-center">{quiz?.title} Leaderboard</h2>

      <Form className="mb-4">
        <Row className="align-items-end">

          <Col md={3}>
            <Form.Group controlId="timeFilter">
              <Form.Label>Filter by Time</Form.Label>
              <Form.Select value={timeFilter} onChange={(e) => setTimeFilter(e.target.value)}>
                <option value="all">All time</option>
                <option value="today">Today</option>
                <option value="week">This Week</option>
                <option value="month">This Month</option>
              </Form.Select>
            </Form.Group>
          </Col>

          <Col md={2}>
            <Button variant="primary" onClick={() => {}}>Apply</Button>
          </Col>
        </Row>
      </Form>

      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>#</th>
            <th>User</th>
            <th>Score</th>
            <th>Time spent</th>
            <th>Percentage</th>
            <th>Completed At</th>
          </tr>
        </thead>
        <tbody>
          {filteredAttempts.map((attempt, index) => (
            <tr key={attempt.id}>
              <td>{index + 1}.</td>
              <td>{userMap[attempt.userId] ?? "Loading..."}</td>
              <td>{attempt.score}</td>
              <td>{attempt.timeSpent} sec</td>
              <td>{attempt.percentage.toFixed(2)}%</td>
              <td>{new Date(attempt.finishedAt).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default LeaderboardPage;
