using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Enums
{
    public enum OutcomesLimitsPeriodsEnum : byte
    {
        PerDay = 1,
        PerWeek = 2,
        PerMonth = 3,
        PerHalfYear = 4,
        PerYear = 5
    }
}
