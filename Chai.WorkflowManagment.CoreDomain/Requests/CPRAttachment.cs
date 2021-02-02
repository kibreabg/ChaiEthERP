using Chai.WorkflowManagment.CoreDomain.Setting;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class CPRAttachment : IEntity
    {
        public CPRAttachment()
        {
            this.ItemAccountChecklists = new List<ItemAccountChecklist>();
            this.CashPaymentRequestDetails = new List<CashPaymentRequestDetail>();
        }
        public int Id { get; set; }
        public string FilePath { get; set; }
        public virtual IList<CashPaymentRequestDetail> CashPaymentRequestDetails { get; set; }
        public virtual IList<ItemAccountChecklist> ItemAccountChecklists { get; set; }

    }
}
