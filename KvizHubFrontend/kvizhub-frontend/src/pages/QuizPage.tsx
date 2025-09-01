import React from 'react';
import { useParams } from 'react-router-dom';

const QuizPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    return (
        <div>
            <h1>Quiz Page - ID: {id}</h1>
        </div>
    );
};

export default QuizPage;
