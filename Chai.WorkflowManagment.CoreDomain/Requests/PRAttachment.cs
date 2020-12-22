using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class PRAttachment : IEntity
    {
        public PRAttachment()
        {
            this.ItemAccountChecklists = new List<ItemAccountChecklist>();
        }
        public int Id { get; set; }
        public string FilePath { get; set; }
     
        public virtual PaymentReimbursementRequestDetail PaymentReimbursementRequestDetail { get; set; }
        public virtual IList<ItemAccountChecklist> ItemAccountChecklists { get; set; }
    }
}
