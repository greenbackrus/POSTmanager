using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using POSTmanager.Helpers;
using POSTmanager.Models;

namespace POSTmanager
{
    /// <summary>
    /// Логика взаимодействия для RestsListWindow.xaml
    /// </summary>
    public partial class RestsListWindow : Window
    {
        private PmDbContext _context = new PmDbContext();
        private List<UserRest> _ableRests;

        public RestsListWindow()
        {
            InitializeComponent();
            _ableRests = _context.UserRests
                .Where(ur => ur.UserId == AuthHelper.CurrentUser.Id)
                .ToList();
            RestsGrid.DataContext = _ableRests;
        }
    }
}
