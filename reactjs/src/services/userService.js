import { userApi } from "../api/userApi";

export const userService = {
    async getAllUsers(params) {
        const res = await userApi.getUsers(params);
        return res.data.data;
    },
    async createUser(formData) {
        return await userApi.registerUser(formData);
    },
    async updateUser(id,formData) {
        return await userApi.updateUser(id,formData);
    },
    async getUser(id) {
        return await userApi.getUser(id);
    },
    async deleteUser(formData) {
        return await userApi.delete(formData);
    },

    async fetchStates() {
        const res = await userApi.getStates();
        return res.data.data;
    },
    async fetchCities(stateId) {
        const res = await userApi.getCities(stateId);
        return res.data.data;
    },
    async fetchHobbies() {
        const res = await userApi.getHobbies();
        return res.data.data;
    },
    async fetchGenders() {
        const res = await userApi.getGenders();
        return res.data.data;
    },
};
