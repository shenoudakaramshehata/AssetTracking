using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class DisposeModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public DateTime DateDisposed { get; set; }
        public string DisposeTo { get; set; }
        public string DisposeNotes { get; set; }
        public string photo { get; set; }
    }
}
