# .NET MAUI Android Setup Guide

Getting Android targets (emulator + real device) working with .NET MAUI in VS
Code.  
**Assumes:** Android Studio is installed, USB debugging is enabled on your real
device.

---

## Table of Contents

- [What MAUI Needs](#what-maui-needs)
- [macOS Setup](#macos-setup)
  - [1. Configure Environment Variables](#macos-1-configure-environment-variables)
  - [2. Install SDK Components via Android Studio](#macos-2-install-sdk-components-via-android-studio)
  - [3. Emulator Target](#macos-3-emulator-target)
  - [4. Real Device via USB](#macos-4-real-device-via-usb)
- [Windows Setup](#windows-setup)
  - [1. Configure Environment Variables](#windows-1-configure-environment-variables)
  - [2. Install SDK Components via Android Studio](#windows-2-install-sdk-components-via-android-studio)
  - [3. Emulator Target](#windows-3-emulator-target)
  - [4. Real Device via USB](#windows-4-real-device-via-usb)
- [Verify Everything Works](#verify-everything-works)
- [Troubleshooting](#troubleshooting)

---

## What MAUI Needs

Regardless of platform, MAUI requires the following to build and deploy for
Android:

| Requirement                    | Purpose                                         |
| ------------------------------ | ----------------------------------------------- |
| `ANDROID_HOME` env variable    | Tells MAUI where the Android SDK lives          |
| `platform-tools` in PATH       | Gives access to `adb` for deploying to devices  |
| Android SDK Platform (API 35+) | The Android API level to compile against        |
| Android SDK Build-Tools        | Compiles and packages the Android APK           |
| Android SDK Command-line Tools | Required by MAUI's build pipeline               |
| Licenses accepted              | SDK won't work until all licenses are agreed to |

---

## macOS Setup

### macOS 1. Configure Environment Variables

Open your shell config file:

```bash
nano ~/.zshrc
```

Add these lines at the bottom:

```bash
export ANDROID_HOME=$HOME/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/platform-tools
export PATH=$PATH:$ANDROID_HOME/emulator
export PATH=$PATH:$ANDROID_HOME/cmdline-tools/latest/bin
```

Apply the changes:

```bash
source ~/.zshrc
```

> **Then restart VS Code completely** — environment variable changes are not
> picked up by a running VS Code instance.

---

### macOS 2. Install SDK Components via Android Studio

1. Open **Android Studio**
2. Go to **Settings → Languages & Frameworks → Android SDK**
3. **SDK Platforms tab** — check and install:
   - Android API 35 (or your target API level)
4. **SDK Tools tab** — check and install:
   - Android SDK Build-Tools (latest)
   - Android SDK Command-line Tools (latest)
   - Android SDK Platform-Tools
   - Android Emulator _(if using emulator)_
5. Click **Apply** — Android Studio will accept all licenses automatically
   during install

---

### macOS 3. Emulator Target

**Start the emulator first, then open VS Code's device picker.**

1. In Android Studio, open **Device Manager** (right panel or View → Tool
   Windows → Device Manager)
2. Start your AVD (Android Virtual Device)
3. Wait for it to fully boot to the home screen
4. In VS Code, click the target device in the status bar — the emulator should
   now appear

If it still doesn't appear:

```bash
# Confirm ADB sees it
adb devices
# Expected: emulator-5554   device

# If nothing shows, restart the ADB server
adb kill-server && adb start-server
adb devices
```

Then restart VS Code.

---

### macOS 4. Real Device via USB

**On your Android device:**

1. Connect via USB cable _(must be a data cable, not charge-only)_
2. Pull down the notification shade → tap the USB connection notification
3. Change mode from **Charging** to **File Transfer (MTP)**
4. A prompt will appear on the phone: **"Allow USB debugging?"** → tap **Always
   allow from this computer**

**On your Mac:**

```bash
# Verify the device is visible
adb devices
# Expected: R5CT21XXXXX    device
```

| Output         | Meaning                                             |
| -------------- | --------------------------------------------------- |
| `device`       | Connected and trusted — ready to use                |
| `unauthorized` | Tap "Allow" on the phone — you dismissed the prompt |
| `offline`      | Try a different cable or USB port                   |
| _(empty)_      | ADB path issue or cable has no data lines           |

Once `adb devices` shows `device`, restart VS Code and select the device from
the target picker.

---

## Windows Setup

### Windows 1. Configure Environment Variables

1. Press `Win + S`, search **"Edit the system environment variables"**
2. Click **Environment Variables**
3. Under **System variables**, click **New**:
   - Variable name: `ANDROID_HOME`
   - Variable value: `C:\Users\<YourUsername>\AppData\Local\Android\Sdk`
4. Find the **Path** variable under System variables → click **Edit → New**, and
   add:
   - `%ANDROID_HOME%\platform-tools`
   - `%ANDROID_HOME%\emulator`
   - `%ANDROID_HOME%\cmdline-tools\latest\bin`
5. Click **OK** on all dialogs

> **Then restart VS Code completely** — environment variables set this way only
> apply to newly launched processes.

Verify in a new terminal:

```cmd
echo %ANDROID_HOME%
adb --version
```

---

### Windows 2. Install SDK Components via Android Studio

Same as macOS — Android Studio's SDK Manager is the same across platforms:

1. Open **Android Studio**
2. Go to **Settings → Languages & Frameworks → Android SDK**
3. **SDK Platforms tab** — install Android API 35 (or your target)
4. **SDK Tools tab** — install:
   - Android SDK Build-Tools (latest)
   - Android SDK Command-line Tools (latest)
   - Android SDK Platform-Tools
   - Android Emulator _(if using emulator)_
   - **Google USB Driver** _(needed for real devices on Windows)_
5. Click **Apply** — licenses are accepted automatically

---

### Windows 3. Emulator Target

**Hardware acceleration must be enabled** for Android emulators to run at usable
speed on Windows.

Check which virtualisation to use:

| Your CPU | Accelerator                         |
| -------- | ----------------------------------- |
| Intel    | Hyper-V (recommended) or Intel HAXM |
| AMD      | Hyper-V                             |

**Enable Hyper-V:**

1. Press `Win + S` → search **"Turn Windows features on or off"**
2. Check **Hyper-V** (all sub-items) → click OK → restart

**Then run the emulator:**

1. In Android Studio, open **Device Manager** → start your AVD
2. Wait for it to fully boot
3. Open a new terminal and verify:

```cmd
adb devices
:: Expected: emulator-5554   device
```

4. Restart VS Code → select the emulator from the device picker

---

### Windows 4. Real Device via USB

**Windows requires a USB driver** — unlike macOS, it does not detect Android
devices automatically.

**Install the driver (pick one):**

- **Recommended (works for most brands):**
  [Universal ADB Driver](https://github.com/koush/UniversalAdbDriver) — install
  and reboot
- **Brand-specific:**
  - Samsung → install Samsung Smart Switch or Samsung USB Drivers
  - Google Pixel → Google USB Driver (installed in Step 2 via SDK Tools)
  - Xiaomi/OnePlus/others → download from manufacturer's website

**After driver install:**

1. Replug the USB cable
2. On the phone: change USB mode to **File Transfer (MTP)**
3. Tap **"Always allow from this computer"** on the USB debugging prompt

```cmd
adb devices
:: Expected: R5CT21XXXXX    device
```

| Output         | Meaning                                 |
| -------------- | --------------------------------------- |
| `device`       | Ready                                   |
| `unauthorized` | Tap Allow on the phone                  |
| _(empty)_      | Driver not installed or wrong USB cable |

Once `device` shows, restart VS Code and select from the device picker.

---

## Verify Everything Works

Run all three checks. If each returns output without errors, MAUI is fully
configured:

```bash
# 1. ADB is reachable
adb --version

# 2. SDK manager is reachable and SDK path is correct
sdkmanager --list

# 3. Target device/emulator is connected
adb devices
```

---

## Troubleshooting

| Symptom                                                            | Fix                                                                                                            |
| ------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------- |
| Emulator/device visible in `adb devices` but not in VS Code picker | Restart VS Code completely (not just reload window)                                                            |
| `adb: command not found`                                           | `platform-tools` not in PATH — recheck env variables and restart terminal                                      |
| `sdkmanager: command not found`                                    | `cmdline-tools/latest/bin` not in PATH                                                                         |
| "Android SDK missing components" error in VS Code                  | Open Android Studio SDK Manager and install missing components listed in Step 2                                |
| Device shows `unauthorized`                                        | On phone: Settings → Developer Options → **Revoke USB debugging authorizations**, then reconnect and tap Allow |
| Emulator extremely slow on Windows                                 | Hyper-V not enabled, or running on a VM that blocks nested virtualisation                                      |
| ADB shows device but deployment fails                              | Run `adb kill-server && adb start-server`, then retry                                                          |
| VS Code showing wrong SDK path                                     | Click the "Android SDK missing" notification → manually point it to `$ANDROID_HOME`                            |
