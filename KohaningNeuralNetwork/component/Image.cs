using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohaningNeuralNetwork.component
{
    class Image
    {
        public string ShortFileName { get; set; }
        public string Uri { get; set; }
        public int[] Pixels { get; set; }
        public int Answer { get; set; }
    }
}
