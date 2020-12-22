using System;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class FixedAssetHistory : IEntity
    {
        public int Id { get; set; }        
        public string Custodian { get; set; }
        public string Operation { get; set; }
        public Nullable<DateTime> TransactionDate { get; set; }
        public virtual FixedAsset FixedAsset { get; set; }
    }
}
