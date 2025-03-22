// Copyright (c) 2025 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;

namespace RhubarbGeekNz.PSWriteLine
{
    [Cmdlet(VerbsCommunications.Write, "PSHost")]
    sealed public class WritePSHost : PSCmdlet
    {
        [Parameter(Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [Alias("Msg", "Message")]
        public object Object { get; set; }

        [Parameter]
        public SwitchParameter NoNewline
        {
            get
            {
                return noNewline;
            }

            set
            {
                noNewline = value;
            }
        }
        private bool noNewline;

        [Parameter]
        public object Separator { get; set; } = " ";

        [Parameter]
        public object ForegroundColor { get; set; }

        [Parameter]
        public object BackgroundColor { get; set; }

        private object psReadLineOption;

        private ConsoleColor foregroundColor, backgroundColor;
        private bool bForegroundColor, bBackgroundColor;

        protected override void BeginProcessing()
        {
            bForegroundColor = TryColor(ForegroundColor, 0, out foregroundColor);
            bBackgroundColor = TryColor(BackgroundColor, 0, out backgroundColor);
        }

        protected override void ProcessRecord()
        {
            string result = ProcessObject(Object);

            HostInformationMessage informationMessage = new HostInformationMessage();
            informationMessage.Message = result;
            informationMessage.NoNewLine = NoNewline.IsPresent;

            try
            {
                if (bForegroundColor)
                {
                    informationMessage.ForegroundColor = foregroundColor;
                }

                if (bBackgroundColor)
                {
                    informationMessage.BackgroundColor = backgroundColor;
                }
            }
            catch (System.Management.Automation.Host.HostException)
            {
            }

            WriteInformation(informationMessage, new string[] { "PSHOST" });
        }

        private bool TryColor(object color, int recurse, out ConsoleColor consoleColor)
        {
            if (color != null)
            {
                if (recurse > 10)
                {
                    throw new ArgumentException($"Color error {color}");
                }

                if (color is ConsoleColor cc)
                {
                    consoleColor = cc;
                    return true;
                }

                if (color is string s)
                {
                    if (s.Length > 0)
                    {
                        if (s[0] == '#')
                        {
                            int intValue = int.Parse(s.Substring(1), System.Globalization.NumberStyles.HexNumber);
                            Color rgb = Color.FromArgb(intValue);

                            consoleColor = PSColorMap.GetNearestConsoleColor(rgb);

                            return true;
                        }

                        if (s.StartsWith("\u001B[") && s.EndsWith("m"))
                        {
                            int[] args = s.Substring(2, s.Length - 3).Split(';').Select(Int32.Parse).ToArray();

                            if (args.Length > 0)
                            {
                                int code = args[0];

                                if ((code >= 30) && (code < 38))
                                {
                                    consoleColor = PSColorMap.ConsoleColorMap[code - 30].ConsoleColor;

                                    return true;
                                }

                                if ((code >= 40) && (code < 48))
                                {
                                    consoleColor = PSColorMap.ConsoleColorMap[code - 40].ConsoleColor;

                                    return true;
                                }

                                if ((code >= 90) && (code < 98))
                                {
                                    consoleColor = PSColorMap.ConsoleColorMap[code - 82].ConsoleColor;

                                    return true;
                                }

                                if ((code >= 100) && (code < 108))
                                {
                                    consoleColor = PSColorMap.ConsoleColorMap[code - 92].ConsoleColor;

                                    return true;
                                }

                                if ((code == 38) || (code == 48))
                                {
                                    switch (args[1])
                                    {
                                        case 2:
                                            consoleColor = PSColorMap.GetNearestConsoleColor(Color.FromArgb(0xFF, args[2], args[3], args[4]));
                                            return true;

                                        case 5:
                                            code = args[2];

                                            if (code < 16)
                                            {
                                                consoleColor = PSColorMap.ConsoleColorMap[code].ConsoleColor;
                                            }
                                            else
                                            {
                                                if (code < 232)
                                                {
                                                    code -= 16;
                                                    consoleColor = PSColorMap.GetNearestConsoleColor(Color.FromArgb(
                                                        0xFF,
                                                        ((code / 36) % 6) * 51,
                                                        ((code / 6) % 6) * 51,
                                                        (code % 6) * 51));
                                                }
                                                else
                                                {
                                                    code = (code - 232) * 11;
                                                    consoleColor = PSColorMap.GetNearestConsoleColor(Color.FromArgb(0xFF, code, code, code));
                                                }
                                            }

                                            return true;
                                    }
                                }
                            }

                            consoleColor = ConsoleColor.Black;

                            return false;
                        }

                        if (Enum.TryParse(s, true, out consoleColor))
                        {
                            return true;
                        }

                        if (psReadLineOption == null)
                        {
                            using (PowerShell shell = PowerShell.Create(RunspaceMode.CurrentRunspace))
                            {
                                shell.AddCommand("Get-PSReadLineOption");

                                var result = shell.Invoke();

                                if (result.Count == 1)
                                {
                                    psReadLineOption = result[0].BaseObject;
                                }
                            }
                        }

                        if (psReadLineOption != null)
                        {
                            var prop = psReadLineOption.GetType().GetProperty(s);

                            if (prop != null)
                            {
                                object value = prop.GetValue(psReadLineOption, null);

                                if (value != null)
                                {
                                    return TryColor(value, recurse + 1, out consoleColor);
                                }
                            }
                        }

                        Color clr = Color.FromName(s);

                        if (clr.ToArgb() != 0)
                        {
                            consoleColor = PSColorMap.GetNearestConsoleColor(clr);

                            return true;
                        }
                    }
                }

                if (color is Color c)
                {
                    consoleColor = PSColorMap.GetNearestConsoleColor(c);

                    return true;
                }

                if (color is Int32 i)
                {
                    if ((i >= 0) && (i < 16))
                    {
                        consoleColor = (ConsoleColor)i;
                        return true;
                    }
                }

                Exception ex = new ArgumentException($"Unable to resolve color: {color}");

                WriteError(new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.InvalidArgument, color));
            }

            consoleColor = ConsoleColor.Black;

            return false;
        }

        private string ProcessObject(object o)
        {
            if (o != null)
            {
                if (o is string s)
                {
                    return s;
                }
                else if (o is IEnumerable enumerable)
                {
                    var result = new List<string>();

                    foreach (var e in enumerable)
                    {
                        ProcessObject(e, result);
                    }

                    return String.Join(Separator == null ? String.Empty : Separator.ToString(), result);
                }
                else
                {
                    return o.ToString();
                }
            }

            return String.Empty;
        }

        private void ProcessObject(object o, List<string> result)
        {
            while (o != null)
            {
                if (o is string s)
                {
                    if (s.Length > 0)
                    {
                        result.Add(s);
                    }

                    break;
                }
                else if (o is IEnumerable enumerable)
                {
                    foreach (var e in enumerable)
                    {
                        ProcessObject(e, result);
                    }

                    break;
                }
                else
                {
                    o = o.ToString();
                }
            }
        }
    }
}
