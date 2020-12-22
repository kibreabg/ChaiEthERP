using Chai.WorkflowManagment.CoreDomain.Inventory;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("PurchaseRequestDetails")]
    public partial class PurchaseRequestDetail : IEntity
    {
        public int Id { get; set; }
        public string ItemDescription { get; set; }
        public string PurposeOfPurchase { get; set; }
        public int Qty { get; set; }
        public int ApprovedQuantity { get; set; }
        public string UnitOfMeasurment { get; set; }
        public decimal Priceperunit { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Remark { get; set; }
        public string BidAnalysisRequestStatus { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual Item Item { get; set; }
        public string AccountCode { get; set; }     
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }     
        public virtual PurchaseRequest PurchaseRequest { get; set; }

    }
}
