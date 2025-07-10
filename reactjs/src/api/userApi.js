import axios from 'axios';
import { store } from '../loading/store';

const BASE_URL = 'https://localhost:7155';

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Automatically attach token if it exists
api.interceptors.request.use(
    (config) => {
        store.setLoading(true);
        return config;
    },
    (error) => {
        store.setLoading(false);
        return Promise.reject(error);
    }
);

api.interceptors.response.use(
    (res) => {
        store.setLoading(false);
        return res;
    },
    (error) => {
        store.setLoading(false);
        if (error.response?.status === 401) {
            window.location.href = '/login';  
        }
        return Promise.reject(error);
    }
);

export const userApi = {
    login: (data) => api.post('/Auth/login', data),
    logout: () => api.get('/Auth/logout'),
    register: (data) => api.post('/Auth/register', data),
    getUsers: (params) => api.get('/UserRegistration', { params }),
    registerUser: (data) => api.post('/UserRegistration', data, {
        headers: {
            'Content-Type': 'multipart/form-data',
        }
    }),
    updateUser: (id,data) => api.put(`/UserRegistration/${id}`, data, {
        headers: {
            'Content-Type': 'multipart/form-data',
        }
    }),
    getUser: (id) => api.get(`/UserRegistration/${id}`),
    delete: (data) => api.delete(`/UserRegistration/${data}`),
    getStates: () => api.get('/Masters/states'),
    getCities: (stateId) => api.get(`/Masters/cities/${stateId}`),
    getHobbies: () => api.get('/Masters/hobbies'),
    getGenders: () => api.get('/Masters/genders'),
    //setAuthHeader: (token) => {
    //    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    //}
};
