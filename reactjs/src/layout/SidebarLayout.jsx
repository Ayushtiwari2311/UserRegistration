// src/components/layout/SidebarLayout.jsx
import React from 'react';
import { userApi } from '../api/userApi';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import {
    UserCircleIcon,
    ArrowLeftOnRectangleIcon,
    UsersIcon,
} from '@heroicons/react/24/outline';

const SidebarLayout = () => {
    const navigate = useNavigate();

    const handleLogout = async () => {
        try {
            const res = await userApi.logout(); // ✅ awaits response
            if (res.data?.isSuccess) {
                navigate('/login'); // Redirect to login page
            } else {
                toast.error(res.data?.message || "Logout failed");
            }
        } catch (err) {
            if (err.response) {
                const message = err.response.data?.message || "Logout failed!";
                toast.error(message);
            } else {
                toast.error("Network error or server is unavailable.");
            }
        }
    };

    return (
        <div className="flex h-screen bg-gray-100 dark:bg-gray-900">
            {/* Sidebar */}
            <aside className="w-64 bg-white dark:bg-gray-800 shadow-md fixed h-full z-10">
                <div className="p-4 text-xl font-bold text-blue-600 dark:text-white border-b">
                    Admin Panel
                </div>
                <nav className="mt-4 px-4">
                    <ul className="space-y-2 text-sm text-gray-700 dark:text-gray-200">
                        <li>
                            <Link
                                to="/"
                                className="flex items-center gap-2 p-2 rounded hover:bg-blue-100 dark:hover:bg-gray-700"
                            >
                                <UsersIcon className="w-5 h-5" />
                                User Management
                            </Link>
                        </li>
                        <li>
                            <button
                                onClick={handleLogout}
                                className="w-full flex items-center gap-2 p-2 rounded hover:bg-red-100 dark:hover:bg-gray-700 text-left text-red-600 dark:text-red-400"
                            >
                                <ArrowLeftOnRectangleIcon className="w-5 h-5" />
                                Logout
                            </button>
                        </li>
                    </ul>
                </nav>
            </aside>

            {/* Main Content */}
            <div className="ml-64 flex-1 overflow-y-auto p-6">
                <Outlet />
            </div>
        </div>
    );
};

export default SidebarLayout;
