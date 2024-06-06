using Godot;
using System;
using System.Collections.Generic;

public delegate void SpeakEndedCsEventHandler();


public interface SpeakerInterface
{
    public void StartSpeak();

    public void EndSpeak();

    public event SpeakEndedCsEventHandler SpeakEndedCs;
}