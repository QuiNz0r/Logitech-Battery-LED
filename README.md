<p align="center">
  <img  src="https://i.imgur.com/YygJvlX.png">
</p>

# Logitech-Battery-LED
Small .NET C# app to change RGB LED on Logitech Wireless Mice within Logitech G Hub according to battery charge

# Dependencies
- Memory.dll
- LogitechLedEnginesWrapper.dll (64bit)

# Requirements
- .NET 4.7.2+
- LogitechLedEnginesWrapper.dll (64bit)
- Logitech G Hub Software (2020.6.58918)
- Windows 7+ Operating System

# Limitations
- Since the Logitech SDK doesn't provide battery status the value is read directly from Memory, thus the pointer offsets needs to be updated most likely after Logitech updates their Software
- Only 64 bit is supported
- Only one Mouse supported (first one connected)

# Functionality
- Changes RGB Logo color of Mouse according to Battery Charge (from green to red)
- Alpha Mode will also decrease the brightness of the logo as the battery charge lowers

# Compatible Devices
Tested with:
- Logitech G Pro Lightspeed
- Logitech g502 Lightspeed

# Offset
If you're only here for the Battery Memory Pointer (which is oddly a float):
(lghub_agent.exe + 0x015C5048) + 0x10) + 0x0) + 0x30) + 0x58) + 0x2E8) + 0x0) + 0x1B8) + 0x28) + 0x170) + 0x0) + 0x450) + 0x30) + 0x30) + 0x90) + 0x348)

# Download
https://github.com/QuiNz0r/Logitech-Battery-LED/releases
