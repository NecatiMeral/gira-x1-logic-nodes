using EasyModbus;
using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.LogicNodes;

namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public abstract class MeltemNodeBase : LocalizableNode
{
    [Input(DisplayOrder = 1, IsDefaultShown = true, IsInput = false)]
    public BoolValueObject Trigger { get; }

    [Input(DisplayOrder = 2, IsDefaultShown = true, IsInput = false)]
    public StringValueObject IPAddress { get; }

    [Input(DisplayOrder = 3, IsDefaultShown = true, IsInput = false)]
    public IntValueObject Port { get; }

    [Input(DisplayOrder = 4, IsDefaultShown = true, IsInput = false)]
    public IntValueObject UnitId { get; }

    protected bool WasTriggered => Trigger != null && Trigger.HasValue && Trigger.WasSet && Trigger.Value;

    private ModbusClient _client;

    public MeltemNodeBase(INodeContext context, string nodeTypeName, bool hasTrigger = false)
        : base(context, nodeTypeName)
    {
        context.ThrowIfNull(nameof(context));

        if (hasTrigger)
        {
            Trigger = TypeService.CreateBool("BINARY", "Trigger");
        }

        IPAddress = TypeService.CreateString("STRING", "IPAddress", string.Empty);
        Port = TypeService.CreateInt("INTEGER", "Port", 502);
        Port.MinValue = 0;

        UnitId = TypeService.CreateInt("INTEGER", "UnitId", 1);
        UnitId.MinValue = 0;
        UnitId.MaxValue = 255;

        _client = new ModbusClient
        {
            ConnectionTimeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds
        };
    }

    public override void Execute()
    {
        if (!IPAddress.HasValue || !Port.HasValue || !UnitId.HasValue)
        {
            return;
        }

        ExecuteCore();
    }

    protected abstract void ExecuteCore();

    protected void ExecuteWithConnection(Action<ModbusClient> action)
    {
        _client.Connect(IPAddress.Value, Port.Value);
        _client.UnitIdentifier = (byte)UnitId.Value;
        if (!_client.Connected)
        {
            return;
        }

        try
        {
            action(_client);
        }
        finally
        {
            _client.Disconnect();
        }
    }
}
