import axios from 'axios';

const API_BASE_URL = 'http://localhost:5000/UserRegistration'; // Change to your actual API base URL if different

// Helper to get auth token if needed
function getAuthHeaders() {
  const token = localStorage.getItem('token'); // Adjust if you use a different storage or key
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export const userRegistrationService = {
  // List users with optional filters and pagination
  list: (params) =>
    axios.get(API_BASE_URL, {
      params,
      headers: { ...getAuthHeaders() },
    }),

  // Get user by ID
  get: (id) =>
    axios.get(`${API_BASE_URL}/${id}`, {
      headers: { ...getAuthHeaders() },
    }),

  // Create user (with file upload)
  create: (data) => {
    const formData = new FormData();
    Object.entries(data).forEach(([key, value]) => {
      if (Array.isArray(value)) {
        value.forEach((v) => formData.append(`${key}[]`, v));
      } else if (value !== undefined && value !== null) {
        formData.append(key, value);
      }
    });
    return axios.post(API_BASE_URL, formData, {
      headers: { 'Content-Type': 'multipart/form-data', ...getAuthHeaders() },
    });
  },

  // Update user (with file upload)
  update: (id, data) => {
    const formData = new FormData();
    Object.entries(data).forEach(([key, value]) => {
      if (Array.isArray(value)) {
        value.forEach((v) => formData.append(`${key}[]`, v));
      } else if (value !== undefined && value !== null) {
        formData.append(key, value);
      }
    });
    return axios.put(`${API_BASE_URL}/${id}`, formData, {
      headers: { 'Content-Type': 'multipart/form-data', ...getAuthHeaders() },
    });
  },

  // Patch user (partial update, with file upload)
  patch: (id, data) => {
    const formData = new FormData();
    Object.entries(data).forEach(([key, value]) => {
      if (Array.isArray(value)) {
        value.forEach((v) => formData.append(`${key}[]`, v));
      } else if (value !== undefined && value !== null) {
        formData.append(key, value);
      }
    });
    return axios.patch(`${API_BASE_URL}/${id}`, formData, {
      headers: { 'Content-Type': 'multipart/form-data', ...getAuthHeaders() },
    });
  },

  // Delete user
  delete: (id) =>
    axios.delete(`${API_BASE_URL}/${id}`, {
      headers: { ...getAuthHeaders() },
    }),
}; 