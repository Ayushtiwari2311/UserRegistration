import React from 'react';

const UserTable = ({ users }) => {
    return (
        <table border="1" cellPadding="8">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Mobile</th>
                    <th>Gender</th>
                    <th>DOB</th>
                    <th>State</th>
                    <th>City</th>
                    <th>Hobbies</th>
                    <th>Photo</th>
                </tr>
            </thead>
            <tbody>
                {users.length > 0 ? (
                    users.map(user => (
                        <tr key={user.id}>
                            <td>{user.name}</td>
                            <td>{user.email}</td>
                            <td>{user.mobile}</td>
                            <td>{user.gender}</td>
                            <td>{user.dateOfBirth?.split('T')[0]}</td>
                            <td>{user.state}</td>
                            <td>{user.city}</td>
                            <td>{user.hobbies?.join(', ')}</td>
                            <td>
                                <img src={`https://localhost:7155${user.photoPath}`} alt="User" width="50" />
                            </td>
                        </tr>
                    ))
                ) : (
                    <tr>
                        <td colSpan="9" align="center">No users found.</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
};

export default UserTable;
