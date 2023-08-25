using System;
using System.Numerics;

namespace Sushi.Level.Conveyor.Data
{
    public interface ITickProvider
    {
        event Action<Vector3> UpdateHappened;
    }
}