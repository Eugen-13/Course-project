using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc
{
    public class User
    {

        public User(string login, string password, string email, string surname, string name, string secondaryName,string adres, DateTime date, string telephone, bool isAdmin)
        {
            Login = login;
            Password = password;
            this.Email = email;
            Surname = surname;
            Name = name;
            this.Adres = adres;
            SecondaryName = secondaryName;
            this.date = date;
            Telephone = telephone;
            this.isAdmin = isAdmin;
        }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }    
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondaryName { get; set; }
        public string Adres { get; set; }
        private DateTime date { get; set; }
        public string Date
        {
            get
            {
                return date.ToString("yyyy.MM.dd");
            }
            set
            {
               this.date = DateTime.Parse(value);
            }
        }
        public string Telephone { get; set; }
        private bool isAdmin { get; set; }

        public override string ToString()
        {
            return $"Login: {this.Login}  Password: {this.Password}\nEmail: {this.Email}\nFIO: {this.Surname} {this.Name} {this.SecondaryName}\nDate: {this.Date} Adres: {this.Adres} Tel: {this.Telephone}\n\n";
        }

    }
}
