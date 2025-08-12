using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Player _player;
    public Player Player {  get { return _player; } set { _player = value; } }

    public JumpPlayer _jumpPlayer;
    public JumpPlayer JumpPlayer { get { return _jumpPlayer; } set { _jumpPlayer = value; } }
}
