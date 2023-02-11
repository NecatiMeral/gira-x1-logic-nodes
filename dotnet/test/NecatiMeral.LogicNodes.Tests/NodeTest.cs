using LogicModule.Nodes.TestHelper;
using LogicModule.ObjectModel;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests;

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
