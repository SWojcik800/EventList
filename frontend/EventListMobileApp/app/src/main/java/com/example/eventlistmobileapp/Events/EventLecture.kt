package com.example.eventlistmobileapp.Events

import com.example.eventlistmobileapp.Lectures.Location

data class EventLecture(
    val id: Int,
    val eventId: Int,
    val lecturerNames: List<String?>?,
    val location: String?,
    val startTime: String?,
    val endTime: String?,
    val duration: String?,
    val name: String?,
    val topic: String?,
    val description: String?
)