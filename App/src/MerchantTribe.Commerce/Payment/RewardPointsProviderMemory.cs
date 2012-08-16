using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MerchantTribe.Commerce.Payment
{
    class RewardPointsProviderMemory: IRewardPointsProvider
    {
        private List<Data.EF.bvc_RewardsPoints> pointsData = new List<Data.EF.bvc_RewardsPoints>();

        public RewardPointsProviderMemory()
        {
            
        }

        public int RewardPointsTotalIssued(long storeId)
        {
            return pointsData.Where(y => y.StoreId == storeId).Sum(y => y.Points);
        }
        public int RewardPointsReservedForStore(long storeId)
        {
            return pointsData.Where(y => y.StoreId == storeId).Sum(y => y.PointsHeld);
        }

        public int RewardPointsAvailableForUser(string userId, long storeId)
        {
            return pointsData.Where(y => y.StoreId == storeId).Where(y => y.UserId == userId).Sum(y => y.Points);
        }
        public int RewardPointsHeldForUser(string userId, long storeId)
        {
            return pointsData.Where(y => y.StoreId == storeId).Where(y => y.UserId == userId).Sum(y => y.PointsHeld);
        }

        public int CapturePoints(string userId, long storeId, int points)
        {
                int currentPoints = RewardPointsHeldForUser(userId, storeId);
                if (currentPoints - points >= 0)
                {
                    pointsData.Add(new Data.EF.bvc_RewardsPoints() { PointsHeld = (-1 * points), StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                    return 1;
                }
                return 0;
        }
        public int DecreasePoints(string userId, long storeId, int points)
        {
            int currentPoints = RewardPointsAvailableForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                pointsData.Add(new Data.EF.bvc_RewardsPoints() { Points = (-1 * points), StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                return 1;
            }
            return 0;
        }
        public int HoldPoints(string userId, long storeId, int points)
        {
            int currentPoints = RewardPointsAvailableForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                pointsData.Add(new Data.EF.bvc_RewardsPoints() { Points = (-1 * points), PointsHeld = points, StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                return 1;
            }
            return 0;
        }
        public int UnHoldPoints(string userId, long storeId, int points)
        {
            int currentPoints = RewardPointsAvailableForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                pointsData.Add(new Data.EF.bvc_RewardsPoints() { Points = points, PointsHeld = (-1 * points), StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                return 1;
            }
            return 0;
        }
        public int IssuePoints(string userId, long storeId, int points)
        {
            pointsData.Add(new Data.EF.bvc_RewardsPoints() { Points = points, StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
            return 1;
        }
    }
}
