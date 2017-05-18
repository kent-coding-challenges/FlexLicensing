using FlexLicensing.Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexLicensing.Calculator.Tests.Factory
{
    public static class InstallWatcherFactory
    {
        public static InstallWatcher NewInstance => new InstallWatcher();

        public static InstallWatcher NewInstanceWithDummyData
        {
            get
            {
                var instance = NewInstance;
                var keys = instance.Watch.Keys.ToList();

                uint count = 5;
                foreach(var key in keys)
                {
                    instance.Watch[key] = count;
                    count += 5;
                }

                return instance;
            }
        }
    }
}
