using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class ELRAttachment : IEntity
    {
        public ELRAttachment()
        {
            this.ItemAccountChecklists = new List<ItemAccountChecklist>();
        }
        public int Id { get; set; }
        public string FilePath { get; set; }
        public virtual ExpenseLiquidationRequestDetail ExpenseLiquidationRequestDetail { get; set; }
        public virtual IList<ItemAccountChecklist> ItemAccountChecklists { get; set; }
    }
}
