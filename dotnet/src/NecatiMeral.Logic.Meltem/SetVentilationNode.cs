using LogicModule.ObjectModel.TypeSystem;

namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public class SetVentilationNode : MeltemNodeBase
{
    [Parameter(DisplayOrder = 4, IsDefaultShown = true)]
    public EnumValueObject Action { get; }

    [Input(DisplayOrder = 5, IsDefaultShown = true, IsInput = false)]
    public IntValueObject BalancedVentilation { get; private set; }

    [Input(DisplayOrder = 6, IsDefaultShown = true, IsInput = false)]
    public IntValueObject UnbalancedIntakeVentilation { get; private set; }

    [Input(DisplayOrder = 7, IsDefaultShown = true, IsInput = false)]
    public IntValueObject UnbalancedExhaustVentilation { get; private set; }

    public SetVentilationNode(INodeContext context)
        : base(context, "SetVentilation")
    {
        context.ThrowIfNull(nameof(context));

        Action = TypeService.CreateEnum(nameof(SetDeviceAction), "Action", SetDeviceAction.Values, SetDeviceAction.SetBalancedVentilationPercent);
        Action.ValueSet += OnActionValueSet;
        InitializeActionInputs(SetDeviceAction.SetBalancedVentilationPercent);
    }


    protected override void ExecuteCore()
    {
        var action = Action.Value.ToString();
        switch (action)
        {
            case SetDeviceAction.SetBalancedVentilationPercent:
                SetBalancedVentilationPercent();
                break;
            case SetDeviceAction.SetUnbalancedVentilationPercent:
                SetUnbalancedVentilationPercent();
                break;
        }
    }

    private void SetBalancedVentilationPercent()
    {
        if (!BalancedVentilation.WasSet)
        {
            return;
        }

        ExecuteWithConnection(client =>
        {
            var ventilationValue = BalancedVentilation.Value * 2;

            client.WriteSingleRegister(MeltemRegisters.InitSetVentilation, 3);
            client.WriteSingleRegister(MeltemRegisters.SetVentilation1, ventilationValue);
            client.WriteSingleRegister(MeltemRegisters.ApplyVentilation, 0);
        });
    }

    private void SetUnbalancedVentilationPercent()
    {
        if (!UnbalancedIntakeVentilation.WasSet && !UnbalancedExhaustVentilation.WasSet)
        {
            return;
        }

        ExecuteWithConnection(client =>
        {
            var intakeValue = UnbalancedIntakeVentilation.Value * 2;
            var exhaustValue = UnbalancedExhaustVentilation.Value * 2;

            client.WriteSingleRegister(MeltemRegisters.InitSetVentilation, 4);
            client.WriteSingleRegister(MeltemRegisters.SetVentilation1, intakeValue);
            client.WriteSingleRegister(MeltemRegisters.SetVentilation2, exhaustValue);
            client.WriteSingleRegister(MeltemRegisters.ApplyVentilation, 0);
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
        if (action == SetDeviceAction.SetBalancedVentilationPercent)
        {
            BalancedVentilation = CreateVentilationItem("BalancedVentilation");
            UnbalancedIntakeVentilation = null;
            UnbalancedExhaustVentilation = null;
        }
        else if (action == SetDeviceAction.SetUnbalancedVentilationPercent)
        {
            BalancedVentilation = null;
            UnbalancedIntakeVentilation = CreateVentilationItem("UnbalancedIntakeVentilation");
            UnbalancedExhaustVentilation = CreateVentilationItem("UnbalancedExhaustVentilation");
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
