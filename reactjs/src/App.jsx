import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginForm from './components/login/LoginForm';
import UserPage from './pages/UserPage';
import { LoadingProvider, useLoading } from './loading/LoadingContext';
import { store } from './loading/store';
import Loader from './components/common/Loader';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


// Helper to check if user is logged in
const isAuthenticated = () => {
    return !!localStorage.getItem('token');
};

// Protect user routes
const PrivateRoute = ({ children }) => {
    return isAuthenticated() ? children : <Navigate to="/login" />;
};

const App = () => {
    const { loading, setLoading } = useLoading();
    useEffect(() => {
        store.setLoading = setLoading;
    }, [setLoading]);
    return (
        <BrowserRouter>
            <ToastContainer position="top-right" autoClose={3000} hideProgressBar />
            {loading && <Loader />}
            <Routes>
                <Route path="/" element={
                    <PrivateRoute>
                        <UserPage />
                    </PrivateRoute>
                } />

                <Route path="/login" element={<LoginForm />} />

                <Route path="*" element={<Navigate to={isAuthenticated() ? "/" : "/login"} />} />
            </Routes>
        </BrowserRouter>
    );
};

const AppWrapper = () => (
    <React.StrictMode>
        <LoadingProvider>
            <App />
        </LoadingProvider>
    </React.StrictMode>
);

export default AppWrapper;
