﻿using Dapr.Actors;

namespace sample.microservice.reservationactor.interfaces;

public interface IReservationItemActor : IActor
{       
    Task<int> AddReservation(int quantity);

    Task<int> GetBalance();

    Task RegisterReminder();

    Task UnregisterReminder();

    Task RegisterTimer();

    Task UnregisterTimer();
}
