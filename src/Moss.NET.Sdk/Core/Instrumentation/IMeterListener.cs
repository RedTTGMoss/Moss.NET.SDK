using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Moss.NET.Sdk.Core.Instrumentation;

public interface IMeterListener
{
    internal void Init();
}

public abstract class MeterListener<T> : IMeterListener
    where T : struct
{
    private readonly MeterListener _listener = new();

    public void Init()
    {
        _listener.InstrumentPublished = (instrument, listener) => listener.EnableMeasurementEvents(instrument);
        _listener.SetMeasurementEventCallback<T>(OnMeasurementRecorded);
        _listener.Start();
    }

    protected abstract void OnMeasurementRecorded(Instrument instrument, T measurement,
        ReadOnlySpan<KeyValuePair<string, object?>> tags,
        object? state);
}