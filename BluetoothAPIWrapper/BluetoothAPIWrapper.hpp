#pragma once

extern "C"
{

#define DllExport __declspec(dllexport)

	DllExport HBLUETOOTH_RADIO_FIND APIENTRY
		MyBluetoothFindFirstRadio(const BLUETOOTH_FIND_RADIO_PARAMS * pbtfrp,
			__out HANDLE *phRadio);
	DllExport BOOL
		MyBluetoothFindNextRadio(__in HBLUETOOTH_RADIO_FIND hFind,
			__out HANDLE *phRadio);
	DllExport BOOL
		MyBluetoothFindRadioClose(__in HBLUETOOTH_RADIO_FIND hfind);
	DllExport DWORD
		MyBluetoothGetRadioInfo(__in HANDLE hRadio,
			__inout PBLUETOOTH_RADIO_INFO pRadioInfo);

	DllExport HBLUETOOTH_DEVICE_FIND
		MyBluetoothFindFirstDevice(const BLUETOOTH_DEVICE_SEARCH_PARAMS * pbtsp,
			__inout BLUETOOTH_DEVICE_INFO * pbtdi);
	DllExport BOOL
		MyBluetoothFindNextDevice(__in HBLUETOOTH_DEVICE_FIND hFind,
			__inout BLUETOOTH_DEVICE_INFO * pbtdi);
	DllExport BOOL
		MyBluetoothFindDeviceClose(__in HBLUETOOTH_DEVICE_FIND hFind);
	DllExport DWORD
		MyBluetoothGetDeviceInfo(__in_opt HANDLE hRadio,
			__inout BLUETOOTH_DEVICE_INFO * pbtdi);
	DllExport DWORD
		MyBluetoothUpdateDeviceRecord(const BLUETOOTH_DEVICE_INFO *pbtdi);
	DllExport DWORD
		MyBluetoothRemoveDevice(const BLUETOOTH_ADDRESS * pAddress);

	DllExport DWORD
		MyBluetoothSetServiceState(__in_opt HANDLE hRadio,
			const BLUETOOTH_DEVICE_INFO * pbtdi,
			const GUID * pGuidService,
			DWORD dwServiceFlags);

	DllExport DWORD
		MyBluetoothAuthenticateDevice(HWND hwnd,
			__in_opt HANDLE hRadio,
			__inout BLUETOOTH_DEVICE_INFO *pbtdi,
			const PWCHAR pszPasskey, ULONG ulPasskeyLength);

	DllExport DWORD
		MyBluetoothEnumerateInstalledServices(__in_opt HANDLE hRadio, __inout BLUETOOTH_DEVICE_INFO *pbtdi,
			DWORD *pcServices, GUID *pGuidServices);

	DllExport BOOL
		MyCloseHandle(HANDLE hObject);

}

