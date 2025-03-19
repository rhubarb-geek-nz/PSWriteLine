#!/usr/bin/env pwsh
# Copyright (c) 2025 Roger Brown.
# Licensed under the MIT License.

trap
{
	throw $PSItem
}

$ErrorActionPreference = 'Stop'
$InformationPreference = 'Continue'

$Reset = "$([char]0x1b)[38m$([char]0x1b)[48m"

Write-Host "#### Base colors"

Write-Host -Foreground ([ConsoleColor]::Black) "Black $($PSStyle.Black)Black$Reset"
Write-Host -Foreground ([ConsoleColor]::DarkRed) "DarkRed $($PSStyle.Red)Red$Reset"
Write-Host -Foreground ([ConsoleColor]::DarkGreen) "DarkGreen $($PSStyle.Green)Green$Reset"
Write-Host -Foreground ([ConsoleColor]::DarkYellow) "DarkYellow $($PSStyle.Yellow)Yellow$Reset"
Write-Host -Foreground ([ConsoleColor]::DarkBlue) "DarkBlue $($PSStyle.Blue)Blue$Reset"
Write-Host -Foreground ([ConsoleColor]::DarkMagenta) "DarkMagenta $($PSStyle.Magenta)Magenta$Reset"
Write-Host -Foreground ([ConsoleColor]::DarkCyan) "DarkCyan $($PSStyle.Cyan)Cyan$Reset"
Write-Host -Foreground ([ConsoleColor]::Gray) "Gray $($PSStyle.White)White$Reset"

Write-Host -Foreground ([ConsoleColor]::DarkGray) "DarkGray $($PSStyle.BrightBlack)BrightBlack$Reset"
Write-Host -Foreground ([ConsoleColor]::Red) "Red $($PSStyle.BrightRed)BrightRed$Reset"
Write-Host -Foreground ([ConsoleColor]::Green) "Green $($PSStyle.BrightGreen)BrightGreen$Reset"
Write-Host -Foreground ([ConsoleColor]::Yellow) "Yellow $($PSStyle.BrightYellow)BrightYellow$Reset"
Write-Host -Foreground ([ConsoleColor]::Blue) "Blue $($PSStyle.BrightBlue)BrightBlue$Reset"
Write-Host -Foreground ([ConsoleColor]::Magenta) "Magenta $($PSStyle.BrightMagenta)BrightMagenta$Reset"
Write-Host -Foreground ([ConsoleColor]::Cyan) "Cyan $($PSStyle.BrightCyan)BrightCyan$Reset"
Write-Host -Foreground ([ConsoleColor]::White) "White $($PSStyle.BrightWhite)BrightWhite$Reset"

Write-Host "#### Enumerate by color name"

foreach ($i in 0..15)
{
	$color=([ConsoleColor]$i)
	$colorName="$color"

	Write-PSHost -ForegroundColor $color "$colorName " -NoNewline
	Write-PSHost -ForegroundColor $colorName $colorName
}

Write-Host "#### Enumerate by color index"

foreach ($i in 30..37)
{
	$escape = "$([char]0x1b)[$i"+"m"

	Write-PSHost -ForegroundColor $escape ("`$([char]0x1b)[$i"+"m")
}

foreach ($i in 40..47)
{
	$escape = "$([char]0x1b)[$i"+"m"

	Write-PSHost -ForegroundColor $escape ("`$([char]0x1b)[$i"+"m")
}

foreach ($i in 90..97)
{
	$escape = "$([char]0x1b)[$i"+"m"

	Write-PSHost -ForegroundColor $escape ("`$([char]0x1b)[$i"+"m")
}

foreach ($i in 100..107)
{
	$escape = "$([char]0x1b)[$i"+"m"

	Write-PSHost -ForegroundColor $escape ("`$([char]0x1b)[$i"+"m")
}

Write-Host "#### Enumerate by 256 color index"

foreach ($i in 0..255)
{
	$escape = "$([char]0x1b)[38;5;$i"+"m"

	Write-PSHost -ForegroundColor $escape "NEAREST $escape ACTUAL $i $reset" -NoNewline
	Write-PSHost -BackgroundColor $escape "$escape OBSCURE $i $reset" -NoNewline
	Write-PSHost
}

Write-Host "#### Get-PSReadLineOption"

foreach ($i in 'ErrorColor', 'EmphasisColor', 'KeywordColor')
{
	Write-PSHost -ForegroundColor $i $i
}

Write-Host "#### Hex RGB values"

foreach ($i in '#FF0000', '#00FF00', '#0000FF')
{
	Write-PSHost -ForegroundColor $i $i
}

foreach ($i in '#FF0000', '#00FF00', '#0000FF')
{
	Write-PSHost -BackgroundColor $i $i -NoNewline
	Write-PSHost
}

foreach ($i in '#FF0000', '#00FF00', '#0000FF')
{
	$j = $i.Replace('FF','80')
	Write-PSHost -ForegroundColor $j -BackgroundColor $i $j $i -NoNewline
	Write-PSHost
}

Write-Host "#### Ansi RGB values"

foreach ($i in "$([char]0x1b)[38;2;255;0;0m", "$([char]0x1b)[38;2;0;255;0m", "$([char]0x1b)[38;2;0;0;255m")
{
	$j = $i.Replace("$([char]0x1b)","`$([char]0x1b)")
	Write-PSHost -BackgroundColor $i $j $i $j -NoNewline
	Write-PSHost
}

Write-Host "#### Multiple parameters"

Write-PSHost ONE TWO THREE FOUR

Write-Host "#### Failed color lookup"

$color = 'Bad Jelly'

try
{
	Write-PSHost -ForegroundColor $color $color
}
catch
{
	$PSItem
}

Write-Host "#### Finally, with a separator"

Write-PSHost -Separator '*' THE END -NoNewline -ForegroundColor 'White' -BackgroundColor 'Green'
Write-PSHost $Reset
