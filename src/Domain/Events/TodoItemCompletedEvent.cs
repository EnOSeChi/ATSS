﻿using RecruitmentTask.Domain.Common;
using RecruitmentTask.Domain.Entities;

namespace RecruitmentTask.Domain.Events
{
    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
