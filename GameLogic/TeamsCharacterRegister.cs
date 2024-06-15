

using System.Collections.Generic;
using Godot;

public partial class TeamsCharacterRegister : Node
{
    public Dictionary<int, List<ITeamMember>> teamIdToMembers { get; set; } = new Dictionary<int,List<ITeamMember>>();

    public void AddMemberToTeam(int id, ITeamMember teamMember)
    {
        if (this.teamIdToMembers.ContainsKey(id))
        {
            this.teamIdToMembers[id].Add(teamMember);
        }
        else
        {
            this.teamIdToMembers.Add(id, new List<ITeamMember>{teamMember});
        }
    }

}