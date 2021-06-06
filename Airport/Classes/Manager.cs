using Airport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace Airport.Classes
{
    public class Manager
    {
        public static Frame mainFrame { get; set; }
        public static Users user;
        public static bool IsValidLogin(string login)
        {
            int valid = 0;
            foreach (char c in login)
            {
                if (c >= 'a' && c <= 'z')
                {
                    valid++;
                    break;
                }
            }
            foreach (char c in login)
            {
                if (c >= '0' && c <= '9')
                {
                    valid++;
                    break;
                }
            }
            if (login.Length > 6)
                valid++;
            if (valid == 3)
                return true;
            else
                return false;
        }
    }
}
