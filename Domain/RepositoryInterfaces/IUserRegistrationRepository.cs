﻿using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRegistrationRepository : IGenericRepository<TrnUserRegistration>
    {
        Task<bool> CheckUserExistsByEmail(string email);
        Task UpdateHobbies(Guid userId, ICollection<TrnUserHobby> userHobbies);
    }
}
