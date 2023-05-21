package com.example.eventlistmobileapp.Commons.Helpers

import com.example.eventlistmobileapp.Commons.ApiService
import com.example.eventlistmobileapp.Commons.AppConsts
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.text.SimpleDateFormat
import java.util.*

object ApiHelper {

    fun getApiServiceInstance(
    ): ApiService {
        val retrofit = Retrofit.Builder()
            .baseUrl(AppConsts.apiBaseUrl) // Replace with your API base URL
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        return retrofit.create(ApiService::class.java)
    }
}