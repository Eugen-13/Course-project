using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zxc
{
    public class Computers
    {
        public int ID { get; set; }
        public string Class { get; set; }
        public int Cost { get; set; }
        public string Stats { get; set; }
        public string Devices { get; set; }

        public Computers(int iD, string @class, int cost, string stats, string devices)
        {
            ID = iD; 
            Class = @class;
            Cost = cost;
            Stats = stats;
            Devices = devices;
        }
        public override string ToString()
        {
            return $"ID: {this.ID} Class: {this.Class}\nCost: {this.Cost} BYN\nStats: {this.Stats}\nDevices: {this.Devices}\n\n";
        }
    }
}
