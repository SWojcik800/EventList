package com.example.eventlistmobileapp.Lectures

data class Lecture(
    val id: Int,
    val eventId: Int,
    val lecturerNames: List<String?>,
    val location: Location,
    val startTime: String,
    val endTime: String,
    val duration: String,
    val name: String?,
    val topic: String?,
    val description: String?
)
