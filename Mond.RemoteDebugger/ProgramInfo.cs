﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Mond.Debugger;

namespace Mond.RemoteDebugger
{
    internal class ProgramInfo
    {
        private readonly object _sync = new object();
        private readonly List<int> _breakpoints;

        public string FileName { get; }
        public MondProgram Program { get; }
        public MondDebugInfo DebugInfo { get; }

        public ReadOnlyCollection<int> Breakpoints
        {
            get
            {
                lock (_sync)
                    return _breakpoints.ToList().AsReadOnly();
            }
        }

        public ProgramInfo(MondProgram program)
        {
            _breakpoints = new List<int>(16);

            Program = program;
            DebugInfo = program.DebugInfo;

            FileName = DebugInfo?.FileName ?? Program.GetHashCode().ToString("X8");
        }

        public void AddBreakpoint(int line)
        {
            lock (_sync)
                _breakpoints.Add(line);
        }

        public void RemoveBreakpoint(int line)
        {
            lock (_sync)
                _breakpoints.Remove(line);
        }

        public bool ContainsBreakpoint(int line)
        {
            lock (_sync)
                return _breakpoints.Contains(line);
        }
    }
}
