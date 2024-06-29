using Necati_Meral_Yahoo_De.Logic.Meltem;
using Shouldly;
using Xunit.Abstractions;

namespace Necati_Meral_Yahoo_De.LogicNodes.Meltem.Tests;
public class GetVentilationNodeTests : MeltemNodeTestBase<GetVentilationNode>
{
    public GetVentilationNodeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public void Should_Get_Balanced_Ventilation()
    {
        // Arrange
        Client.WriteSingleRegister(UnitIdentifier, MeltemRegisters.GetIntakeVentilation, (ushort)100);
        Client.WriteSingleRegister(UnitIdentifier, MeltemRegisters.GetExhaustVentilation, (ushort)100);

        Sut.Action.Value = GetDeviceAction.GetVentilationPercent;
        Sut.Trigger.Value = true;

        // Act
        Sut.Execute();

        // Assert
        Sut.VentilationPercentage.Value.ShouldBe(100);
    }

    [Fact]
    public void Should_Get_Unbalanced_Ventilation()
    {
        // Arrange
        Client.WriteSingleRegister(UnitIdentifier, MeltemRegisters.GetIntakeVentilation, (ushort)100);
        Client.WriteSingleRegister(UnitIdentifier, MeltemRegisters.GetExhaustVentilation, (ushort)0);

        Sut.Action.Value = GetDeviceAction.GetVentilationPercent;
        Sut.Trigger.Value = true;

        // Act
        Sut.Execute();

        // Assert
        Sut.VentilationPercentage.Value.ShouldBe(50);
    }
}
