using Necati_Meral_Yahoo_De.Logic.Meltem;
using Shouldly;
using Xunit.Abstractions;

namespace Necati_Meral_Yahoo_De.LogicNodes.Meltem.Tests;
public class SetVentilationNodeTests : MeltemNodeTestBase<SetVentilationNode>
{
    public SetVentilationNodeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public void Should_Set_Balanced_Ventilation()
    {
        // Arrange
        Sut.Action.Value = SetDeviceAction.SetBalancedVentilationPercent;
        Sut.BalancedVentilation.Value = 100;

        // Act
        Sut.Execute();

        // Assert
        var registers = Server.GetHoldingRegisters((byte)UnitIdentifier);
        registers[MeltemRegisters.InitSetVentilation].ShouldBe((short)768);
        registers[MeltemRegisters.SetVentilation1].ShouldBe((short)-14336);
        registers[MeltemRegisters.ApplyVentilation].ShouldBe((short)0);
    }

    [Fact]
    public void Should_Set_Unbalanced_Ventilation()
    {
        // Arrange
        Sut.Action.Value = SetDeviceAction.SetUnbalancedVentilationPercent;
        Sut.UnbalancedIntakeVentilation.Value = 50;
        Sut.UnbalancedExhaustVentilation.Value = 100;

        // Act
        Sut.Execute();

        // Assert
        var registers = Server.GetHoldingRegisters((byte)UnitIdentifier);
        registers[MeltemRegisters.InitSetVentilation].ShouldBe((short)1024);
        registers[MeltemRegisters.SetVentilation1].ShouldBe((short)25600);
        registers[MeltemRegisters.SetVentilation2].ShouldBe((short)-14336);
        registers[MeltemRegisters.ApplyVentilation].ShouldBe((short)0);
    }
}
