import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import QuizAttemptService, { QuizAttemptDto } from "../services/QuizAttemptService";
import QuizService, { QuizWithQuestionsDto } from "../services/QuizService";
import UserAnswerService, { UserAnswerDto } from "../services/UserAnswerService";
import { Card, Container, Form, Alert, Badge } from "react-bootstrap";
import { JSX } from "react/jsx-runtime";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

export default function QuizResultPage() {
  const { attemptId } = useParams<{ attemptId: string }>();
  const [attempt, setAttempt] = useState<QuizAttemptDto | null>(null);
  const [attempts, setAttempts] = useState<QuizAttemptDto[]>([]);
  const [quiz, setQuiz] = useState<QuizWithQuestionsDto | null>(null);
  const [userAnswers, setUserAnswers] = useState<UserAnswerDto[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      if (!attemptId) return;

      // 1. Učitaj attempt
      const attemptData = await QuizAttemptService.getAttemptById(Number(attemptId));
      setAttempt(attemptData);

      // 2. Učitaj kviz da bi imao pitanja
      const quizData = await QuizService.getQuizByIdWithQuestions(attemptData.quizId);
      setQuiz(quizData);

      // 3. Učitaj sve pokušaje korisnika i filtriraj samo za ovaj kviz
      const attemptsData = await QuizAttemptService.getAttemptsByUserId(
        Number(localStorage.getItem("userId"))
      );
      const filteredAttempts = attemptsData.filter(
        (a: QuizAttemptDto) => a.quizId === attemptData.quizId
      );
      setAttempts(filteredAttempts);

      // 4. Učitaj odgovore korisnika
      const answersData = await UserAnswerService.getByAttemptId(Number(attemptId));
      setUserAnswers(answersData);
    };

    fetchData();
  }, [attemptId]);

  if (!attempt || !quiz) return <div>Loading...</div>;

  const formatTime = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s < 10 ? "0" : ""}${s}`;
  };

  return (
    <Container className="mt-4">
      <h2>Quiz Result: {quiz.title}</h2>
      <Alert variant="info">
        Score: {attempt.score} ({attempt.percentage.toFixed(2)}%) <br />
        Time Spent: {formatTime(attempt.timeSpent)}
      </Alert>

      {/* Pitanja i odgovori */}
      {quiz.questions.map((q, index) => {
        const ua = userAnswers.find((u) => u.questionId === q.id);
        const selectedIds = ua?.selectedAnswerIdsCsv
          ? ua.selectedAnswerIdsCsv.split(",").map(Number)
          : [];

        return (
          <Card className="mb-3" key={q.id}>
            <Card.Body>
              <Card.Title>
                Question {index + 1}: {q.text}
              </Card.Title>

              <Form>
                {q.type === "FillIn" ? (
                  <Form.Control
                    type="text"
                    value={ua?.textAnswer || ""}
                    readOnly
                    className={ua?.isCorrect ? "is-valid" : "is-invalid"}
                  />
                ) : (
                  q.answers.map((a) => {
                    const isSelected = selectedIds.includes(a.id);
                    const isCorrect = a.isCorrect;

                    let badge: JSX.Element | null = null;
                    if (isSelected && isCorrect) badge = <Badge bg="success">✔</Badge>;
                    else if (isSelected && !isCorrect) badge = <Badge bg="danger">✖</Badge>;
                    else if (!isSelected && isCorrect) badge = <Badge bg="warning">✔ correct</Badge>;

                    return (
                      <Form.Check
                        key={a.id}
                        type={q.type === "MultipleChoice" ? "checkbox" : "radio"}
                        label={
                          <>
                            {a.text} {badge}
                          </>
                        }
                        checked={isSelected}
                        readOnly
                        style={{ marginBottom: "0.5rem" }}
                      />
                    );
                  })
                )}
              </Form>
            </Card.Body>
          </Card>
        );
      })}

      {/* Grafikon napretka */}
      <h3 className="mt-5">Napredak kroz pokušaje</h3>
      <div style={{ width: "100%", height: 300 }}>
        <ResponsiveContainer>
          <LineChart
            data={attempts.map((a, i) => ({
              attempt: i + 1,
              percentage: a.percentage,
              date: new Date(a.finishedAt).toLocaleString(),
            }))}
            margin={{ top: 20, right: 30, left: 20, bottom: 5 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="attempt"
              label={{ value: "Pokušaj", position: "insideBottom", offset: -5 }}
            />
            <YAxis
              domain={[0, 100]}
              label={{ value: "Procenat", angle: -90, position: "insideLeft" }}
            />
            <Tooltip
              formatter={(value: number) => `${value.toFixed(2)}%`}
              labelFormatter={(label, payload) => {
                if (payload && payload[0]) {
                  return `Pokušaj ${label} (${payload[0].payload.date})`;
                }
                return `Pokušaj ${label}`;
              }}
            />
            <Legend />
            <Line
              type="monotone"
              dataKey="percentage"
              stroke="#007bff"
              activeDot={{ r: 8 }}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </Container>
  );
}
