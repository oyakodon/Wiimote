#include <windows.h>
#include <Bthsdpdef.h>
#include <BluetoothAPIs.h>
#include "BluetoothAPIWrapper.hpp"

HBLUETOOTH_RADIO_FIND APIENTRY MyBluetoothFindFirstRadio(const BLUETOOTH_FIND_RADIO_PARAMS * pbtfrp,
	__out HANDLE * phRadio) {
	return BluetoothFindFirstRadio(pbtfrp, phRadio);
}

BOOL MyBluetoothFindNextRadio(__in HBLUETOOTH_RADIO_FIND hFind,
	__out HANDLE *phRadio) {
	return BluetoothFindNextRadio(hFind, phRadio);
}
BOOL MyBluetoothFindRadioClose(__in HBLUETOOTH_RADIO_FIND hFind) {
	return BluetoothFindRadioClose(hFind);
}
DWORD MyBluetoothGetRadioInfo(__in HANDLE hRadio,
	__inout PBLUETOOTH_RADIO_INFO pRadioInfo) {
	return BluetoothGetRadioInfo(hRadio, pRadioInfo);
}
HBLUETOOTH_DEVICE_FIND MyBluetoothFindFirstDevice(const BLUETOOTH_DEVICE_SEARCH_PARAMS * pbtsp,
	__inout BLUETOOTH_DEVICE_INFO * pbtdi) {
	return BluetoothFindFirstDevice(pbtsp, pbtdi);
}
BOOL MyBluetoothFindNextDevice(__in HBLUETOOTH_DEVICE_FIND hFind,
	__inout BLUETOOTH_DEVICE_INFO * pbtdi) {
	return BluetoothFindNextDevice(hFind, pbtdi);
}
BOOL MyBluetoothFindDeviceClose(__in HBLUETOOTH_DEVICE_FIND hFind) {
	return BluetoothFindDeviceClose(hFind);
}
DWORD MyBluetoothGetDeviceInfo(__in_opt HANDLE hRadio,
	__inout BLUETOOTH_DEVICE_INFO * pbtdi) {
	return BluetoothGetDeviceInfo(hRadio, pbtdi);
}

DWORD MyBluetoothUpdateDeviceRecord(const BLUETOOTH_DEVICE_INFO *pbtdi) {
	return BluetoothUpdateDeviceRecord(pbtdi);
}

DWORD MyBluetoothRemoveDevice(const BLUETOOTH_ADDRESS * pAddress) {
	return BluetoothRemoveDevice(pAddress);
}
DWORD MyBluetoothSetServiceState(__in_opt HANDLE hRadio,
	const BLUETOOTH_DEVICE_INFO * pbtdi,
	const GUID * pGuidService,
	DWORD dwServiceFlags) {
	return BluetoothSetServiceState(hRadio, pbtdi, pGuidService, dwServiceFlags);
}


DWORD MyBluetoothAuthenticateDevice(HWND hwnd,
	__in_opt HANDLE hRadio,
	__inout BLUETOOTH_DEVICE_INFO *pbtdi,
	const PWCHAR pszPasskey, ULONG ulPasskeyLength) {
	return BluetoothAuthenticateDevice(hwnd, hRadio, pbtdi, pszPasskey, ulPasskeyLength);
}


DWORD MyBluetoothEnumerateInstalledServices(__in_opt HANDLE hRadio, __inout BLUETOOTH_DEVICE_INFO *pbtdi,
	DWORD *pcServices, GUID *pGuidServices) {
	return BluetoothEnumerateInstalledServices(hRadio, pbtdi, pcServices, pGuidServices);
}

BOOL MyCloseHandle(HANDLE hObject) {
	return CloseHandle(hObject);
}
