package com.example.eventlistmobileapp.Events

data class Event (
    val id: Integer,
    val name: String?,
    val startDate: String?,
    val lectures: List<EventLecture>
    )
