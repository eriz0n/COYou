using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coyou.Model
{
    public class KeyNameModel
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public KeyNameModel(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
