﻿namespace EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces
{
    public interface IEntityWithUserContext<T>
    {
        T UserId { get; set; }
    }
}
