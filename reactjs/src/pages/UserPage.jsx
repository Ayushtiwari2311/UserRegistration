import React, { useEffect, useState } from 'react';
import { userService } from '../services/userService';
import UserForm from '../components/userregistration/UserForm';
import UserTable from '../components/userregistration/UserTable';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const UserPage = () => {
    const [users, setUsers] = useState([]);
    const [showModal, setShowModal] = useState(false);

    const loadUsers = () => {
        userService.getAllUsers({ start: 0, length: 10 }).then((res) => {
            setUsers(res.data);
        });
    };

    const handleFormSubmit = async (formData) => {
        const response = await userService.createUser(formData);
        if (response.data.isSuccess) {
            toast.success(response.data.message);
            setShowModal(false); // ✅ close modal
            loadUsers();
        } else if (response.data?.message?.includes('<ul')) {
            // Multiple HTML errors — render as raw
            toast.error(<div dangerouslySetInnerHTML={{ __html: response.data.message }} />);
        } else {
            toast.error(response.data.message || "Login failed");
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    return (
        <div className="flex flex-col px-6 py-8 bg-gray-100 min-h-screen">
            {/* Header row */}
            <div className="flex items-center justify-between mb-6">
                <h1 className="text-2xl font-semibold">User Management</h1>
                <button
                    onClick={() => setShowModal(true)}
                    className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md shadow"
                >
                    + Add User
                </button>
            </div>

            {/* Table */}
            <UserTable users={users} />

            {/* ✅ Modal for Add User Form */}
            {showModal && (
                <div
                    className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-none"
                    onClick={() => setShowModal(false)}
                >
                    <div
                        className="bg-white w-full max-w-2xl max-h-[90vh] p-6 rounded-lg relative shadow-lg overflow-y-auto"
                        onClick={(e) => e.stopPropagation()}
                    >
                        {/* Close button */}
                        <button
                            onClick={() => setShowModal(false)}
                            className="absolute top-2 right-3 text-xl text-gray-500 hover:text-red-500"
                        >
                            ×
                        </button>

                        <h2 className="text-xl font-semibold mb-4">Add New User</h2>

                        {/* 🚀 Your Tailwind-styled form */}
                        <UserForm onSubmit={handleFormSubmit} />
                    </div>
                </div>
            )}
        </div>
    );
};

export default UserPage;
