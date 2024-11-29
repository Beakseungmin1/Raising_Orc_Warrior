using System.Collections.Generic;

public class SkillInventory : IInventory<SkillSO>
{
    private List<SkillSO> skills = new List<SkillSO>();

    public void AddItem(SkillSO item)
    {
        skills.Add(item);
    }

    public void RemoveItem(SkillSO item)
    {
        skills.Remove(item);
    }

    public SkillSO GetItem(string itemName)
    {
        return skills.Find(skill => skill.itemName == itemName);
    }

    public List<SkillSO> GetAllItems()
    {
        return new List<SkillSO>(skills);
    }

    public int GetItemCount()
    {
        return skills.Count;
    }
}