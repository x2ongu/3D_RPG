using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot_Skill : UI_Drag_Skill
{
    enum Images
    {
        Skill_Icon,
        Block_Image
    }

    enum Texts
    {
        Skill_Name_Text,
        Skill_Description_Text,
        Block_Text
    }

    public override void SetInfo()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetImage((int)Images.Skill_Icon).sprite = m_skillData.m_icon;
        GetImage((int)Images.Block_Image).gameObject.SetActive(true);
        GetText((int)Texts.Skill_Name_Text).text = m_skillData.m_name;
        GetText((int)Texts.Skill_Description_Text).text = m_skillData.m_desc;
        GetText((int)Texts.Block_Text).text = $"레벨 {m_skillData.m_limitedLevel}이상 사용 가능";
    }

    private void Update()
    {
        if (GameManager.Inst.m_player.m_stat.Level >= m_skillData.m_limitedLevel)
            GetImage((int)Images.Block_Image).gameObject.SetActive(false);
    }

    protected override void OnBeginDragSlot(PointerEventData eventData)
    {
        BindImage(typeof(Images));

        if (!GetImage((int)Images.Block_Image).gameObject.activeSelf)
            base.OnBeginDragSlot(eventData);
    }
}
