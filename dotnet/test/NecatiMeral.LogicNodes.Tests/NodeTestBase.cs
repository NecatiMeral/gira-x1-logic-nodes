using System;
using LogicModule.Nodes.TestHelper;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests;

public abstract class NodeTestBase<TNode>
    where TNode : ILogicNode
{
    protected INodeContext Context { get; }

    protected NodeTestBase()
    {
        Context = TestNodeContext.Create();
    }

    protected TNode CreateNode()
    {
        return (TNode)Activator.CreateInstance(typeof(TNode), new object[] { Context });
    }
}
