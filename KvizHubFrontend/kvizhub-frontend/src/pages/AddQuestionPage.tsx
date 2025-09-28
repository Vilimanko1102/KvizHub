import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import QuestionService, { QuestionCreateDto } from "../services/QuestionService";

const AddQuestionPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [text, setText] = useState("");
    const [type, setType] = useState<"SingleChoice" | "MultipleChoice" | "TrueFalse" | "FillIn">("SingleChoice");
    const [points, setPoints] = useState(1);
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");

        const dto: QuestionCreateDto = { quizId: Number(id), text, type, points };

        try {
            await QuestionService.createQuestion(dto);
            navigate(`/quiz/${id}`);
        } catch (err: any) {
            setError(err.response?.data || "Error creating question");
        }
    };

    return (
        <div className="container mt-4">
            <h2>Add Question</h2>
            {error && <div className="alert alert-danger">{error}</div>}
            <form onSubmit={handleSubmit}>
                <div className="mb-3">
                    <label className="form-label">Question Text</label>
                    <input
                        type="text"
                        className="form-control"
                        value={text}
                        onChange={(e) => setText(e.target.value)}
                        required
                    />
                </div>

                <div className="mb-3">
                    <label className="form-label">Type</label>
                    <select
                        className="form-select"
                        value={type}
                        onChange={(e) => setType(e.target.value as any)}
                    >
                        <option value="SingleChoice">Single Choice</option>
                        <option value="MultipleChoice">Multiple Choice</option>
                        <option value="TrueFalse">True/False</option>
                        <option value="FillIn">Fill In</option>
                    </select>
                </div>

                <div className="mb-3">
                    <label className="form-label">Points</label>
                    <input
                        type="number"
                        className="form-control"
                        value={points}
                        onChange={(e) => setPoints(Number(e.target.value))}
                        min={1}
                        required
                    />
                </div>

                <button type="submit" className="btn btn-success">
                    Create Question
                </button>
            </form>
        </div>
    );
};

export default AddQuestionPage;
