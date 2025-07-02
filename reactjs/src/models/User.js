export default class User {
    constructor({ id, name, email, gender, dateOfBirth, mobile, state, city, hobbies, photoPath }) {
        this.id = id;
        this.name = name;
        this.email = email;
        this.gender = gender;
        this.dateOfBirth = dateOfBirth;
        this.mobile = mobile;
        this.state = state;
        this.city = city;
        this.hobbies = hobbies;
        this.photoPath = photoPath;
    }
}
