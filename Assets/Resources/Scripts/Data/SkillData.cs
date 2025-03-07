using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType { BaseSkill, FinalSkill }

[CreateAssetMenu]
public class SkillData : ScriptableObject
{
    public string m_name;

    public SkillType m_skillType;

    [TextArea]
    public string m_desc;

    public float m_damage;

    public float m_coolTime;

    public string m_animName;

    public Sprite m_icon;

    public int m_limitedLevel;

    public int m_mp;
}
