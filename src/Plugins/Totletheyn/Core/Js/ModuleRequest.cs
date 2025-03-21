﻿using System;

namespace Totletheyn.Core.Js;

public sealed class ModuleRequest
{
    public ModuleRequest(Module initiator, string cmdArgument, string absolutePath)
    {
        if (string.IsNullOrWhiteSpace(cmdArgument)) throw new ArgumentException("message", nameof(cmdArgument));

        if (string.IsNullOrWhiteSpace(absolutePath)) throw new ArgumentException("message", nameof(absolutePath));

        Initiator = initiator ?? throw new ArgumentNullException(nameof(initiator));
        CmdArgument = cmdArgument;
        AbsolutePath = absolutePath;
    }

    public Module Initiator { get; }
    public string CmdArgument { get; }
    public string AbsolutePath { get; }
}