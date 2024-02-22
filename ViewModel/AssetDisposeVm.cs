using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ViewModel
{
    public class AssetDisposeVm
    {
       
        public int AssetId { get; set; }
        public DateTime DateDisposed { get; set; }
        public string DisposeTo { get; set; }
        public string Notes { get; set; }


    }
}
