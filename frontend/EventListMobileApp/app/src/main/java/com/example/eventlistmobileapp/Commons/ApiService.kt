package com.example.eventlistmobileapp.Commons

import com.example.eventlistmobileapp.Events.GetEventsListViewModel
import com.example.eventlistmobileapp.Lecturers.GetLecturersListViewModel
import com.example.eventlistmobileapp.Lectures.GetLecturesListViewModel
import retrofit2.Call
import retrofit2.http.GET

interface ApiService {
    @GET("lectures")
    fun getLectures(): Call<GetLecturesListViewModel>
    @GET("lecturers")
    fun getLecturers(): Call<GetLecturersListViewModel>
    @GET("events")
    fun getEvents(): Call<GetEventsListViewModel>
}