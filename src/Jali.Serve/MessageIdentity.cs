﻿using System;

namespace Jali.Serve
{
    public class MessageIdentity
    {
        public string TransactionId { get; set; }
        public string SessionId { get; set; }
        public string ConversationId { get; set; }
        public string MessageId { get; set; }
        public string MessageTransmissionId { get; set; }
        public string UserId { get; set; }
        public string ImpersonatorId { get; set; }
        public string DeputyId { get; set; }

        public MessageIdentity CreateResponseIdentity()
        {
            return new MessageIdentity
            {
                TransactionId = this.TransactionId,
                SessionId = this.SessionId,
                ConversationId = this.ConversationId,
                MessageId = Guid.NewGuid().ToString("D").ToUpperInvariant(),
                MessageTransmissionId = Guid.NewGuid().ToString("D").ToUpperInvariant(),
                UserId = this.UserId,
                ImpersonatorId = this.ImpersonatorId,
                DeputyId = this.DeputyId,
            };
        }
    }
}