// src/components/layout/Navbar.jsx
import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Navbar = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem("token");
        navigate("/login");
    };

    return (
        <nav className="bg-white border-b border-gray-200 shadow-sm dark:bg-gray-800">
            <div className="max-w-screen-xl mx-auto px-4 py-3 flex justify-between items-center">
                <div className="text-lg font-semibold text-gray-800 dark:text-white">
                    My App
                </div>
                <ul className="flex space-x-6 text-sm font-medium text-gray-700 dark:text-gray-200">
                    <li>
                        <Link to="/" className="hover:text-blue-600">User Management</Link>
                    </li>
                    <li>
                        <button
                            onClick={handleLogout}
                            className="text-red-600 hover:text-red-700"
                        >
                            Logout
                        </button>
                    </li>
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;
