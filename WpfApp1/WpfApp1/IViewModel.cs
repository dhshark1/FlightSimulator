using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace WpfApp1
{
    interface IViewModel
    {

        //properties
        public string VM_CsvPath
        {
            get;
            set;
        }
        
    }
}
