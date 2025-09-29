import React, { JSX } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import RegisterPage from '../pages/RegisterPage';
import QuizListPage from '../pages/QuizListPage';
import QuizPage from '../pages/QuizPage';
import UserResultsPage from '../pages/UserResultsPage';
import AdminDashboardPage from '../pages/AdminDashboardPage';
import LandingPage from '../pages/LandingPage';
import { useAuth } from '../context/AuthContext';
import CreateQuizPage from '../pages/CreateQuizPage';
import AddQuestionPage from '../pages/AddQuestionPage';
import AddAnswerPage from '../pages/AddAnswerPage';
import QuizPlayPage from '../pages/QuizPlayPage';
import QuizResultPage from '../pages/QuizResultPage';
import Navbar from '../components/Navbar';
import EditQuizPage from '../pages/EditQuizPage';
import EditQuestionPage from '../pages/EditQuestionPage';

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
    const { user } = useAuth();
    return user ? children : <Navigate to="/login" />;
};

const AdminRoute = ({ children }: { children: JSX.Element }) => {
    const { user } = useAuth();
    console.log(user?.role)
    return user?.role === "Admin" ? children : <Navigate to="/login" />;
};

const AppRouter = () => {
    return (
        <Router>
            <Navbar/>
            <Routes>
                <Route path="/landing" element={<LandingPage/>}/>
                <Route path="/register" element={<RegisterPage />} />
                
                <Route
                    path="/quizzes"
                    element={<PrivateRoute><QuizListPage /></PrivateRoute>}
                />
                <Route
                    path="/quizzes/create"
                    element={<PrivateRoute><CreateQuizPage /></PrivateRoute>}
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
                <Route path="/quiz/:id/play" element={<QuizPlayPage />} />
                <Route path="/quiz/:attemptId/result" element={<QuizResultPage />} />
                <Route
                    path="/admin/quiz/:id/add-question"
                    element={
                        <AdminRoute>
                            <AddQuestionPage />
                        </AdminRoute>
                    }
                />
                <Route
                    path="/admin/question/:questionId/add-answer"
                    element={
                        <AdminRoute>
                            <AddAnswerPage />
                        </AdminRoute>
                    }
                />

                <Route
                    path="/admin/quiz/:id/edit"
                    element={
                        <AdminRoute>
                            <EditQuizPage />
                        </AdminRoute>
                    }
                />

                <Route
                    path="/admin/question/:questionId/edit"
                    element={
                        <AdminRoute>
                            <EditQuestionPage />
                        </AdminRoute>
                    }
                />

                <Route path="*" element={<Navigate to="/landing" />} />
            </Routes>
        </Router>
    );
};

export default AppRouter;
