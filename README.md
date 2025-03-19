# rhubarb-geek-nz.PSWriteLine
Write-PSHost tool for PowerShell

# Overview

Write-PSHost is a replacement for Write-Host with enchanced color lookup.

# Details

The colors for ForegroundColor and BackgroundColor can be defined as

Type | Example Value | Comment
---- | ----- | -------
ConsoleColor | [System.ConsoleColor]::DarkRed | .NET color type
Color | [System.Drawing.Color]::Green | Drawing primitive color
Int32 | 4 | Index to convert to ConsoleColor
String | "`e[38;5;4m" | Ansi 16 colour escape sequence
String | "`e[38;5;192;m" | Ansi 216 colour escape sequence
String | "`e[38;5;243;m" | Ansi 24 gray scale escape sequence
String | "`e[38;2;192;233;63" | Ansi 256/256/256 RGB value escape sequence
String | "#453332" | Hex RGB value
String | "Blue" | Name lookup as ConsoleColor
String | "Navy" | Name lookup as KnownColor
String | "ErrorColor" | Property lookup from result of Get-PSReadLineOption

Once the true color has been determined then the closest ConsoleColor is used.

# Messaging

Messages are written using [WriteInformation](https://learn.microsoft.com/en-us/dotnet/api/system.management.automation.cmdlet.writeinformation) with [HostInformationMessage](https://learn.microsoft.com/en-us/dotnet/api/system.management.automation.hostinformationmessage).

# Syntax

```
Write-PSHost [[-Object] <Object>] [-NoNewline] [-Separator <Object>] [-ForegroundColor <Object>] [-BackgroundColor <Object>] 
```
