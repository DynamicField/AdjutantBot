using System;
using System.Collections.Generic;
using Discord;

namespace AdjutantBot.Models
{
    public class TeamModel
    {
        public string TeamName { get; set; }
        public ulong GuildId { get; set; }
        public long GameJamId { get; set; }
        public List<ulong> TeamMemberIds { get; set; }
        public ulong TeamCaptainId { get; set; }

        public int TeamLimit { get; set; }
        
        //
        /// <summary>
        /// this is for reflection purposes for the API stuff that Matt will implement later. DO NOT USE.
        /// </summary>
        [Obsolete]
        public TeamModel()
        {
            
        }
        
        /// <summary>
        /// Creates an object of type TeamModel
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="guildId"></param>
        /// <param name="gameJamId"></param>
        /// <param name="teamMemberIds"></param>

        public TeamModel(string teamName, ulong guildId, long gameJamId, ulong teamMemberId)
        {
            TeamLimit = 1;
            TeamCaptainId = teamMemberId;
            TeamName = teamName;
            GuildId = guildId;
            GameJamId = gameJamId;
            TeamMemberIds = new List<ulong>() {TeamCaptainId};
        }
    }
}