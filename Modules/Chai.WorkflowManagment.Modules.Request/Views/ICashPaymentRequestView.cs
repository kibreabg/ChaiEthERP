namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ICashPaymentRequestView
    {
        int GetCashPaymentRequestId { get; }
        string GetRequestNo { get; }
        int GetPayee { get; }
        string GetRequestType { get; }
        string GetDescription { get; }
        string GetArrivalDateTime { get; }
        string GetReturnDateTime { get; }
        string GetVoucherNo { get; }
        string GetAmountType { get; }
        int GetProgram { get; }
    }
}




