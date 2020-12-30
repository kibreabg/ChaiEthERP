using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class ExpenseLiquidationRequestDetail : IEntity
    {
        public ExpenseLiquidationRequestDetail()
        {
            this.ELRAttachments = new List<ELRAttachment>();
        }
        public int Id { get; set; }
        public decimal AmountAdvanced { get; set; }
        public decimal ActualExpenditure { get; set; }
        public decimal Variance { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
        public virtual ExpenseLiquidationRequest ExpenseLiquidationRequest { get; set; }
        public virtual IList<ELRAttachment> ELRAttachments { get; set; }
        #region ELAttachment
        public virtual void RemoveELAttachment(string FilePath)
        {
            foreach (ELRAttachment ela in ELRAttachments)
            {
                if (ela.FilePath == FilePath)
                {
                    ELRAttachments.Remove(ela);
                    break;
                }
            }
        }
        #endregion
    }
}
