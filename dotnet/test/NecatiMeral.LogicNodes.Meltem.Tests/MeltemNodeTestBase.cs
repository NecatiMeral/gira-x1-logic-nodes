using System.Net;
using FluentModbus;
using Necati_Meral_Yahoo_De.Logic.Meltem;
using Necati_Meral_Yahoo_De.LogicNodes.Tests;
using Necati_Meral_Yahoo_De.LogicNodes.Tests.Core;
using Xunit.Abstractions;

namespace Necati_Meral_Yahoo_De.LogicNodes.Meltem.Tests;
public class MeltemNodeTestBase<TNode> : NodeTestBase<TNode>
    where TNode : MeltemNodeBase
{
    protected int Port { get; }
    protected ModbusTcpServer Server { get; }

    protected int UnitIdentifier { get; } = 5;
    public ModbusTcpClient Client { get; }

    public MeltemNodeTestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        Port = PortManager.GetAvailablePort(500);
        Server = new ModbusTcpServer(Logger);
        Server.AddUnit((byte)UnitIdentifier);
        Server.Start(new IPEndPoint(IPAddress.Any, Port));

        Client = new ModbusTcpClient();
        Client.Connect(new IPEndPoint(IPAddress.Loopback, Port), ModbusEndianness.BigEndian);

        ConfigureMeltemNode(Sut);
    }

    protected virtual void ConfigureMeltemNode(MeltemNodeBase node)
    {
        node.UnitId.Value = UnitIdentifier;
        node.IPAddress.Value = "localhost";
        node.Port.Value = Port;
    }
}
