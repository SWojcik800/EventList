package com.example.eventlistmobileapp.Commons.Helpers

import java.text.SimpleDateFormat
import java.util.*

object DateFormatter {

    fun formatDate(dateString: String,
                   inputFormat: SimpleDateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()),
                   outputFormat: SimpleDateFormat = SimpleDateFormat("dd MMMM yyyy, HH:mm:ss", Locale.getDefault())): String {
        val date: Date = inputFormat.parse(dateString)
        return outputFormat.format(date)
    }
}