import React, { useEffect, useState, useRef } from 'react';
import { userService } from "../../services/userService";
import { XMarkIcon } from '@heroicons/react/24/solid';

const UserForm = ({ onSubmit, userId = null, onClose }) => {
    const fileInputRef = useRef();
    const [states, setStates] = useState([]);
    const [cities, setCities] = useState([]);
    const [hobbies, setHobbies] = useState([]);
    const [genders, setGenders] = useState([]);
    const [formData, setFormData] = useState({});

    // Load dropdowns
    useEffect(() => {
        userService.fetchStates().then(setStates);
        userService.fetchHobbies().then(setHobbies);
        userService.fetchGenders().then(setGenders);
    }, []);

    // Prefill form if userId is given (Edit mode)
    useEffect(() => {
        const loadUser = async () => {
            if (!userId) return;

            try {
                const [statesList, hobbiesList, gendersList] = await Promise.all([
                    userService.fetchStates(),
                    userService.fetchHobbies(),
                    userService.fetchGenders()
                ]);
                setStates(statesList);
                setHobbies(hobbiesList);
                setGenders(gendersList);

                const res = await userService.getUser(userId);
                const user = res.data.data;

                const selectedState = statesList.find(s => s.name === user.state);
                const stateId = selectedState?.id;

                let citiesRes = [];
                if (stateId) {
                    citiesRes = await userService.fetchCities(stateId);
                    setCities(citiesRes);
                }

                let previewUrl = user.photoPath;

                const hobbyIds = hobbiesList
                    .filter(h => user.hobbies.includes(h.name))
                    .map(h => String(h.id));

                const genderId = gendersList.find(g => g.name === user.gender)?.id;

                const cityId = citiesRes.find(c => c.name === user.city)?.id;

                setFormData({
                    ...user,
                    state: stateId,
                    city: cityId,
                    gender: genderId,
                    hobbies: hobbyIds,
                    previewUrl,
                });

            } catch (err) {
                console.error("Error fetching user:", err);
            }
        };

        loadUser();
    }, [userId]);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        if (type === 'checkbox') {
            setFormData((prev) => {
                const selectedHobbies = prev.hobbies || [];
                console.log(checked);
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
                    value={formData.name || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
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
                    value={formData.email || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
                    onChange={handleChange}
                    required
                />
            </div>

            {/* Gender */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Gender</label>
                <select
                    name="gender"
                    value={formData.gender || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
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
                    value={formData.mobile || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
                    onChange={handleChange}
                />
            </div>

            {/* ContactNo */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">ContactNo</label>
                <input
                    name="contactNo"
                    value={formData.contactNo || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
                    onChange={handleChange}
                />
            </div>

            {/* DOB */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">Date of Birth</label>
                <input
                    name="dateOfBirth"
                    type="date"
                    value={formData.dateOfBirth?.split('T')[0] || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
                    onChange={handleChange}
                />
            </div>

            {/* State */}
            <div>
                <label className="block mb-2 text-sm font-medium text-gray-900">State</label>
                <select
                    name="state"
                    value={formData.state || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
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
                    value={formData.city || ''}
                    className="w-full p-2.5 border border-gray-300 rounded-lg text-sm"
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
                                value={String(h.id)}
                                name="hobbies"
                                checked={formData.hobbies?.includes(String(h.id)) || false}
                                onChange={handleChange}
                                className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                            />
                            {h.name}
                        </label>
                    ))}
                </div>
            </div>

            {/* File Upload */}
            <div className="mb-5">
                <label className="block mb-2 text-sm font-medium text-gray-900">Photo</label>

                <div className="flex items-center gap-3">
                    <button
                        type="button"
                        onClick={() => fileInputRef.current?.click()}
                        className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 text-sm"
                    >
                        Choose File
                    </button>
                    <span className="text-sm text-gray-600">
                        {formData.photo?.name || "No file selected"}
                    </span>
                </div>

                <input
                    ref={fileInputRef}
                    type="file"
                    name="photo"
                    accept="image/*"
                    onChange={(e) => {
                        const file = e.target.files[0];
                        if (file && file.size < 2 * 1024 * 1024) {
                            setFormData((prev) => ({
                                ...prev,
                                photo: file,
                                previewUrl: URL.createObjectURL(file),
                            }));
                        }
                    }}
                    className="hidden"
                />

                {formData.previewUrl && (
                    <div className="relative mt-3 inline-block">
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
                    {userId ? 'Update' : 'Submit'}
                </button>
            </div>
        </form>
    );
};

export default UserForm;
