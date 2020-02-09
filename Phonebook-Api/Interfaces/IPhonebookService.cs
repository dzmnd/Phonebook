using Business.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phonebook_Api.Interfaces
{
    public interface IPhonebookService
    {
        Task<User> GetUser(long userId);
        Task<List<User>> GetUsers(int pageNumber, int pageSize);
    }
}
