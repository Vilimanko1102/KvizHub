import React from "react";
import { Card, Button, Alert } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

interface QuizCardProps {
    id: number;
    title: string;
    description: string;
    questionCount: number;
    difficulty: "Easy" | "Medium" | "Hard";
    timeLimit: number;
    isPlayable: boolean; // 🔑 sada dobijamo sa backa
}

const difficultyColors = {
    Easy: "success",
    Medium: "warning",
    Hard: "danger",
};

const QuizCard: React.FC<QuizCardProps> = ({
    id,
    title,
    description,
    questionCount,
    difficulty,
    timeLimit,
    isPlayable,
}) => {
    const navigate = useNavigate();
    const isAdmin = localStorage.getItem("userRole") === "Admin";

    const handleStart = () => {
        navigate(`/quiz/${id}`);
    };

    const handleAddQuestion = () => {
        navigate(`/admin/quiz/${id}/add-question`);
    };

    const handleViewQuestions = () => {
        navigate(`/quiz/${id}`);
    };

    return (
        <Card border={difficultyColors[difficulty]} className="h-100 shadow-sm">
            <Card.Body>
                <Card.Title>{title}</Card.Title>
                <Card.Text>{description}</Card.Text>
                <Card.Text>
                    <strong>Questions:</strong> {questionCount} <br />
                    <strong>Difficulty:</strong>{" "}
                    <span className={`text-${difficultyColors[difficulty]}`}>
                        {difficulty}
                    </span>
                    <br />
                    <strong>Time:</strong> {timeLimit} min
                </Card.Text>

                {isAdmin ? (
                    <>
                        <Button
                            variant="info"
                            className="me-2"
                            onClick={handleViewQuestions}
                        >
                            View Questions
                        </Button>
                        <Button variant="primary" onClick={handleAddQuestion}>
                            Add Question
                        </Button>
                    </>
                ) : isPlayable ? (
                    <Button
                        variant={difficultyColors[difficulty]}
                        onClick={handleStart}
                    >
                        Start Quiz
                    </Button>
                ) : (
                    <Alert variant="secondary" className="mb-0">
                        This quiz is not ready yet.
                    </Alert>
                )}
            </Card.Body>
        </Card>
    );
};

export default QuizCard;
