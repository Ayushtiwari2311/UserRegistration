import React, { useEffect, useState } from 'react';
import { userService } from '../services/userService';
import UserForm from '../components/userregistration/UserForm';
import UserTable from '../components/userregistration/UserTable';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Swal from 'sweetalert2';

const UserPage = () => {
    const [users, setUsers] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [editingUserId, setEditingUserId] = useState(null); // ✅ for edit mode

    const loadUsers = () => {
        userService.getAllUsers({ start: 0, length: 10 }).then((res) => {
            setUsers(res.data);
        });
    };

    const handleDelete = async (id) => {
        const result = await Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!',
        });

        if (result.isConfirmed) {
            try {
                const response = await userService.deleteUser(id);
                if (response.data.isSuccess) {
                    toast.success(response.data.message);
                    setUsers(prevUsers => prevUsers.filter(user => user.id !== id));
                } else {
                    toast.error(response.data.message || "Unexpected Error!");
                }
            } catch (error) {
                toast.error("Something Went Wrong!");
            }
        }
    };

    const handleFormSubmit = async (formData) => {
        const isEdit = !!editingUserId;
        const apiCall = isEdit
            ? userService.updateUser(editingUserId, formData)
            : userService.createUser(formData);

        const response = await apiCall;

        if (response.data.isSuccess) {
            toast.success(response.data.message);
            setShowModal(false);
            setEditingUserId(null);
            loadUsers();
        } else if (response.data?.message?.includes('<ul')) {
            toast.error(<div dangerouslySetInnerHTML={{ __html: response.data.message }} />);
        } else {
            toast.error(response.data.message || "Something went wrong");
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    return (
        <div className="flex flex-col px-6 py-8 bg-gray-100 min-h-screen">
            {/* Header */}
            <div className="flex items-center justify-between mb-6">
                <h1 className="text-2xl font-semibold">User Management</h1>
                <button
                    onClick={() => {
                        setEditingUserId(null); // reset form
                        setShowModal(true);
                    }}
                    className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md shadow"
                >
                    + Add User
                </button>
            </div>

            {/* Table */}
            <UserTable
                users={users}
                onDelete={handleDelete}
                onEdit={(user) => {
                    setEditingUserId(user.id);
                    setShowModal(true);
                }}
            />

            {/* Modal */}
            {showModal && (
                <div
                    className="fixed inset-0 z-50 flex items-center justify-center bg-black/40"
                    onClick={() => setShowModal(false)}
                >
                    <div
                        className="bg-white w-full max-w-2xl max-h-[90vh] p-6 rounded-lg relative shadow-lg overflow-y-auto"
                        onClick={(e) => e.stopPropagation()}
                    >
                        <button
                            onClick={() => setShowModal(false)}
                            className="absolute top-2 right-3 text-xl text-gray-500 hover:text-red-500"
                        >
                            ×
                        </button>

                        <h2 className="text-xl font-semibold mb-4">
                            {editingUserId ? 'Edit User' : 'Add New User'}
                        </h2>

                        <UserForm
                            onSubmit={handleFormSubmit}
                            userId={editingUserId} // ✅ prefill if editing
                            onClose={() => {
                                setShowModal(false);
                                setEditingUserId(null);
                            }}
                        />
                    </div>
                </div>
            )}
        </div>
    );
};

export default UserPage;
