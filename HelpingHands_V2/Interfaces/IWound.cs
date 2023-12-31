﻿using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IWound
    {
        public Task<IEnumerable<Wound>> GetWounds();

        public Task<Wound> GetWound(int? id);

        public Task<dynamic> AddWound(Wound wound);

        public Task<dynamic> UpdateWound(Wound wound);

        public Task<dynamic> DeleteWound(int id);
    }
}
