package com.example.eventlistmobileapp.Events

import java.io.Serializable

class Event (
    val id: Integer,
    val name: String?,
    val startDate: String?,
    val lectures: List<EventLecture>
    ) : Serializable
