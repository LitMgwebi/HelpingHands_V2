﻿using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IOperation
    {
        public Task<IEnumerable<OperationHour>> GetOperationHours();

        public Task<OperationHour> GetOperation(int? id);

        public Task<dynamic> AddOperationHours(OperationHour operation);

        public Task<dynamic> UpdateOperationHour(OperationHour operation);

        public Task<dynamic> DeleteOperationHour(int id);
    }
}
