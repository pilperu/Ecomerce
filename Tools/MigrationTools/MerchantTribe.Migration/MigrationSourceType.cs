using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Migration
{
    public enum MigrationSourceType
    {
        BVC2004 = 1,
        BV5 = 0,
        MerchantTribe = 2,
        ReIndexOnly = 3
    }
}
