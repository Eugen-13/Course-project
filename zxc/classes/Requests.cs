using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc
{
    public class Requests
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public int Computer { get; set; }
        public DateTime date { get; set; }
        public string isAccept { get; set; }

        public string getA
        {
            get
            {
                if (isAccept == "True")
                    return "Подтверждена";
                else
                    return "В ожидании";
            }
        }
        public Requests(int iD, string login, int pc, DateTime date, string isAccept)
        {
            ID = iD;
            Login = login;
            Computer = pc;
            this.date = date;
            this.isAccept = isAccept;
        }
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
        public override string ToString()
        {
            return $"ID: {this.ID}\nUser login: {this.Login}\nComputer_ID: {this.Computer}\nDate: {this.Date}\n\n";
        }
    }
}
