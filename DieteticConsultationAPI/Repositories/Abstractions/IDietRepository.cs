﻿using DieteticConsultationAPI.Entities;

namespace DieteticConsultationAPI.Repositories.Abstractions
{
    public interface IDietRepository
    {
        Task<ICollection<Diet>> GetAll();
        Task<Diet> GetById(int id);
        Task<Diet> AddOrUpdate(Diet diet);
        Task Delete(int id);
    }
}
