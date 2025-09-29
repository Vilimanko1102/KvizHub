import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuizService, { QuizWithQuestionsDto, QuestionDtoWithAnswers } from "../services/QuizService";
import QuizAttemptService, { QuizAttemptSubmitDto, UserAnswerSubmitDto } from "../services/QuizAttemptService";
import { Button, Card, Container, Form, Alert } from "react-bootstrap";

export default function QuizPlayPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [quiz, setQuiz] = useState<QuizWithQuestionsDto | null>(null);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [selectedAnswers, setSelectedAnswers] = useState<Record<number, number[] | string>>({});
  const [timeLeft, setTimeLeft] = useState<number>(0);
  const timerRef = useRef<NodeJS.Timeout | null>(null);

  const userId = Number(localStorage.getItem("userId")); // može i iz auth konteksta

  // Fetch quiz with questions
  useEffect(() => {
    const fetchQuiz = async () => {
      if (!id) return;
      const data: QuizWithQuestionsDto = await QuizService.getQuizByIdWithQuestions(parseInt(id));
      setQuiz(data);
      setTimeLeft(data.timeLimit * 60);
    };
    fetchQuiz();
  }, [id]);

  // Timer
  useEffect(() => {
    if (!quiz) return;
    if (timeLeft <= 0) {
      finishQuiz();
      return;
    }

    timerRef.current = setInterval(() => setTimeLeft(prev => prev - 1), 1000);
    return () => {
      if (timerRef.current) clearInterval(timerRef.current);
    };
  }, [timeLeft, quiz]);

  if (!quiz) return <div>Loading...</div>;

  const question: QuestionDtoWithAnswers = quiz.questions[currentQuestionIndex];

  const handleSelectAnswer = (answerId: number, isMultiple: boolean, textAnswer?: string) => {
    setSelectedAnswers(prev => {
      const existing = prev[question.id] || [];

      switch (question.type) {
        case "MultipleChoice":
          const updated = Array.isArray(existing) ? [...existing] : [];
          if (updated.includes(answerId)) updated.splice(updated.indexOf(answerId), 1);
          else updated.push(answerId);
          return { ...prev, [question.id]: updated };
        case "SingleChoice":
        case "TrueFalse":
          return { ...prev, [question.id]: [answerId] };
        case "FillIn":
          return { ...prev, [question.id]: textAnswer || "" };
        default:
          return prev;
      }
    });
  };

  const handleNext = () => {
    if (currentQuestionIndex < quiz.questions.length - 1) setCurrentQuestionIndex(prev => prev + 1);
  };

  const handlePrevious = () => {
    if (currentQuestionIndex > 0) setCurrentQuestionIndex(prev => prev - 1);
  };

  const finishQuiz = async () => {
  if (timerRef.current) clearInterval(timerRef.current);
  if (!quiz) return;

  const userAnswers: UserAnswerSubmitDto[] = quiz.questions.map(q => ({
    questionId: q.id,
    selectedAnswerIds: Array.isArray(selectedAnswers[q.id]) ? (selectedAnswers[q.id] as number[]) : undefined,
    textAnswer: typeof selectedAnswers[q.id] === "string" ? (selectedAnswers[q.id] as string) : undefined
  }));

  // Izračunaj timeSpent
  const timeSpent = quiz.timeLimit * 60 - timeLeft; // timeLimit je u minutima

  const dto: QuizAttemptSubmitDto = {
    quizId: quiz.id,
    userId,
    userAnswers,
    timeSpent // <--- dodaj ovo
  };

  const subData = await QuizAttemptService.submitQuizAnswers(dto);
  console.log(subData)
  navigate(`/quiz/${subData.id}/result`);
  };


  const formatTime = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s < 10 ? "0" : ""}${s}`;
  };

  return (
    <Container className="mt-4">
      <h2>{quiz.title}</h2>
      <Alert variant="info">Time left: {formatTime(timeLeft)}</Alert>

      <Card className="mb-3">
        <Card.Body>
          <Card.Title>
            Question {currentQuestionIndex + 1} of {quiz.questions.length}
          </Card.Title>
          <Card.Text>{question.text}</Card.Text>

          <Form>
            {question.type === "FillIn" ? (
              <Form.Control
                type="text"
                value={(selectedAnswers[question.id] as string) || ""}
                onChange={e => handleSelectAnswer(0, false, e.target.value)}
              />
            ) : (
              question.answers.map(answer => (
                <Form.Check
                  key={answer.id}
                  type={question.type === "MultipleChoice" ? "checkbox" : "radio"}
                  label={answer.text}
                  name={`question-${question.id}`}
                  checked={
                    Array.isArray(selectedAnswers[question.id]) &&
                    (selectedAnswers[question.id] as number[]).includes(answer.id)
                  }
                  onChange={() => handleSelectAnswer(answer.id, question.type === "MultipleChoice")}
                />
              ))
            )}
          </Form>
        </Card.Body>
      </Card>

      <div className="d-flex justify-content-between">
        <Button onClick={handlePrevious} disabled={currentQuestionIndex === 0}>
          Previous
        </Button>
        {currentQuestionIndex < quiz.questions.length - 1 ? (
          <Button onClick={handleNext}>Next</Button>
        ) : (
          <Button variant="success" onClick={finishQuiz}>
            Finish Quiz
          </Button>
        )}
      </div>
    </Container>
  );
}
