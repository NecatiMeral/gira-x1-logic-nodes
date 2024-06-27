using LogicModule.ObjectModel.TypeSystem;

namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public class GetVentilationNode : MeltemNodeBase
{
    [Parameter(DisplayOrder = 4, IsDefaultShown = true)]
    public EnumValueObject Action { get; }

    [Output]
    public IntValueObject VentilationPercentage { get; private set; }

    private const int _maxVentilationValue = 200;

    public GetVentilationNode(INodeContext context)
        : base(context, "GetVentilation", true)
    {
        context.ThrowIfNull(nameof(context));

        Action = TypeService.CreateEnum(nameof(SetDeviceAction), "Action", GetDeviceAction.Values, GetDeviceAction.GetVentilationPercent);
        Action.ValueSet += OnActionValueSet;
        InitializeActionInputs(GetDeviceAction.GetVentilationPercent);
    }

    protected override void ExecuteCore()
    {
        if (!WasTriggered)
        {
            return;
        }

        var action = Action.Value.ToString();
        switch (action)
        {
            case SetDeviceAction.SetBalancedVentilationPercent:
                GetVentilationPercent();
                break;
        }
    }

    private void GetVentilationPercent()
    {
        ExecuteWithConnection(client =>
        {
            var intake = client.ReadHoldingRegisters(MeltemRegisters.GetIntakeVentilation, 1);
            var exhaust = client.ReadHoldingRegisters(MeltemRegisters.GetExhaustVentilation, 1);

            var total = intake[0] + exhaust[0];

            VentilationPercentage.Value = total * 100 / _maxVentilationValue;
        });
    }

    private void OnActionValueSet(object sender, ValueChangedEventArgs e)
    {
        if (Equals(e.NewValue, e.OldValue))
        {
            return;
        }

        InitializeActionInputs(e.NewValue.ToString());
    }

    private void InitializeActionInputs(string action)
    {
        if (action == GetDeviceAction.GetVentilationPercent)
        {
            VentilationPercentage = CreateVentilationItem("VentilationPercentage");
        }
    }

    private IntValueObject CreateVentilationItem(string name)
    {
        var item = TypeService.CreateInt("INTEGER", name, 0);
        item.MinValue = 0;
        item.MaxValue = 100;

        return item;
    }
}
