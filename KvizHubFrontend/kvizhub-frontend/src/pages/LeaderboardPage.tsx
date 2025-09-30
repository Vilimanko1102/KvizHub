import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Table, Spinner, Alert } from "react-bootstrap";
import QuizAttemptService from "../services/QuizAttemptService";
import UserService from "../services/authService";
import { User } from "../models/User";

interface QuizAttemptDto {
  id: number;
  userId: number;
  score: number;
  timeSpent: number;
  percentage: number;
  finishedAt: string;
}

const LeaderboardPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [attempts, setAttempts] = useState<QuizAttemptDto[]>([]);
  const [userMap, setUserMap] = useState<Record<number, string>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchAttempts = async () => {
      if (!id) return;
      try {
        setLoading(true);
        const data = (await QuizAttemptService.getAttemptsByQuizId(
          Number(id)
        )) as QuizAttemptDto[];

        // sortiraj pokušaje
        const sorted = [...data].sort((a, b) => {
          if (b.percentage === a.percentage) {
            return a.timeSpent - b.timeSpent;
          }
          return b.percentage - a.percentage;
        });

        setAttempts(sorted);

        // uzmi jedinstvene userId-jeve
        const uniqueUserIds = Array.from(new Set(sorted.map((a) => a.userId)));

        // paralelno dohvati sve korisnike
        const users = await Promise.all(
          uniqueUserIds.map((uid) => UserService.getUserById(uid))
        );

        // napravi mapu userId -> userName
        const map: Record<number, string> = {};
        users.forEach((u) => {
          map[u.id] = u.username; // pretpostavljam da User ima id i userName
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

  if (loading) return <Spinner animation="border" className="mt-4" />;

  if (error) return <Alert variant="danger" className="mt-4">{error}</Alert>;

  if (attempts.length === 0)
    return <Alert variant="info" className="mt-4">No attempts found for this quiz.</Alert>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4 text-center">Quiz Leaderboard</h2>
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
          {attempts.map((attempt, index) => (
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
