using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.BusinessRules
{	
	public class Workflow
	{

		private Collection<Task> _Tasks = new Collection<Task>();
		private WorkFlowType _ContextType = WorkFlowType.Uknown;
		public WorkFlowType ContextType {
			get { return _ContextType; }
			set { _ContextType = value; }
		}

		public Workflow()
		{

		}

		public bool Run(TaskContext c)
		{
			bool result = true;			

			for (int i = 0; i <= _Tasks.Count - 1; i++) {
				bool taskResult = false;
				try {
					taskResult = _Tasks[i].Execute(c);
				}
				catch (Exception ex) {
					if (!(ex is System.Threading.ThreadAbortException)) {
						taskResult = false;
						c.Errors.Add(new WorkflowMessage("EXCEPTION", ex.Message, false));
						EventLog.LogEvent(ex);
					}
				}

				if (taskResult == false) {
					result = false;
					Rollback(i, c);
					return false;
				}
			}

			return result;
		}

		public static bool RunByName(TaskContext c, WorkflowNames name)
		{
			bool result = false;
			Workflow wf = Workflow.Load(name);
			result = wf.Run(c);
			return result;
		}
		
		private bool Rollback(int startFromStepIndex, TaskContext c)
		{
			bool result = true;

			for (int i = startFromStepIndex; i >= 0; i += -1) {
				if (_Tasks[i].Rollback(c) == false) {
					result = false;
				}
			}

			return result;
		}
		
		public static BusinessRules.Workflow Load(WorkflowNames name)
		{
            Workflow wf = new Workflow();
            switch (name)
            {
                case WorkflowNames.CheckoutSelected:
                    wf._Tasks.Add(new OrderTasks.ApplyMinimumOrderAmount());
                    wf._Tasks.Add(new OrderTasks.CheckForOrderMaximums());
                    break;
                case WorkflowNames.DropShip:
                    wf._Tasks.Add(new OrderTasks.RunAllDropShipWorkflows());
                    break;
                case WorkflowNames.OrderEdited:
                    wf._Tasks.Add(new OrderTasks.AvalaraResubmitTaxes());
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    break;
                case WorkflowNames.PackageShipped:                    
                    break;
                case WorkflowNames.PaymentChanged:
                    wf._Tasks.Add(new OrderTasks.RunWorkFlowIfPaid());
                    wf._Tasks.Add(new OrderTasks.MarkCompletedWhenShippedAndPaid());
                    wf._Tasks.Add(new OrderTasks.ChangeOrderStatusWhenPaymentRemoved());
                    wf._Tasks.Add(new OrderTasks.AvalaraCancelWhenPaymentRemoved());
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    break;
                case WorkflowNames.PaymentComplete:
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    wf._Tasks.Add(new OrderTasks.AvalaraCommitTaxes());
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    wf._Tasks.Add(new OrderTasks.IssueGiftCertificates());
                    wf._Tasks.Add(new OrderTasks.IssueRewardsPoints());
                    wf._Tasks.Add(new OrderTasks.RunAllDropShipWorkflows());                    
                    break;
                case WorkflowNames.ProcessNewOrder:
                    // Change in 6.0.50.117
                    // 
                    // Instead of receiving payments here, call in separate process.
                    // End this workflow by making order placed
                    wf._Tasks.Add(new OrderTasks.WorkflowNote("Starting Process Order Workflow"));
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                                                            
                    wf._Tasks.Add(new OrderTasks.CheckForZeroDollarOrders());
                    wf._Tasks.Add(new OrderTasks.CreateUserAccountForNewCustomer());
                    wf._Tasks.Add(new OrderTasks.AssignOrderToUser());
                    wf._Tasks.Add(new OrderTasks.AssignOrderNumber());
                    wf._Tasks.Add(new OrderTasks.MakeOrderAddressUsersCurrentAddress());
                    wf._Tasks.Add(new OrderTasks.UpdateLineItemsForSave());
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    wf._Tasks.Add(new OrderTasks.MakePlacedOrder());
                    wf._Tasks.Add(new OrderTasks.WorkflowNote("Finished Process Order Workflow"));
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());                                                                                
                    break;
                case WorkflowNames.ProcessNewOrderPayments:
                    // Receive Payments and throw error if needed
                    //wf._Tasks.Add(new OrderTasks.DebitGiftCertificates());
                    wf._Tasks.Add(new OrderTasks.WorkflowNote("Starting Process Payment Workflow"));
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    wf._Tasks.Add(new OrderTasks.ReceivePaypalExpressPayments());
                    wf._Tasks.Add(new OrderTasks.ReceiveCreditCards());
                    wf._Tasks.Add(new OrderTasks.ReceiveRewardsPoints());
                    wf._Tasks.Add(new OrderTasks.WorkflowNote("Finished Process Payment Workflow"));
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    break;
                case WorkflowNames.ProcessNewOrderAfterPayments:
                    // After Payments, notify customer, etc.
                    wf._Tasks.Add(new OrderTasks.WorkflowNote("Starting Order After Payment Workflow"));
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    wf._Tasks.Add(new OrderTasks.LocalFraudCheck());
                    wf._Tasks.Add(new OrderTasks.RunWorkFlowIfPaid());
                    wf._Tasks.Add(new OrderTasks.MarkCompletedWhenShippedAndPaid());
                    wf._Tasks.Add(new OrderTasks.EmailOrder("Customer"));
                    wf._Tasks.Add(new OrderTasks.EmailOrder("Admin"));
                    wf._Tasks.Add(new OrderTasks.WorkflowNote("Finished Order After Payment Workflow"));
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    break;
                case WorkflowNames.ShippingChanged:
                    wf._Tasks.Add(new OrderTasks.MarkCompletedWhenShippedAndPaid());
                    wf._Tasks.Add(new OrderTasks.ChangeOrderStatusWhenShipmentRemoved());
                    wf._Tasks.Add(new OrderTasks.UpdateOrder());
                    wf._Tasks.Add(new OrderTasks.RunShippingCompleteWorkFlow());
                    break;
                case WorkflowNames.ShippingComplete:
                    wf._Tasks.Add(new OrderTasks.EmailShippingInfo());                    
                    break;
                case WorkflowNames.ThirdPartyCheckoutSelected:
                    wf._Tasks.Add(new OrderTasks.StartPaypalExpressCheckout());
                    break;
            }

            return wf;
		}
        
	}
}

