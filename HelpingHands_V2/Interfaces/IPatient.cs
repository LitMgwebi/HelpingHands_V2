﻿namespace HelpingHands_V2.Interfaces
{
    public interface IPatient
    {
        public List<dynamic> GetPatients();

        public dynamic GetPatient(int? id);
    }
}
