using EasyModbus;
using LogicModule.ObjectModel.TypeSystem;

namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public class GetDeviceInfoNode : MeltemNodeBase
{
    [Output]
    public BoolValueObject DeviceErrorMessage { get; private set; }
    [Output]
    public BoolValueObject FrostProtectionFunction { get; private set; }
    [Output]
    public DoubleValueObject ExhaustAirTemperature { get; private set; }
    [Output]
    public DoubleValueObject OutdoorAirTemperature { get; private set; }
    [Output]
    public DoubleValueObject InnerExhaustAirTemperature { get; private set; }
    [Output]
    public DoubleValueObject IntakeAirTemperature { get; private set; }
    [Output]
    public UIntValueObject ExhaustAirHumdity { get; private set; }
    [Output]
    public UIntValueObject IntakeAirHumdity { get; private set; }
    [Output]
    public UIntValueObject Co2ExhaustAir { get; private set; }
    [Output]
    public UIntValueObject VocIntake { get; private set; }
    [Output]
    public UIntValueObject IntakeVentilation { get; private set; }
    [Output]
    public UIntValueObject ExhaustVentilation { get; private set; }
    [Output]
    public BoolValueObject AirFilterChangeIndicator { get; private set; }
    [Output]
    public UIntValueObject DaysToAirFilterChange { get; private set; }
    [Output]
    public UIntValueObject OperatingHoursVentilationDevice { get; private set; }
    [Output]
    public UIntValueObject OperatingHoursFanMotors { get; private set; }

    public GetDeviceInfoNode(INodeContext context)
        : base(context, "GetDeviceInfo", true)
    {
        context.ThrowIfNull(nameof(context));

        DeviceErrorMessage = TypeService.CreateBool(PortTypes.Binary, "DeviceErrorMessage");
        FrostProtectionFunction = TypeService.CreateBool(PortTypes.Binary, "FrostProtectionFunction");
        ExhaustAirTemperature = TypeService.CreateDouble(PortTypes.Float, "ExhaustAirTemperature");
        OutdoorAirTemperature = TypeService.CreateDouble(PortTypes.Float, "OutdoorAirTemperature");
        InnerExhaustAirTemperature = TypeService.CreateDouble(PortTypes.Float, "InnerExhaustAirTemperature");
        IntakeAirTemperature = TypeService.CreateDouble(PortTypes.Float, "IntakeAirTemperature");
        ExhaustAirHumdity = TypeService.CreateUInt(PortTypes.DWord, "ExhaustAirHumdity");
        IntakeAirHumdity = TypeService.CreateUInt(PortTypes.DWord, "IntakeAirHumdity");
        Co2ExhaustAir = TypeService.CreateUInt(PortTypes.DWord, "Co2ExhaustAir");
        VocIntake = TypeService.CreateUInt(PortTypes.DWord, "VocIntake");
        IntakeVentilation = TypeService.CreateUInt(PortTypes.DWord, "IntakeVentilation");
        ExhaustVentilation = TypeService.CreateUInt(PortTypes.DWord, "ExhaustVentilation");
        AirFilterChangeIndicator = TypeService.CreateBool(PortTypes.Binary, "AirFilterChangeIndicator");
        DaysToAirFilterChange = TypeService.CreateUInt(PortTypes.DWord, "DaysToAirFilterChange");
        OperatingHoursVentilationDevice = TypeService.CreateUInt(PortTypes.DWord, "OperatingHoursVentilationDevice");
        OperatingHoursFanMotors = TypeService.CreateUInt(PortTypes.DWord, "OperatingHoursFanMotors");
    }

    protected override void ExecuteCore()
    {
        if (!WasTriggered)
        {
            return;
        }

        GetData();
    }

    private void GetData()
    {
        ExecuteWithConnection(client =>
        {
            var deviceError = client.ReadInputRegisters(MeltemRegisters.GetErrorMessages, 1);
            var frostProtection = client.ReadInputRegisters(MeltemRegisters.GetFrostProtectionFunction, 1);
            var exhaustAirTemp = client.ReadInputRegisters(MeltemRegisters.GetExhaustAirTemperature, 2);
            var outdoorAirTemp = client.ReadInputRegisters(MeltemRegisters.GetOutdoorAirTemperature, 2);
            var innerExhaustAirTemp = client.ReadInputRegisters(MeltemRegisters.GetInnerExhaustAirTemperature, 2);
            var intakeAirTemp = client.ReadInputRegisters(MeltemRegisters.GetIntakeAirTemperature, 2);
            var exhaustAirHumidity = client.ReadInputRegisters(MeltemRegisters.GetExhaustAirHumdity, 1);
            var intakeAirHumidity = client.ReadInputRegisters(MeltemRegisters.GetIntakeAirHumdity, 1);
            var co2ExhaustAir = client.ReadInputRegisters(MeltemRegisters.GetCo2ExhaustAir, 1);
            var vocIntake = client.ReadInputRegisters(MeltemRegisters.GetVocIntake, 1);
            var intakeVentilation = client.ReadInputRegisters(MeltemRegisters.GetIntakeVentilation, 1);
            var exhaustVentilation = client.ReadInputRegisters(MeltemRegisters.GetExhaustVentilation, 1);
            var airFilterChangeIndicator = client.ReadInputRegisters(MeltemRegisters.GetAirFilterChangeIndicator, 1);
            var daysToAirFilterChange = client.ReadInputRegisters(MeltemRegisters.GetDaysToAirFilterChange, 1);
            var operatingHourVentilationDevice = client.ReadInputRegisters(MeltemRegisters.GetOperatingHoursVentilationDevice, 2);
            var operatingHoursFanMotors = client.ReadInputRegisters(MeltemRegisters.GetOperatingHoursFanMotors, 2);

            DeviceErrorMessage.Value = deviceError[0] != 0;
            FrostProtectionFunction.Value = frostProtection[0] != 0;
            ExhaustAirTemperature.Value = GetFloatFromRegisters(exhaustAirTemp);
            OutdoorAirTemperature.Value = GetFloatFromRegisters(outdoorAirTemp);
            InnerExhaustAirTemperature.Value = GetFloatFromRegisters(innerExhaustAirTemp);
            IntakeAirTemperature.Value = GetFloatFromRegisters(intakeAirTemp);
            ExhaustAirHumdity.Value = (uint)exhaustAirHumidity[0];
            IntakeAirHumdity.Value = (uint)intakeAirHumidity[0];
            Co2ExhaustAir.Value = (uint)co2ExhaustAir[0];
            VocIntake.Value = (uint)vocIntake[0];
            IntakeVentilation.Value = (uint)intakeVentilation[0];
            ExhaustVentilation.Value = (uint)exhaustVentilation[0];
            AirFilterChangeIndicator.Value = airFilterChangeIndicator[0] != 0;
            DaysToAirFilterChange.Value = (uint)daysToAirFilterChange[0];
            OperatingHoursVentilationDevice.Value = (uint)operatingHourVentilationDevice[0];
            OperatingHoursFanMotors.Value = (uint)operatingHoursFanMotors[0];
        });
    }

    private static float GetFloatFromRegisters(int[] registers)
    {
        if (registers[0] == 0)
        {
            // Not all devices have sensors for all values; in this case, the first register is 0.
            return 0;
        }

        return ModbusClient.ConvertRegistersToFloat(registers, ModbusClient.RegisterOrder.LowHigh);
    }
}
