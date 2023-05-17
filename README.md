# Event List

## Project purpose

A mobile app capable of displaying information about upcoming Events created by an app administrator.

## Technologies used

- .NET Core 6
- MediatR
- FluentValidation
- Log4Net
- Kotlin

## Credits
The project is based on this Vertical Slice app template: https://github.com/nadirbad/VerticalSliceArchitecture

***

# Launch instructions

## Backend

1. With SQL Server running, create database using ef core (e.g. from Visual Studio Package Manager Console):
    > dotnet ef database update
1. Run the app, Swagger should appear

## Frontend (Blue Stacks emulator)

1. Install Android SDK from Android Studio (confirmed to work with Android SDK 9)
1. Run Blue Stacks 5, enable Hyper-V if prompted during first launch
1. Enable ADB in Blue Stacks:
    Blue Stacks 5 -> Settings -> Advanced -> Android Debug Bridge
1. Reopen Android Studio project
1. Locate adb.exe path (usually %localappdata%\Android\Sdk\platform-tools)
1. In adb.exe path run:
    > .\adb connect localhost:\<emualtor port, like 55435\>
    >> The port might be different, you can find it in: Blue Stacks 5 -> Settings -> Advanced -> Android Debug Bridge
1. If Android Studio does not list new device, click Troubleshoot Device Connections and follow troubleshooting steps
1. Run the app