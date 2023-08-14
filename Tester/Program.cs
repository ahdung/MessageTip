// Copyright (c) AhDung. All Rights Reserved.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AhDung;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
#if NET
        //ApplicationConfiguration.Initialize();
        Application.SetDefaultFont(SystemFonts.DefaultFont);
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FmMDI());
    }
}