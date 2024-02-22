using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AssetProject.Models
{
    public class Asset
    {
        public int AssetId { set; get; }
        [Required]
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }

        //[Column(TypeName = "date")]
        public DateTime AssetPurchaseDate { set; get; }
        [JsonIgnore]
        public Item Item { set; get; }
        public int ItemId { set; get; }
        public string Photo { set; get; }
        [Required]
        public string PurchaseNo { get; set; }
        [NotMapped]
        public int? Quantity { set; get; }
        public bool DepreciableAsset { set; get; }
        public double? DepreciableCost { set; get; }
        public double? SalvageValue { set; get; }
        public int? AssetLife { set; get; }

        //[Column(TypeName = "date")]
        public DateTime? DateAcquired { set; get; }
        [JsonIgnore]
        public DepreciationMethod DepreciationMethod { set; get; }
        public int? DepreciationMethodId { set; get; }
        [JsonIgnore]
        public AssetStatus AssetStatus { set; get; }
        public int? AssetStatusId { set; get; }

        public int? VendorId { get; set; }
        [JsonIgnore]
        public virtual Vendor Vendor { get; set; }
        public int? StoreId { get; set; }
        [JsonIgnore]
        public virtual Store Store { get; set; }
        public int? TenantId { get; set; }
        [JsonIgnore]
        public virtual Tenant tenant { get; set; }
        [JsonIgnore]

        public ICollection<AssetPhotos> AssetPhotos { set; get; }
        [JsonIgnore]
        public ICollection<AssetContract> AssetContracts { get; set; }
        [JsonIgnore]
        public ICollection<AssetsInsurance> AssetsInsurances { get; set; }
        [JsonIgnore]
        public ICollection<AssetDocument> documents { get; set; }
        [JsonIgnore]
        public ICollection<AssetMovementDetails> AssetMovementDetails{ get; set; }
        [JsonIgnore]
        public ICollection<AssetLeasingDetails> AssetLeasingDetails{ get; set; }
        [JsonIgnore]
        public ICollection<AssetLostDetails> AssetLostDetails{ get; set; }
        [JsonIgnore]

        public ICollection<AssetDisposeDetails> AssetDisposeDetails{ get; set; }
        [JsonIgnore]
        public ICollection<AssetRepairDetails> AssetRepairDetails{ get; set; }
        [JsonIgnore]
        public ICollection<AssetSellDetails> AssetSellDetails{ get; set; }
        [JsonIgnore]
        public ICollection<AssetBrokenDetails> AssetBrokenDetails{ get; set; }
        [JsonIgnore]
        public ICollection<AssetWarranty> Warranty { get; set; }
        

    }
}
