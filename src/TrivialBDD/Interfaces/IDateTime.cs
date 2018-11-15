using System;

namespace TrivialBDD.Interfaces
{
    internal interface IDateTime
    {
        System.DateTime UtcNow { get; }
    }
}
