using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class FixedAsset : IEntity
    {
        public FixedAsset()
        {
            FixedAssetHistories = new List<FixedAssetHistory>();
        }
        public int Id { get; set; }
        public string CPVNo { get; set; }
        public string ReceiveNo { get; set; }
        public Nullable<DateTime> ReceiveDate { get; set; }
        public string AssetCode { get; set; }
        public string AssetStatus { get; set; }
        public string SerialNo { get; set; }
        public string Custodian { get; set; }
        public string Location { get; set; }
        public decimal UnitCost { get; set; }
        public string Condition { get; set; }
        public int TotalLife { get; set; }
        public string Remark { get; set; }
        public bool CheckedByIt { get; set; }
        public string ReturnRemark { get; set; }
        public virtual Receive Receive { get; set; }
        public virtual Item Item { get; set; }
        public virtual Store Store { get; set; }
        public virtual Section Section { get; set; }
        public virtual Shelf Shelf { get; set; }
        public virtual Supplier Supplier { get; set; }
        public IList<FixedAssetHistory> FixedAssetHistories { get; set; }
        public virtual FixedAssetHistory GetFixedAssetHistory(int Id)
        {
            foreach (FixedAssetHistory fah in FixedAssetHistories)
            {
                if (fah.Id == Id)
                    return fah;
            }
            return null;
        }
        public virtual void RemoveFixedAssetHistory(int Id)
        {
            foreach (FixedAssetHistory fah in FixedAssetHistories)
            {
                if (fah.Id == Id)
                    FixedAssetHistories.Remove(fah);
                break;
            }
        }
    }
}
