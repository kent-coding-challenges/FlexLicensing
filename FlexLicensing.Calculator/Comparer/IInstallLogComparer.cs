using System;
using System.Collections.Generic;
using FlexLicensing.Calculator.Models;

namespace FlexLicensing.Calculator.Comparer
{
    public class InstallLogComparer : IEqualityComparer<InstallLog>
    {
        public bool Equals(InstallLog x, InstallLog y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.ApplicationID == y.ApplicationID
                && x.ComputerID == y.ComputerID
                && x.UserID == y.UserID
                && x.ComputerType == y.ComputerType;
        }

        public int GetHashCode(InstallLog obj)
        {
            if (obj == null)
                return 0;

            return obj.ApplicationID.GetHashCode()
                ^ obj.ComputerID.GetHashCode()
                ^ obj.UserID.GetHashCode()
                ^ obj.ComputerType.GetHashCode();
        }
    }
}
