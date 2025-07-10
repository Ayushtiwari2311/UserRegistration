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
    const [totalRecords, setTotalRecords] = useState(0);
    const [params, setParams] = useState({
        start: 0,
        length: 1,
        searchValue: '',
        sortColumn: 'CreatedOn',
        sortDirection: 'desc',
    });

    const loadUsers = async () => {
        try {
            const res = await userService.getAllUsers(params);

            setUsers(res.data); // ✅ user records
            setTotalRecords(res.recordsFiltered); // ✅ total for pagination
        } catch (err) {
            console.error("Failed to load users", err);
        }
    };

    const handleSearch = (e) => {
        const searchValue = e.target.value;
        setParams(prev => ({
            ...prev,
            searchValue,
            start: 0, // reset to first page
        }));
    };

    const handleSort = (column) => {
        setParams(prev => ({
            ...prev,
            sortColumn: column,
            sortDirection: prev.sortDirection === 'asc' ? 'desc' : 'asc'
        }));
    };

    const handlePageChange = (pageIndex) => {
        setParams(prev => ({
            ...prev,
            start: (pageIndex - 1) * prev.length
        }));
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
        const updatedUser = response.data.data;
        if (response.data.isSuccess) {
            toast.success(response.data.message);
            setShowModal(false);
            setEditingUserId(null);
            setUsers((prevUsers) => {
                if (isEdit) {
                    return prevUsers.map(user =>
                        user.id === editingUserId ? updatedUser : user
                    );
                } else {
                    return [updatedUser, ...prevUsers];
                }
            });
        } else if (response.data?.message?.includes('<ul')) {
            toast.error(<div dangerouslySetInnerHTML={{ __html: response.data.message }} />);
        } else {
            toast.error(response.data.message || "Something went wrong");
        }
    };

    useEffect(() => {
        loadUsers();
    }, [params]);

    return (
        <div className="flex flex-col px-6 py-8 bg-gray-100 min-h-screen">
            <h1 className="text-2xl font-semibold">User Management</h1>
            <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-6 gap-4">
                <div className="flex items-center gap-2">
                    <select
                        value={params.length}
                        onChange={(e) =>
                            setParams((prev) => ({
                                ...prev,
                                length: parseInt(e.target.value),
                                start: 0, // reset to first page
                            }))
                        }
                        className="p-2 border rounded text-sm"
                    >
                        <option value={1}>1</option>
                        <option value={2}>2</option>
                        <option value={5}>5</option>
                        <option value={10}>10</option>
                        <option value={20}>20</option>
                        <option value={50}>50</option>
                    </select>
                    <input
                        type="text"
                        placeholder="Search..."
                        value={params.searchValue}
                        onChange={handleSearch}
                        className="p-2 border rounded text-sm"
                    />
                </div>

                <button
                    onClick={() => {
                        setEditingUserId(null);
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
                onSort ={handleSort}
                onEdit={(user) => {
                    setEditingUserId(user.id);
                    setShowModal(true);
                }}
            />

            {/* Pagination Controls */}
            {totalRecords > params.length && (
                <div className="flex items-center justify-center mt-4 gap-2">
                    <button
                        onClick={() => handlePageChange(params.start / params.length)}
                        disabled={params.start === 0}
                        className="px-3 py-1 text-sm border rounded-md bg-gray-200 hover:bg-gray-300 disabled:opacity-50"
                    >
                        Prev
                    </button>
                    <span className="text-sm text-gray-700">
                        Page {params.start / params.length + 1} of {Math.ceil(totalRecords / params.length)}
                    </span>
                    <button
                        onClick={() => handlePageChange(params.start / params.length + 2)}
                        disabled={params.start + params.length >= totalRecords}
                        className="px-3 py-1 text-sm border rounded-md bg-gray-200 hover:bg-gray-300 disabled:opacity-50"
                    >
                        Next
                    </button>
                </div>
            )}


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
