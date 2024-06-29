using LogicModule.ObjectModel.TypeSystem;

namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public class GetVentilationNode : MeltemNodeBase
{
    [Parameter(DisplayOrder = 5, IsDefaultShown = true)]
    public EnumValueObject Action { get; }

    [Output]
    public IntValueObject VentilationPercentage { get; private set; }

    [Output]
    public IntValueObject VentilationLevelNumeric { get; private set; }

    private const int _maxVentilationValue = 200;

    public GetVentilationNode(INodeContext context)
        : base(context, "GetVentilation", true)
    {
        context.ThrowIfNull(nameof(context));

        Action = TypeService.CreateEnum(nameof(GetDeviceAction), "Action", GetDeviceAction.Values, GetDeviceAction.GetVentilationPercent);
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
            case GetDeviceAction.GetVentilationPercent:
                GetVentilationPercent();
                break;
            case GetDeviceAction.GetVentilationLevelNumeric:
                GetVentilationLevelNumeric();
                break;
        }
    }

    private void GetVentilationLevelNumeric()
    {
        var percent = GetVentilationPercentCore();

        VentilationLevelNumeric.Value = VentilationPreset.GetNearestVentilationLevel(percent);
    }

    private void GetVentilationPercent()
    {
        VentilationPercentage.Value = GetVentilationPercentCore();
    }

    private int GetVentilationPercentCore()
    {
        var percentage = 0;
        ExecuteWithConnection(client =>
        {
            var intake = client.ReadHoldingRegisters(MeltemRegisters.GetIntakeVentilation, 1);
            var exhaust = client.ReadHoldingRegisters(MeltemRegisters.GetExhaustVentilation, 1);

            var total = intake[0] + exhaust[0];

            percentage = total * 100 / _maxVentilationValue;
        });
        return percentage;
    }

    private void OnActionValueSet(object sender, ValueChangedEventArgs e)
    {
        if (!Equals(e.NewValue, e.OldValue))
        {
            InitializeActionInputs(e.NewValue.ToString());
        }
    }

    private void InitializeActionInputs(string action)
    {
        if (action == GetDeviceAction.GetVentilationPercent)
        {
            VentilationPercentage = CreateVentilationItem("VentilationPercentage");
            VentilationLevelNumeric = null;
        }
        else if (action == GetDeviceAction.GetVentilationLevelNumeric)
        {
            VentilationPercentage = null;
            VentilationLevelNumeric = CreateVentilationItem("VentilationLevelNumeric");
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
