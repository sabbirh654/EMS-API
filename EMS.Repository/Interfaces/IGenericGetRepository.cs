﻿namespace EMS.Repository.Interfaces;

public interface IGenericGetRepository<T> where T : class
{
    Task<IEnumerable<T>?> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}
