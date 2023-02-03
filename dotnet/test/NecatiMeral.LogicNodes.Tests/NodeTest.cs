using LogicModule.Nodes.TestHelper;
using LogicModule.ObjectModel;

namespace NecatiMeral.LogicNodes.Tests;

public class NodeTest
{
    protected INodeContext Context { get; }

    public NodeTest()
    {
        Context = TestNodeContext.Create();
    }

    [Fact]
    public void YourTest()
    {

    }
}
