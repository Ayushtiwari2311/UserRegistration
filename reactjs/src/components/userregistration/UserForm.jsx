import React, { useEffect, useState } from 'react';
import { userService } from "../../services/userService";

const UserForm = ({ onSubmit }) => {
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
        const hobbies = prev.hobbies || [];
        return {
          ...prev,
          hobbies: checked ? [...hobbies, value] : hobbies.filter((h) => h !== value)
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
    <form onSubmit={submit}>
      <input name="name" placeholder="Name" onChange={handleChange} required />
      <input name="email" placeholder="Email" onChange={handleChange} required />
      <select name="gender" onChange={handleChange}>
        {genders.map(g => <option key={g.id} value={g.name}>{g.name}</option>)}
      </select>
      <input name="mobile" placeholder="Mobile" onChange={handleChange} />
      <input type="date" name="dateOfBirth" onChange={handleChange} />

      <select name="state" onChange={handleChange}>
        {states.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
      </select>

      <select name="city" onChange={handleChange}>
        {cities.map(c => <option key={c.id} value={c.name}>{c.name}</option>)}
      </select>

      {hobbies.map(h => (
        <label key={h.id}>
          <input type="checkbox" value={h.id} name="hobbies" onChange={handleChange} />
          {h.name}
        </label>
      ))}

      <button type="submit">Submit</button>
    </form>
  );
};

export default UserForm;
