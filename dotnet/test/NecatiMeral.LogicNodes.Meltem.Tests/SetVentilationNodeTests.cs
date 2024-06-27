using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using FluentModbus;
using Necati_Meral_Yahoo_De.Logic.Meltem;
using Necati_Meral_Yahoo_De.LogicNodes.Tests;
using Shouldly;
using Xunit.Abstractions;

namespace NecatiMeral.LogicNodes.Meltem.Tests;
public class SetVentilationNodeTests : NodeTestBase<SetVentilationNode>
{
    private readonly int _port;
    private readonly ModbusTcpServer _server;

    protected int UnitIdentifier { get; } = 5;

    public SetVentilationNodeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        _port = GetAvailablePort(500);
        _server = new ModbusTcpServer(Logger);
        _server.AddUnit((byte)UnitIdentifier);
        _server.Start(new IPEndPoint(IPAddress.Any, _port));

        Sut.UnitIdentifier.Value = UnitIdentifier;
        Sut.IPAddress.Value = "localhost";
        Sut.Port.Value = _port;
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
        var registers = _server.GetHoldingRegisters((byte)UnitIdentifier);
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
        var registers = _server.GetHoldingRegisters((byte)UnitIdentifier);
        registers[MeltemRegisters.InitSetVentilation].ShouldBe((short)1024);
        registers[MeltemRegisters.SetVentilation1].ShouldBe((short)25600);
        registers[MeltemRegisters.SetVentilation2].ShouldBe((short)-14336);
        registers[MeltemRegisters.ApplyVentilation].ShouldBe((short)0);
    }

    /// <summary>
    /// checks for used ports and retrieves the first free port
    /// </summary>
    /// <returns>the free port or 0 if it did not find a free port</returns>
    public static int GetAvailablePort(int startingPort)
    {
        IPEndPoint[] endPoints;
        var portArray = new List<int>();

        var properties = IPGlobalProperties.GetIPGlobalProperties();

        // getting active connections
        var connections = properties.GetActiveTcpConnections();
        portArray.AddRange(from n in connections
                           where n.LocalEndPoint.Port >= startingPort
                           select n.LocalEndPoint.Port);

        // getting active tcp listners - WCF service listening in tcp
        endPoints = properties.GetActiveTcpListeners();
        portArray.AddRange(from n in endPoints
                           where n.Port >= startingPort
                           select n.Port);

        // getting active udp listeners
        endPoints = properties.GetActiveUdpListeners();
        portArray.AddRange(from n in endPoints
                           where n.Port >= startingPort
                           select n.Port);

        portArray.Sort();

        for (var i = startingPort; i < UInt16.MaxValue; i++)
            if (!portArray.Contains(i))
                return i;

        return 0;
    }
}
