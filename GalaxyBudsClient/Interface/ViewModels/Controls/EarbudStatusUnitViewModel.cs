using System.ComponentModel;
using GalaxyBudsClient.Message;
using GalaxyBudsClient.Message.Decoder;
using GalaxyBudsClient.Model.Constants;
using GalaxyBudsClient.Platform;
using GalaxyBudsClient.Utils;
using GalaxyBudsClient.Utils.Interface;
using ReactiveUI.Fody.Helpers;

namespace GalaxyBudsClient.Interface.ViewModels.Controls;

public class EarbudStatusUnitViewModel : ViewModelBase
{
    [Reactive] public bool IsLeftOnline { set; get; }
    [Reactive] public bool IsRightOnline { set; get; }
    [Reactive] public int LeftBattery { set; get; }
    [Reactive] public int RightBattery { set; get; }
    [Reactive] public int? CaseBattery { set; get; }
    [Reactive] public double LeftVoltage { set; get; }
    [Reactive] public double RightVoltage { set; get; }
    [Reactive] public double LeftCurrent { set; get; }
    [Reactive] public double RightCurrent { set; get; }
    [Reactive] public double LeftTemperature { set; get; }
    [Reactive] public double RightTemperature { set; get; }
    [Reactive] public PlacementStates LeftWearState { set; get; }
    [Reactive] public PlacementStates RightWearState { set; get; }
    
    public EarbudStatusUnitViewModel()
    {
        SppMessageReceiver.Instance.BaseUpdate += OnStatusUpdated;
        SppMessageReceiver.Instance.GetAllDataResponse += OnGetAllDataResponse;
        BluetoothImpl.Instance.PropertyChanged += OnBluetoothPropertyChanged;
        LegacySettings.Instance.PropertyChanged += OnMainSettingsPropertyChanged;
        Loc.LanguageUpdated += LoadFromCache;
        LoadFromCache();
    }

    private void OnBluetoothPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BluetoothImpl.Instance.IsConnected) && 
            DeviceMessageCache.Instance.BasicStatusUpdate != null)
            OnStatusUpdated(null, DeviceMessageCache.Instance.BasicStatusUpdate);
    }

    private void OnMainSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Recalculate temperature values when temperature unit changes
        if (e.PropertyName == nameof(LegacySettings.Instance.TemperatureUnit))
            LoadFromCache();
    }
    
    private void LoadFromCache()
    {
        if (DeviceMessageCache.Instance.BasicStatusUpdate != null)
            OnStatusUpdated(null, DeviceMessageCache.Instance.BasicStatusUpdate);
        if (DeviceMessageCache.Instance.DebugGetAllData != null)
            OnGetAllDataResponse(null, DeviceMessageCache.Instance.DebugGetAllData);
        else
            _ = BluetoothImpl.Instance.SendRequestAsync(MsgIds.DEBUG_GET_ALL_DATA);
    }
    
    private void OnStatusUpdated(object? sender, IBasicStatusUpdate e)
    {
        var connected = BluetoothImpl.Instance.IsConnected;
        LeftBattery = e.BatteryL;
        RightBattery = e.BatteryR;
        CaseBattery = e.BatteryCase is <= 0 or > 100 ? null : e.BatteryCase;
        LeftWearState = e.PlacementL;
        RightWearState = e.PlacementR;
        IsLeftOnline = connected && e.BatteryL > 0 && e.PlacementL != PlacementStates.Disconnected;
        IsRightOnline = connected && e.BatteryR > 0 && e.PlacementR != PlacementStates.Disconnected;
    }
        
    private void OnGetAllDataResponse(object? sender, DebugGetAllDataDecoder e)
    {
        // Buds2 Pro seem to send 0 for all values sometimes. Ignore those updates.
        if(e is { LeftThermistor: <= 0, RightThermistor: <= 0 })
            return;
        
        var useF = LegacySettings.Instance.TemperatureUnit == TemperatureUnits.Fahrenheit;
        LeftVoltage = e.LeftAdcVCell;
        RightVoltage = e.RightAdcVCell;
        LeftCurrent = e.LeftAdcCurrent;
        RightCurrent = e.RightAdcCurrent;
        LeftTemperature = useF ? 9.0 / 5.0 * e.LeftThermistor + 32 : e.LeftThermistor;
        RightTemperature = useF ? 9.0 / 5.0 * e.RightThermistor + 32 :  e.RightThermistor;
    }
}