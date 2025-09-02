import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const LandingPage = () => {
    const { login, register } = useAuth();
    const [isLogin, setIsLogin] = useState(true);
    const navigate = useNavigate();


    const [loginData, setLoginData] = useState({ usernameOrEmail: '', password: '' });
    const [registerData, setRegisterData] = useState({ username: '', email: '', password: '', avatarUrl: '' });
    const [error, setError] = useState<string | null>(null);

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await login(loginData.usernameOrEmail, loginData.password);
            navigate("/quizzes");
        } catch (err: any) {
            setError(err.message || 'Login failed');
        }
    };

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await register(registerData);
            navigate("/quizzes");
        } catch (err: any) {
            setError(err.message || 'Registration failed');
        }
    };

    return (
        <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <div className="card shadow">
                        <div className="card-body">
                            <h2 className="card-title text-center mb-4">{isLogin ? 'Login' : 'Register'}</h2>

                            {error && <div className="alert alert-danger">{error}</div>}

                            {isLogin ? (
                                <form onSubmit={handleLogin}>
                                    <div className="mb-3">
                                        <label className="form-label">Username or Email</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={loginData.usernameOrEmail}
                                            onChange={e => setLoginData({ ...loginData, usernameOrEmail: e.target.value })}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label className="form-label">Password</label>
                                        <input
                                            type="password"
                                            className="form-control"
                                            value={loginData.password}
                                            onChange={e => setLoginData({ ...loginData, password: e.target.value })}
                                        />
                                    </div>
                                    <button type="submit" className="btn btn-primary w-100">Login</button>
                                    <div className="text-center mt-3">
                                        <button
                                            type="button"
                                            className="btn btn-link"
                                            onClick={() => { setIsLogin(false); setError(null); }}
                                        >
                                            Don't have an account? Register
                                        </button>
                                    </div>
                                </form>
                            ) : (
                                <form onSubmit={handleRegister}>
                                    <div className="mb-3">
                                        <label className="form-label">Username</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={registerData.username}
                                            onChange={e => setRegisterData({ ...registerData, username: e.target.value })}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label className="form-label">Email</label>
                                        <input
                                            type="email"
                                            className="form-control"
                                            value={registerData.email}
                                            onChange={e => setRegisterData({ ...registerData, email: e.target.value })}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label className="form-label">Password</label>
                                        <input
                                            type="password"
                                            className="form-control"
                                            value={registerData.password}
                                            onChange={e => setRegisterData({ ...registerData, password: e.target.value })}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label className="form-label">Avatar URL (optional)</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={registerData.avatarUrl}
                                            onChange={e => setRegisterData({ ...registerData, avatarUrl: e.target.value })}
                                        />
                                    </div>
                                    <button type="submit" className="btn btn-success w-100">Register</button>
                                    <div className="text-center mt-3">
                                        <button
                                            type="button"
                                            className="btn btn-link"
                                            onClick={() => { setIsLogin(true); setError(null); }}
                                        >
                                            Already have an account? Login
                                        </button>
                                    </div>
                                </form>
                            )}

                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LandingPage;
