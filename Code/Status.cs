using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatusType { Poison, Unconscious, Panic, Charm, Stun, Paralyze, Fear, Invisible, None }

[CreateAssetMenu(fileName = "New Status", menuName = "Status")]
public class Status : ScriptableObject
{
    public string statusName;
    public string desc;
    public Sprite statusSprite;
    public StatusType type;
}
