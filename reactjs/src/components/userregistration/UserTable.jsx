import React, { useState } from 'react';
import AuthorizedImage from '../common/AuthorizedImage';
import { TrashIcon, PencilSquareIcon } from '@heroicons/react/24/solid';



const UserTable = ({ users, onDelete, onEdit, onSort }) => {
    const [showModal, setShowModal] = useState(false);
    const [selectedPhoto, setSelectedPhoto] = useState(null);

    const handleImageClick = (photoPath) => {
        setSelectedPhoto(photoPath);
        setShowModal(true);
    };

    return (
        <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th onClick={() => onSort('name')} className="cursor-pointer px-6 py-3 hover:underline">Name</th>
                        <th onClick={() => onSort('email')} className="cursor-pointer px-6 py-3 hover:underline">Email</th>
                        <th onClick={() => onSort('mobile')} className="cursor-pointer px-6 py-3 hover:underline">Mobile</th>
                        <th onClick={() => onSort('gender')} className="cursor-pointer px-6 py-3 hover:underline">Gender</th>
                        <th onClick={() => onSort('dob')} className="cursor-pointer px-6 py-3 hover:underline">DOB</th>
                        <th onClick={() => onSort('state')} className="cursor-pointer px-6 py-3 hover:underline">State</th>
                        <th onClick={() => onSort('city')} className="cursor-pointer px-6 py-3 hover:underline">City</th>
                        <th className="px-6 py-3">Hobbies</th>
                        <th className="px-6 py-3">Photo</th>
                        <th className="px-6 py-3">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.length > 0 ? (
                        users.map((user) => (
                            <tr key={user.email} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                                <td className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white">{user.name}</td>
                                <td className="px-6 py-4">{user.email}</td>
                                <td className="px-6 py-4">{user.mobile}</td>
                                <td className="px-6 py-4">{user.gender}</td>
                                <td className="px-6 py-4">{user.dateOfBirth?.split('T')[0]}</td>
                                <td className="px-6 py-4">{user.state}</td>
                                <td className="px-6 py-4">{user.city}</td>
                                <td className="px-6 py-4">
                                    {Array.isArray(user.hobbies) ? user.hobbies.join(', ') : user.hobbies}
                                </td>
                                <td className="px-6 py-4">
                                    {/*<AuthorizedImage*/}
                                    {/*    src={user.photoPath}*/}
                                    {/*    alt="User"*/}
                                    {/*    onClick={() => handleImageClick(user.photoPath)}*/}
                                    {/*    className="w-10 h-10 object-cover rounded-full border cursor-pointer hover:scale-110 transition-transform duration-200"*/}
                                    {/*/>*/}
                                    <img
                                        src={user.photoPath}
                                        alt="User"
                                        onClick={() => handleImageClick(user.photoPath)}
                                        className="w-10 h-10 object-cover rounded-full border cursor-pointer hover:scale-110 transition-transform duration-200"
                                    />
                                </td>
                                <td className="px-6 py-4 flex gap-2 items-center">
                                    <button
                                        onClick={() => onDelete(user.id)}
                                        className="text-red-600 hover:text-red-800 transition duration-200"
                                        title="Delete"
                                    >
                                        <TrashIcon className="w-6 h-6" />
                                    </button>
                                    <button
                                        onClick={() => onEdit(user)}
                                        className="text-blue-600 hover:text-blue-800 transition duration-200"
                                    >
                                        <PencilSquareIcon className="w-6 h-6" />
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr className="bg-white dark:bg-gray-800">
                            <td colSpan="9" className="px-6 py-4 text-center text-gray-500 dark:text-gray-400">
                                No users found.
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

            {/* ✅ Modal Popup */}
            {showModal && (
                <div
                    className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-none"
                    onClick={() => setShowModal(false)}
                >
                    <div
                        className="bg-white p-4 rounded-lg shadow-lg relative"
                        onClick={(e) => e.stopPropagation()} // prevents closing when clicking inside
                    >
                        <button
                            className="absolute top-2 right-2 text-gray-600 hover:text-red-500 text-xl"
                            onClick={() => setShowModal(false)}
                        >
                            ×
                        </button>
                        <img
                            src={selectedPhoto}
                            alt="Full Size"
                            className="max-w-[80vw] max-h-[80vh] rounded-md border border-gray-300"
                        />
                    </div>
                </div>
            )}
        </div>
    );
};

export default UserTable;
