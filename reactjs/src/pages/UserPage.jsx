import React, { useEffect, useState } from 'react';
import { userService } from '../services/userService';
import UserForm from '../components/userregistration/UserForm';
import UserTable from '../components/userregistration/UserTable'; // 👈 Import it here

const UserPage = () => {
    const [users, setUsers] = useState([]);

    const loadUsers = () => {
        userService.getAllUsers({ start: 0, length: 10 }).then(setUsers);
    };

    const handleFormSubmit = async (formData) => {
        const response = await userService.createUser(formData);
        if (response.data.isSuccess) {
            alert("User created!");
            loadUsers();
        } else {
            alert("Error: " + response.data.message);
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    return (
        <>
            <h1>User Management</h1>
            <UserForm onSubmit={handleFormSubmit} />
            <UserTable users={users} />
        </>
    );
};

export default UserPage;
