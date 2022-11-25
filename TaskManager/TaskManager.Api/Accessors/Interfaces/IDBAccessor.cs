﻿using TaskManager.Api.DO;
using TaskManager.Api.Models;

namespace TaskManager.Api.Accessors.Interfaces
{
    public interface IDBAccessor
    {
        Task<User> GetUserByCredentionalsAsync(UserLogin userLogin);
        Task<User> GetUserById(int userId);
    }
}