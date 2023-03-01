using Necati_Meral_Yahoo_De.Logic.ComfortOnline;
using Necati_Meral_Yahoo_De.LogicNodes.Tests;
using Shouldly;

namespace Necati_Meral_Yahoo_De;
public class ComfortOnlineListenerNodeIntegrationTests : NodeTestBase<ComfortOnlineRequestNode>
{
    const string PlantId = "10458";
    const string PlantSectionId = "96_0";
    const string UserName = "";
    const string Password = "";

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
        node.UserName.Value = UserName;
        node.Password.Value = Password;
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

        node.UserName.Value = UserName;
        node.Password.Value = Password;

        node.Execute();

        node.Diagnostics.Value.ShouldBe(ComfortOnlineConsts.ErrorCodes.Ok);
    }
}
