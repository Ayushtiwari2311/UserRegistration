import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginForm from './components/login/LoginForm';
import UserPage from './pages/UserPage';
import { LoadingProvider, useLoading } from './loading/LoadingContext';
import SidebarLayout from './layout/SidebarLayout'; // ✅ import navbar
import { store } from './loading/store';
import Loader from './components/common/Loader';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const isAuthenticated = () => !!localStorage.getItem("token");

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
            <ToastContainer />
            {loading && <Loader />}
            <Routes>
                <Route
                    path="/"
                    element={
                        <PrivateRoute>
                            <UserPage />
                        </PrivateRoute>
                    }
                />
                <Route path="/login" element={<LoginForm />} />
                {/* Protected Layout */}
                <Route
                    path="/"
                    element={
                        isAuthenticated() ? <SidebarLayout /> : <Navigate to="/login" />
                    }
                >
                    <Route index element={<UserPage />} />
                </Route>

                <Route path="*" element={<Navigate to="/" />} />
                <Route path="*" element={<Navigate to={isAuthenticated() ? "/" : "/login"} />} />
            </Routes>
        </BrowserRouter>
    );
};

export default () => (
    <LoadingProvider>
        <App />
    </LoadingProvider>
);
