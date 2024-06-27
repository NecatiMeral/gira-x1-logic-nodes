using System;
using LogicModule.Nodes.TestHelper;
using Microsoft.Extensions.Logging;
using Necati_Meral_Yahoo_De.LogicNodes.Tests.Logging;
using Xunit.Abstractions;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests;

public abstract class NodeTestBase<TNode>
    where TNode : ILogicNode
{
    protected INodeContext Context { get; }
    protected ILogger Logger { get; }

    private TNode _sut;
    protected TNode Sut => _sut ??= CreateNode();

    protected NodeTestBase()
    {
        Context = TestNodeContext.Create();
        Logger = new MemoryLogger();
    }

    protected NodeTestBase(ITestOutputHelper testOutputHelper)
    {
        Context = TestNodeContext.Create();
        Logger = new XUnitLogger(testOutputHelper);
    }

    protected TNode CreateNode()
        => CreateNode<TNode>();

    protected TLogicNode CreateNode<TLogicNode>()
        => (TLogicNode)Activator.CreateInstance(typeof(TLogicNode), [Context]);
}
