using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using POSTmanager.Models;
using SQLitePCL;

namespace POSTmanager.Helpers
{
    internal class AuthHelper
    {
        public static User CurrentUser;
        private PmDbContext _context = new PmDbContext();
        public AuthHelper(string login, string password) 
        {
            try
            {
                // TODO: crypto?
                CurrentUser = _context.Users
                .Where(u => u.Login == login && u.Password == password)
                .Single();
            }
            catch (Exception)
            {
                return;
            }

        }
    }
}
