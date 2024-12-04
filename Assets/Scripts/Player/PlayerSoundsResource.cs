using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

[Serializable]
public class PlayerSoundsResource : ScriptableObject
{
    public List<AudioClip> WalkSounds;
    public List<AudioClip> HitSounds;
    public List<AudioClip> PickupSounds;
    public List<AudioClip> DraggingSounds;
    public List<AudioClip> JumpSounds;
    public List<AudioClip> AttackSounds;
    public List<AudioClip> LedgeSounds;



}
