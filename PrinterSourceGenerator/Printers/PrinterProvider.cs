﻿using System.Collections.Generic;

#nullable enable

namespace PrinterSourceGenerator.Printers
{
    internal partial class PrinterProvider
    {
        private List<string> Servers { get; } = new();

        public PrinterProvider()
        {
            RegisterServers();
        }

        public TPrinter? GetServer<TPrinter>() where TPrinter : IPrinter, new()
        {
            var fullName = typeof(TPrinter).FullName;

            if (fullName is null)
            {
                return default;
            }

            if (Servers.Contains(fullName))
            {
                return new TPrinter();
            }

            return default;
        }

        private void AddServer<TPrinter>() where TPrinter : IPrinter
        {
            var fullName = typeof(TPrinter).FullName;

            if (fullName is null)
            {
                return;
            }

            Servers.Add(fullName);
        }

        /// <summary>
        /// 由框架自动重写。
        /// </summary>
        partial void RegisterServers();
    }


}
