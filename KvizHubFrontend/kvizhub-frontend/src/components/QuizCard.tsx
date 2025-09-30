import React from "react";
import { Card, Button, Alert } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import QuizService from "../services/QuizService";

interface QuizCardProps {
    id: number;
    title: string;
    description: string;
    questionCount: number;
    category: string;
    difficulty: "Easy" | "Medium" | "Hard";
    timeLimit: number;
    isPlayable: boolean; 
    onDeleted?: () => void;
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
    category,
    timeLimit,
    isPlayable,
    onDeleted,
}) => {
    const navigate = useNavigate();
    const isAdmin = localStorage.getItem("userRole") === "Admin";

    const handleStart = () => navigate(`/quiz/${id}/play`);
    const handleAddQuestion = () => navigate(`/admin/quiz/${id}/add-question`);
    const handleViewQuestions = () => navigate(`/quiz/${id}`);
    const handleUpdateQuiz = () => navigate(`/admin/quiz/${id}/edit`);
    const handleViewAttempts = () => navigate(`/quiz/${id}/attempts`);

    const handleDeleteQuiz = async () => {
        if (window.confirm("Are you sure you want to delete this quiz?")) {
            try {
                await QuizService.deleteQuiz(id);
                alert("Quiz deleted successfully!");
                if (onDeleted) onDeleted();
            } catch (error) {
                console.error("Error deleting quiz:", error);
                alert("Failed to delete quiz.");
            }
        }
    };

    return (
        <Card border={difficultyColors[difficulty]} className="h-100 shadow-sm">
            <Card.Body>
                <Card.Title>{title}</Card.Title>
                <Card.Text>{description}</Card.Text>
                <Card.Text>
                    <strong>Questions:</strong> {questionCount} <br />
                    <strong>Category:</strong> {category} <br />
                    <strong>Difficulty:</strong>{" "}
                    <span className={`text-${difficultyColors[difficulty]}`}>
                        {difficulty}
                    </span>
                    <br />
                    <strong>Time:</strong> {timeLimit} min
                </Card.Text>

                {/* Leaderboard dugme (vidljivo svima) */}
                <Button
                    variant="secondary"
                    className="w-100 mb-3"
                    onClick={handleViewAttempts}
                >
                    View Leaderboard
                </Button>

                {isAdmin ? (
                    <>
                        <Button
                            variant="info"
                            className="w-100 mb-2"
                            onClick={handleViewQuestions}
                        >
                            View Questions
                        </Button>
                        <br />
                        <Button
                            variant="primary"
                            className="me-2 mb-2"
                            onClick={handleAddQuestion}
                        >
                            Add Question
                        </Button>
                        <Button
                            variant="warning"
                            className="me-2 mb-2"
                            onClick={handleUpdateQuiz}
                        >
                            Update Quiz
                        </Button>
                        <Button
                            variant="danger"
                            className="mb-2"
                            onClick={handleDeleteQuiz}
                        >
                            Delete Quiz
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
