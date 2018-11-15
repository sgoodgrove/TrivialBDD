using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialBDD.Interfaces
{
    internal class DateTime : IDateTime
    {
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}
