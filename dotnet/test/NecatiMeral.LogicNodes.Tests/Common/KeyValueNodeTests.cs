using Necati_Meral_Yahoo_De.Logic.Common;
using Shouldly;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.Common;
public class KeyValueNodeTests : NodeTestBase<KeyValueNode>
{
    [Fact]
    public void Should_Select_Last_Value()
    {
        var node = CreateNode();

        node.InputCount.Value = 2;
        node.Inputs[0].Value = "1|BMW";
        node.Inputs[1].Value = "2|Audi";
        node.Input.Value = "2";

        node.Execute();

        node.Output.Value.ShouldBe("Audi");
    }

    [Fact]
    public void Should_Reverse()
    {
        var node = CreateNode();

        node.InputCount.Value = 2;
        node.Inputs[0].Value = "1|BMW";
        node.Inputs[1].Value = "2|Audi";
        node.Input.Value = "2";

        node.Execute();

        node.Output.Value.ShouldBe("Audi");

        node.Input.Value = "BMW";
        node.Reverse.Value = true;

        node.Execute();
        node.Output.Value.ShouldBe("1");
    }
}
