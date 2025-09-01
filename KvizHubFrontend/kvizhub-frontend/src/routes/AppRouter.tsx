import React, { JSX } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from '../pages/LoginPage';
import RegisterPage from '../pages/RegisterPage';
import QuizListPage from '../pages/QuizListPage';
import QuizPage from '../pages/QuizPage';
import UserResultsPage from '../pages/UserResultsPage';
import AdminDashboardPage from '../pages/AdminDashboardPage';
import { useAuth } from '../context/AuthContext';

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
    const { user } = useAuth();
    return user ? children : <Navigate to="/login" />;
};

const AdminRoute = ({ children }: { children: JSX.Element }) => {
    const { user } = useAuth();
    return user?.isAdmin ? children : <Navigate to="/login" />;
};

const AppRouter = () => {
    return (
        <Router>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                
                <Route
                    path="/quizzes"
                    element={<PrivateRoute><QuizListPage /></PrivateRoute>}
                />
                <Route
                    path="/quiz/:id"
                    element={<PrivateRoute><QuizPage /></PrivateRoute>}
                />
                <Route
                    path="/results"
                    element={<PrivateRoute><UserResultsPage /></PrivateRoute>}
                />
                <Route
                    path="/admin/*"
                    element={<AdminRoute><AdminDashboardPage /></AdminRoute>}
                />
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
};

export default Router;
