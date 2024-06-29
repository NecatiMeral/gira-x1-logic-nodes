namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public static class MeltemRegisters
{
    /// <summary>
    /// Fehlermeldung: 0 = Gerät OK; 1 = Fehler
    /// </summary>
    public const int GetErrorMessages = 41016;

    /// <summary>
    /// Frostschutzfunktion: 0 = nicht aktiv; 1 = aktiv
    /// </summary>
    public const int GetFrostProtectionFunction = 41018;

    /// <summary>
    /// Fortlufttemperatur; Float 32 bit, °C
    /// <para>41000, 41001</para>
    /// </summary>
    public const int GetExhaustAirTemperature = 41000;

    /// <summary>
    /// Außenlufttemperatur; Float 32 bit, °C
    /// <para>41002, 41003</para>
    /// </summary>
    public const int GetOutdoorAirTemperature = 41002;

    /// <summary>
    /// Ablufttemperatur; Float 32 bit, °C
    /// <para>41004, 41005</para>
    /// </summary>
    public const int GetInnerExhaustAirTemperature = 41004;

    /// <summary>
    /// Zulufttemperatur
    /// <para>
    /// <strong>Type:</strong> Float 32 bit
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> °C
    /// </para>
    /// </summary>
    public const int GetIntakeAirTemperature = 41009;

    /// <summary>
    /// Feuchte Abluft
    /// <para>
    /// <strong>Type:</strong> UINT16
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> %
    /// </para>
    /// </summary>
    public const int GetExhaustAirHumdity = 41006;

    /// <summary>
    /// Feuchte Zuluft
    /// <para>
    /// <strong>Type:</strong> UINT16
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> %
    /// </para>
    /// </summary>
    public const int GetIntakeAirHumdity = 41011;

    /// <summary>
    /// CO2 Abluft
    /// <para>
    /// <strong>Type:</strong> UINT16
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> ppm
    /// </para>
    /// </summary>
    public const int GetCo2ExhaustAir = 41007;

    /// <summary>
    /// VOC Zuluft
    /// <para>
    /// <strong>Type:</strong> UINT16
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> ppm
    /// </para>
    /// </summary>
    public const int GetVocIntake = 41007;

    /// <summary>
    /// Lüftungsstufe Abluft
    /// <para>
    /// <strong>Type:</strong> UINT8
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> m3/h
    /// </para>
    /// </summary>
    public const int GetExhaustVentilation = 41020;

    /// <summary>
    /// Lüftungsstufe Zuluft
    /// <para>
    /// <strong>Type:</strong> UINT8
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> m3/h
    /// </para>
    /// </summary>
    public const int GetIntakeVentilation = 41021;

    /// <summary>
    /// Luftfilterwechsel-Anzeige: 0 = Luftfilterwechsel-Zeit nicht abgelaufen; 1 = Luftfilterwechsel-Zeit abgelaufen;
    /// <para>
    /// <strong>Type:</strong> UINT8
    /// </para>
    /// </summary>
    public const int GetAirFilterChangeIndicator = 41017;

    /// <summary>
    /// Zeit bis Luftfilterwechsel
    /// <para>
    /// <strong>Type:</strong> UINT16
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> Days
    /// </para>
    /// </summary>
    public const int GetDaysToAirFilterChange = 41027;

    /// <summary>
    /// Betriebsstunden Lüftungsgerät
    /// <para>
    /// <strong>Type:</strong> UINT32
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> Hours
    /// </para>
    /// </summary>
    public const int GetOperatingHoursVentilationDevice = 41030;

    /// <summary>
    /// Betriebsstunden Lüftermotore
    /// <para>
    /// <strong>Type:</strong> UINT32
    /// </para>
    /// <para>
    /// <strong>Unit:</strong> Hours
    /// </para>
    /// </summary>
    public const int GetOperatingHoursFanMotors = 41032;

    public const int InitSetVentilation = 41120;
    public const int SetVentilation1 = 41121;
    public const int SetVentilation2 = 41122;
    public const int ApplyVentilation = 41132;
}
