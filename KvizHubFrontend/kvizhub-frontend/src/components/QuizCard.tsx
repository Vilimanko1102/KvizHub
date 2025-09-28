import React from "react";
import { Card, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

interface QuizCardProps {
    id: number;
    title: string;
    description: string;
    questionCount: number;
    difficulty: "Easy" | "Medium" | "Hard";
    timeLimit: number;
}

const difficultyColors = {
    Easy: "success",
    Medium: "warning",
    Hard: "danger",
};

const QuizCard: React.FC<QuizCardProps> = ({ id, title, description, questionCount, difficulty, timeLimit }) => {
    const navigate = useNavigate();

    const isAdmin = localStorage.getItem("userRole") === "Admin";

    const handleStart = () => {
        navigate(`/quiz/${id}`);
    };

    const handleAddQuestion = () => {
        navigate(`/admin/quiz/${id}/add-question`);
    };

    return (
        <Card border={difficultyColors[difficulty]} className="h-100">
            <Card.Body>
                <Card.Title>{title}</Card.Title>
                <Card.Text>{description}</Card.Text>
                <Card.Text>
                    Questions: {questionCount} <br />
                    Difficulty: <span className={`text-${difficultyColors[difficulty]}`}>{difficulty}</span> <br />
                    Time: {timeLimit} min
                </Card.Text>

                {isAdmin ? (
                    <Button variant="primary" onClick={handleAddQuestion}>
                        Add Question
                    </Button>
                ) : (
                    <Button variant={difficultyColors[difficulty]} onClick={handleStart}>
                        Start Quiz
                    </Button>
                )}
            </Card.Body>
        </Card>
    );
};

export default QuizCard;
