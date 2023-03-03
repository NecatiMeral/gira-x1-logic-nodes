using Necati_Meral_Yahoo_De.Logic.ComfortOnline;
using Shouldly;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.ComfortOnline;
public class ComfortOnlineListenerNodeTests : NodeTestBase<TestComfortOnlineRequestNode>
{
    const string PlantId = "10458";
    const string PlantSectionId = "96_0";

    [Fact]
    public void Should_Fail_Login()
    {
        var node = CreateNode();

        node.Trigger.Value = true;
        node.PlantId.Value = PlantId;
        node.PlantSection.Value = PlantSectionId;

        node.Execute();

        node.Diagnostics.Value.ShouldBe(ComfortOnlineConsts.ErrorCodes.InvalidCredentials);
    }

    [Fact]
    public void Should_Login()
    {
        var node = CreateNode();

        node.Trigger.Value = true;
        node.UserName.Value = ComfortOnlineTestConsts.UserName;
        node.Password.Value = ComfortOnlineTestConsts.Password;
        node.PlantId.Value = PlantId;
        node.PlantSection.Value = PlantSectionId;

        node.Execute();

        node.Diagnostics.Value.ShouldBe(ComfortOnlineConsts.ErrorCodes.Ok);
    }

    [Fact]
    public void Should_Recover_From_Error()
    {
        var node = CreateNode();

        node.Trigger.Value = true;
        node.PlantId.Value = PlantId;
        node.PlantSection.Value = PlantSectionId;

        node.Execute();

        node.Diagnostics.Value.ShouldBe(ComfortOnlineConsts.ErrorCodes.InvalidCredentials);

        node.UserName.Value = ComfortOnlineTestConsts.UserName;
        node.Password.Value = ComfortOnlineTestConsts.Password;

        node.Execute();

        node.Diagnostics.Value.ShouldBe(ComfortOnlineConsts.ErrorCodes.Ok);
    }
}
