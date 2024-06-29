using System;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.Core;
public class NullDisposable : IDisposable
{
    public static NullDisposable Instance { get; } = new NullDisposable();

    private NullDisposable() { }

    public void Dispose() { }
}
