using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models.DbModels
{
    public class User
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Photo { get; set; }
    }
}
