using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] public int hp;
    [SerializeField] public int dmg;
    [SerializeField] public AnimationClip clipAttack;
    [SerializeField] public AnimationClip clipAttack2;
    [SerializeField] public float rangeAttack;
}
