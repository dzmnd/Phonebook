using Business.Context;
using Business.Models.DbModels;
using Phonebook_Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook_Api.Services
{
    public class PhonebookService : IPhonebookService
    {
        private readonly PhonebookContext _phonebookContext;

        public PhonebookService(PhonebookContext phonebookContext) 
        {
            _phonebookContext = phonebookContext;
        }

        public async Task<User> GetUser(long userId)
        {
            User dbUser = new User();
            try
            {
                dbUser = this._phonebookContext.Users.FirstOrDefault(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dbUser;
        }

        public async Task<List<User>> GetUsers(int pageNumber, int pageSize) 
        {
            List<User> dbUsers = new List<User>();

            try
            {
                dbUsers = this._phonebookContext.Users.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dbUsers;
        } 
    }
}
