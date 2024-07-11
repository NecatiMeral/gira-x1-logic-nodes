using LogicModule.ObjectModel.TypeSystem;

namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public class SetVentilationNode : MeltemNodeBase
{
    [Parameter(DisplayOrder = 5, IsDefaultShown = true)]
    public EnumValueObject Action { get; }

    [Input(DisplayOrder = 6, IsDefaultShown = true, IsInput = false)]
    public IntValueObject BalancedVentilation { get; private set; }

    [Input(DisplayOrder = 7, IsDefaultShown = true, IsInput = false)]
    public IntValueObject UnbalancedIntakeVentilation { get; private set; }

    [Input(DisplayOrder = 8, IsDefaultShown = true, IsInput = false)]
    public IntValueObject UnbalancedExhaustVentilation { get; private set; }

    [Input(DisplayOrder = 9, IsDefaultShown = true, IsInput = false)]
    public EnumValueObject BalancedVentilationLevel { get; private set; }

    [Input(DisplayOrder = 10, IsDefaultShown = true, IsInput = false)]
    public IntValueObject BalancedVentilationLevelNumeric { get; private set; }

    [Output]
    public AnyValueObject Output { get; private set; }

    public SetVentilationNode(INodeContext context)
        : base(context, "SetVentilation")
    {
        context.ThrowIfNull(nameof(context));

        Action = TypeService.CreateEnum(
            nameof(SetDeviceAction),
            "Action",
            SetDeviceAction.Values,
            SetDeviceAction.SetBalancedVentilationPercent
        );

        Action.ValueSet += OnActionValueSet;
        Output = TypeService.CreateAny(PortTypes.Any, "Output");
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
            case SetDeviceAction.SetBalancedVentilationLevel:
                SetBalancedVentilationLevel();
                break;
            case SetDeviceAction.SetUnbalancedVentilationPercent:
                SetUnbalancedVentilationPercent();
                break;
            case SetDeviceAction.SetBalancedVentilationNumeric:
                SetBalancedVentilationLevelNumeric();
                break;
        }
    }

    private void SetBalancedVentilationPercent()
    {
        if (BalancedVentilation.WasSet)
        {
            SetVentilationPercent(BalancedVentilation.Value);
            Output.Value = BalancedVentilation.Value;
        }
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

        Output.Value = UnbalancedIntakeVentilation.Value + UnbalancedExhaustVentilation.Value;
    }

    private void SetBalancedVentilationLevel()
    {
        if (BalancedVentilationLevel.WasSet)
        {
            SetVentilationPercent(
                VentilationPreset.GetVentilationPercent(BalancedVentilationLevel.Value)
            );
            Output.Value = BalancedVentilationLevel.Value;
        }
    }

    private void SetBalancedVentilationLevelNumeric()
    {
        if (BalancedVentilationLevelNumeric.WasSet)
        {
            SetVentilationPercent(
                VentilationPreset.GetVentilationPercent(BalancedVentilationLevelNumeric.Value)
            );
            Output.Value = BalancedVentilationLevelNumeric.Value;
        }
    }

    private void SetVentilationPercent(int percent)
    {
        ExecuteWithConnection(client =>
        {
            var ventilationValue = percent * 2;

            client.WriteSingleRegister(MeltemRegisters.InitSetVentilation, 3);
            client.WriteSingleRegister(MeltemRegisters.SetVentilation1, ventilationValue);
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
            BalancedVentilationLevel = null;
            BalancedVentilationLevelNumeric = null;
        }
        else if (action == SetDeviceAction.SetUnbalancedVentilationPercent)
        {
            BalancedVentilation = null;
            UnbalancedIntakeVentilation = CreateVentilationItem("UnbalancedIntakeVentilation");
            UnbalancedExhaustVentilation = CreateVentilationItem("UnbalancedExhaustVentilation");
            BalancedVentilationLevel = null;
            BalancedVentilationLevelNumeric = null;
        }
        else if (action == SetDeviceAction.SetBalancedVentilationLevel)
        {
            BalancedVentilation = null;
            UnbalancedIntakeVentilation = null;
            UnbalancedExhaustVentilation = null;
            BalancedVentilationLevel = TypeService.CreateEnum(
                nameof(VentilationPreset),
                "BalancedVentilationLevel",
                VentilationPreset.Values,
                VentilationPreset.NominalVentilation
            );
            BalancedVentilationLevelNumeric = null;
        }
        else if (action == SetDeviceAction.SetBalancedVentilationNumeric)
        {
            BalancedVentilation = null;
            UnbalancedIntakeVentilation = null;
            UnbalancedExhaustVentilation = null;
            BalancedVentilationLevel = null;
            BalancedVentilationLevelNumeric = CreateVentilationItem("BalancedVentilationLevelNumeric");
            BalancedVentilationLevelNumeric.MinValue = 0;
            BalancedVentilationLevelNumeric.MaxValue = VentilationPreset.Values.Length;
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
