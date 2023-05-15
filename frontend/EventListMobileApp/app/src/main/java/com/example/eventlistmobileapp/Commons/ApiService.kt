package com.example.eventlistmobileapp.Commons

import com.example.eventlistmobileapp.Lectures.GetLecturesListViewModel
import retrofit2.Call
import retrofit2.http.GET

interface ApiService {
    @GET("lectures") // Replace with your API endpoint
    fun getLectures(): Call<GetLecturesListViewModel>
}