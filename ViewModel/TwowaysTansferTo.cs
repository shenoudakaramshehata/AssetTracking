using AssetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ViewModel
{
    public class TwowaysTansferTo
    {
        public int? LeftActionTypeId { get; set; }
        public int? RightActionTypeId { get; set; }
        public int? LeftDepartmentId { get; set; }
        public int? RightDepartmentId { get; set; }
        public int? LeftEmployeeId { get; set; }
        public int? RightEmployeeId { get; set; }
        public int? LeftLocationId { get; set; }
        public int? RightLocationId { get; set; }
        public int? LeftStoreId { get; set; }
        public int? RightStoreId { get; set; }

        public List<AssetVm2> SelectedLeftAssets { get; set; }
        public List<AssetVm2> RightDataSource { get; set; }

        public List<AssetVm2> SelectedRightAssets { get; set; }
        public List<AssetVm2> LeftDataSource { get; set; }
        public List<AssetVm2> RightEmployeeDataSource { get; set; }
        public List<AssetVm2> LeftEmployeeDataSource { get; set; }
        public List<AssetVm2> LeftDepartmentDataSource { get; set; }
        public List<AssetVm2> RightDepartmentDataSource { get; set; }

    }
}
