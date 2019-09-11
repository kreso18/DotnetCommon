using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCommon.Tests.Data
{
    public class TestObject
    {
        public int Id { get; set; }

        public int Max10 { get; set; }

        public string MaxLength10 { get; set; }

        public int StatusMax3 { get; private set; }

        public TestObject(int statusMax3)
        {
            if (statusMax3 > 3)
                throw new InvalidOperationException();

            StatusMax3 = statusMax3;
        }

        public bool CanUpdateStatus() => StatusMax3 < 3;

        public virtual void UpdateStatus()
        {
            if (!CanUpdateStatus())
                throw new InvalidOperationException();

            StatusMax3++;
        }
    }
}
