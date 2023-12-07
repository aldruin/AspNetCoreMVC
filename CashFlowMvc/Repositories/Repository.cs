﻿using CashFlowMvc.Data;
using CashFlowMvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CashFlowMvc.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> Query { get; set; }
        protected DbContext Context { get; set; }

        public Repository(AppDbContext context)
        {
            Context = context;
            Query = Context.Set<T>();
        }

        public async Task AddAsync(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "O model não pode ser nula.");
            }
            await Query.AddAsync(model);
            await Context.SaveChangesAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return Query.AnyAsync(expression);
        }

        public async Task DeleteAsync(Guid id)
        {
            var model = await Query.FindAsync(id);
            Query.Remove(model);
            await Context.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            try
            {
                var consulta = await Query.ToListAsync();
                return consulta;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Query.FindAsync(id);
        }

        public async Task UpdateAsync(T model)
        {
            Query.Update(model);
            await Context.SaveChangesAsync();
        }
    }
}
