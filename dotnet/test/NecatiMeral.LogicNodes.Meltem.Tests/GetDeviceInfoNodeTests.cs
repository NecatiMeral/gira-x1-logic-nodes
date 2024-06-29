using Necati_Meral_Yahoo_De.Logic.Meltem;
using Xunit.Abstractions;

namespace Necati_Meral_Yahoo_De.LogicNodes.Meltem.Tests;
public class GetDeviceInfoNodeTests : MeltemNodeTestBase<GetDeviceInfoNode>
{
    public GetDeviceInfoNodeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public void Should_Get_Device_Info()
    {
        // Arrange
        Sut.Trigger.Value = true;

        // Act
        Sut.Execute();
    }
}
