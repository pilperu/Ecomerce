using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;

namespace MerchantTribe.Commerce.Payment
{
    public class RewardPoints : MerchantTribe.Payment.Method
    {        
        private RewardPointsSettings _Settings = new RewardPointsSettings(0);

        public RewardPointsSettings Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }

        public override string Name
        {
            get { return "Reward Points"; }
        }

        public override string Id
        {
            get { return "4629835A-76E0-4B6E-A72B-0D8044DB0052"; }
        }

        public override void ProcessTransaction(Transaction t)
        {
            Membership.CustomerPointsManager manager =
                Membership.CustomerPointsManager.InstantiateForDatabase(Settings.PointsIssuedPerDollarSpent,
                                                    Settings.PointsNeededForDollarCredit,                                                    
                                                    Settings.CurrentStoreId);
            switch (t.Action)
            {
                case ActionType.RewardPointsBalanceInquiry:
                    int points = manager.FindAvailablePoints(t.Customer.UserId);
                    t.Result.Succeeded = true;
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Points Availble " + points.ToString();
                    t.Result.BalanceAvailable = points;
                    break;
                case ActionType.RewardPointsCapture:
                    if (manager.CapturePoints(t.Customer.UserId,t.RewardPoints))
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "OK";
                        t.Result.ResponseCodeDescription = t.RewardPoints.ToString() + " Points Captured";                        
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new Message("Not enough points available to capture", "FAIL", MessageType.Error));
                    }
                    break;
                case ActionType.RewardPointsDecrease:
                    if (manager.DecreasePoints(t.Customer.UserId,t.RewardPoints))
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "OK";
                        t.Result.ResponseCodeDescription = t.RewardPoints.ToString() + " Points Used";
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new Message("Unable to Use Points", "FAIL", MessageType.Error));
                    }
                    break;
                case ActionType.RewardPointsHold:
                    if (manager.HoldPoints(t.Customer.UserId,t.RewardPoints))
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "OK";
                        t.Result.ResponseCodeDescription = t.RewardPoints.ToString() + " Points Held";
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new Message("Not enough points available to hold", "FAIL", MessageType.Error));
                    }
                    break;
                case ActionType.RewardPointsUnHold:
                    if (manager.UnHoldPoints(t.Customer.UserId, t.RewardPoints))
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "OK";
                        t.Result.ResponseCodeDescription = t.RewardPoints.ToString() + " Points Held";
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new Message("Not enought points held to unhold", "FAIL", MessageType.Error));
                    }
                    break;
                case ActionType.RewardPointsIncrease:
                    if (manager.IssuePoints(t.Customer.UserId,t.RewardPoints))
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "OK";
                        t.Result.ResponseCodeDescription = t.RewardPoints.ToString() + " Points Issued";
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new Message("Unable to Issue Points", "FAIL", MessageType.Error));
                    }
                    break;
                case ActionType.RewardPointsInfo:
                    t.Result.Succeeded = true;
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Reward Points Info";
                    break;
                default:
                    t.Result.Succeeded = false;
                    t.Result.Messages.Add(new Message("Operation Not Supported by this Method", "OPFAIL", MessageType.Error));
                    break;
            }
        }

        public override MethodSettings BaseSettings
        {
            get { return _Settings; }
        }
    }
}
