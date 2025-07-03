import React, { useEffect, useState, useRef } from 'react';
import { userService } from "../../services/userService";
import { XMarkIcon } from '@heroicons/react/24/solid'; // Heroicons v2 (Tailwind-compatible)



const UserForm = ({ onSubmit }) => {
    const fileInputRef = useRef();
    const [states, setStates] = useState([]);
    const [cities, setCities] = useState([]);
    const [hobbies, setHobbies] = useState([]);
    const [genders, setGenders] = useState([]);
    const [formData, setFormData] = useState({});

    useEffect(() => {
        userService.fetchStates().then(setStates);
        userService.fetchHobbies().then(setHobbies);
        userService.fetchGenders().then(setGenders);
    }, []);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        if (type === 'checkbox') {
            setFormData((prev) => {
                const selectedHobbies = prev.hobbies || [];
                return {
                    ...prev,
                    hobbies: checked
                        ? [...selectedHobbies, value]
                        : selectedHobbies.filter((h) => h !== value),
                };
            });
        } else {
            setFormData((prev) => ({ ...prev, [name]: value }));
        }

        if (name === 'state') {
            userService.fetchCities(value).then(setCities);
        }
    };

    const submit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <form onSubmit={submit} className="space-y-5 max-w-2xl mx-auto">
            {/* Name */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Name</label>
                <input
                    name="name"
                    type="text"
                    placeholder="Enter name"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                    required
                />
            </div>

            {/* Email */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Email</label>
                <input
                    name="email"
                    type="email"
                    placeholder="Enter email"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                    required
                />
            </div>

            {/* Gender */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Gender</label>
                <select
                    name="gender"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                >
                    <option value="">Select gender</option>
                    {genders.map((g) => (
                        <option key={g.id} value={g.id}>{g.name}</option>
                    ))}
                </select>
            </div>

            {/* Mobile */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Mobile</label>
                <input
                    name="mobile"
                    type="text"
                    placeholder="Enter mobile number"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                />
            </div>

            {/* ContactNo */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">ContactNo</label>
                <input
                    name="contactNo"
                    type="text"
                    placeholder="Enter Contact number"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                />
            </div>

            {/* DOB */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Date of Birth</label>
                <input
                    name="dateOfBirth"
                    type="date"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                />
            </div>

            {/* State */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">State</label>
                <select
                    name="state"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                >
                    <option value="">Select state</option>
                    {states.map((s) => (
                        <option key={s.id} value={s.id}>{s.name}</option>
                    ))}
                </select>
            </div>

            {/* City */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">City</label>
                <select
                    name="city"
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm focus:ring-blue-500 focus:border-blue-500"
                    onChange={handleChange}
                >
                    <option value="">Select city</option>
                    {cities.map((c) => (
                        <option key={c.id} value={c.id}>{c.name}</option>
                    ))}
                </select>
            </div>

            {/* Hobbies */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Hobbies</label>
                <div className="grid grid-cols-2 gap-2">
                    {hobbies.map((h) => (
                        <label key={h.id} className="flex items-center gap-2 text-sm text-gray-700">
                            <input
                                type="checkbox"
                                value={h.id}
                                name="hobbies"
                                onChange={handleChange}
                                className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                            />
                            {h.name}
                        </label>
                    ))}
                </div>
            </div>

            <div className="mb-5">
                <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
                    Upload Photo
                </label>

                {/* Custom upload button */}
                <div className="flex items-center gap-3">
                    <button
                        type="button"
                        onClick={() => fileInputRef.current?.click()}
                        className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 text-sm"
                    >
                        Choose File
                    </button>
                    {/* Filename display */}
                    <span className="text-sm text-gray-600 dark:text-gray-300">
                        {formData.photo ? formData.photo.name : "No file selected"}
                    </span>
                </div>

                {/* Hidden actual input */}
                <input
                    ref={fileInputRef}
                    id="file_input"
                    type="file"
                    name="photo"
                    accept="image/*"
                    onChange={(e) => {
                        const file = e.target.files[0];
                        if (file) {
                            if (file.size > 2 * 1024 * 1024) {
                                alert("File must be under 2MB");
                                return;
                            }
                            setFormData((prev) => ({
                                ...prev,
                                photo: file,
                                previewUrl: URL.createObjectURL(file),
                            }));
                        }
                    }}
                    className="hidden"
                />

                <p className="mt-1 text-sm text-gray-500 dark:text-gray-300">
                    JPG, PNG, SVG, or GIF (max 2MB).
                </p>

                {/* Image Preview */}
                {formData.previewUrl && (
                    <div className="relative mt-4 inline-block">
                        <img
                            src={formData.previewUrl}
                            alt="Preview"
                            className="h-32 w-32 object-cover rounded border border-gray-300"
                        />
                        <button
                            type="button"
                            onClick={() => {
                                setFormData((prev) => ({
                                    ...prev,
                                    photo: null,
                                    previewUrl: null,
                                }));
                                if (fileInputRef.current) fileInputRef.current.value = '';
                            }}
                            className="absolute -top-2 -right-2 bg-red-600 hover:bg-red-700 text-white p-1 rounded-full shadow-md"
                            title="Remove image"
                        >
                            <XMarkIcon className="h-4 w-4" />
                        </button>
                    </div>
                )}
            </div>


            {/* Submit */}
            <div className="text-right">
                <button
                    type="submit"
                    className="text-white bg-blue-700 hover:bg-blue-800 font-medium rounded-lg text-sm px-5 py-2.5 focus:ring-4 focus:outline-none focus:ring-blue-300"
                >
                    Submit
                </button>
            </div>
        </form>
    );
};

export default UserForm;
