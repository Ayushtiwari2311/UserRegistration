import axios from 'axios';
import { store } from '../loading/store';

const BASE_URL = 'https://localhost:7155';

const api = axios.create({
    baseURL: BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Automatically attach token if it exists
api.interceptors.request.use(
    (config) => {
        store.setLoading(true);
        const token = localStorage.getItem('token');
        if (token) config.headers.Authorization = `Bearer ${token}`;
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
        return Promise.reject(error);
    }
);

export const userApi = {
    login: (data) => api.post('/Auth/login', data),
    register: (data) => api.post('/Auth/register', data),
    getUsers: (params) => api.get('/UserRegistration', { params }),
    registerUser: (data) => api.post('/UserRegistration', data),
    getStates: () => api.get('/Masters/states'),
    getCities: (stateId) => api.get(`/Masters/cities/${stateId}`),
    getHobbies: () => api.get('/Masters/hobbies'),
    getGenders: () => api.get('/Masters/genders'),
    setAuthHeader: (token) => {
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }
};
