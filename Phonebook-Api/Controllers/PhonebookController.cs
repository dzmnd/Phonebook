using Business.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Phonebook_Api.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phonebook_Api.Controllers
{
    public class PhonebookController : Controller
    {
        private readonly IPhonebookService _phonebookService;
        public PhonebookController(IPhonebookService phonebookService)
        {
            _phonebookService = phonebookService;
        }

        public async Task<User> GetUser(long userId)
        {
            return await this._phonebookService.GetUser(userId);
        }

        public async Task<List<User>> GetUsers(int pageNumber, int pageSize) 
        {
            return await this._phonebookService.GetUsers(pageNumber, pageSize);
        }
    }
}
