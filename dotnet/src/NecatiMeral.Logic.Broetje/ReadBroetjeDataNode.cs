using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.Helpers;
using Necati_Meral_Yahoo_De.Logic.Broetje;
using Necati_Meral_Yahoo_De.LogicNodes;
using TcAdsWebService;

namespace NecatiMeral.Logic.Broetje;
public class ReadBroetjeDataNode : LocalizableNode
{
    [Input(DisplayOrder = 1, IsRequired = true)]
    public BoolValueObject Trigger { get; private set; }

    [Input(DisplayOrder = 2, IsDefaultShown = true, IsInput = false)]
    public StringValueObject RemoteAddress { get; }

    [Input(DisplayOrder = 3, IsDefaultShown = true, IsInput = false)]
    public StringValueObject NetId { get; }

    [Input(DisplayOrder = 4, IsDefaultShown = true, IsInput = false)]
    public IntValueObject Port { get; }

    [Input(DisplayOrder = 5, IsDefaultShown = true, IsInput = false)]
    public UIntValueObject IndexGroup { get; }

    [Input(DisplayOrder = 6, IsDefaultShown = true, IsInput = false)]
    public UIntValueObject IndexOffset { get; }

    [Output(DisplayOrder = 1, IsDefaultShown = true)]
    public StringValueObject Data { get; }

    [Output(DisplayOrder = 99, IsDefaultShown = false)]
    public StringValueObject Diagnostics { get; }

    public ReadBroetjeDataNode(INodeContext context)
        : base(context, nameof(ReadBroetjeDataNode))
    {
        context.ThrowIfNull("context");

        Trigger = TypeService.CreateBool(PortTypes.Binary, "Trigger", false);
        RemoteAddress = TypeService.CreateString(PortTypes.String, "RemoteAddress", string.Empty);
        NetId = TypeService.CreateString(PortTypes.String, "NetId", string.Empty);
        Port = TypeService.CreateInt(PortTypes.Integer, "Port", 800);
        IndexGroup = TypeService.CreateUInt(PortTypes.Integer, "IndexGroup", 16416);
        IndexOffset = TypeService.CreateUInt(PortTypes.Integer, "IndexOffset", 0);

        Data = TypeService.CreateString(PortTypes.String, "Data", string.Empty);
        Diagnostics = TypeService.CreateString(PortTypes.String, "Diagnostics", string.Empty);
    }

    public override void Execute()
    {
        if (Trigger.WasSet && (bool)Trigger)
        {
            try
            {
                AsyncHelper.RunSync(ExecuteAsync);
            }
            catch (Exception ex)
            {
                Diagnostics.Value = ex.Message;
            }
        }
    }

    protected virtual async Task ExecuteAsync()
    {
        var service = new TcAdsSyncSoapPortClient(TcAdsSyncSoapPortClient.EndpointConfiguration.TcAdsSyncSoapPort, RemoteAddress);

        var value = await service.ReadAsync(NetId.Value, Port.Value, IndexGroup.Value, IndexOffset.Value, BroetjeConsts.ReadWriteLength);

    }
}
