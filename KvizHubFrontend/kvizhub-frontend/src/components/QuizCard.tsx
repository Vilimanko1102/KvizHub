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

    const handleStart = () => {
        navigate(`/quiz/${id}`);
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
                <Button variant={difficultyColors[difficulty]} onClick={handleStart}>
                    Start Quiz
                </Button>
            </Card.Body>
        </Card>
    );
};

export default QuizCard;
