package com.example.eventlistmobileapp

import android.content.Context
import android.graphics.Color
import android.graphics.Paint.Style
import android.os.Bundle
import android.os.StrictMode
import android.os.StrictMode.ThreadPolicy
import android.view.Menu
import android.view.MenuItem
import android.widget.TableLayout
import android.widget.TableRow
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import com.example.eventlistmobileapp.Commons.ApiService
import com.example.eventlistmobileapp.Commons.AppConsts
import com.example.eventlistmobileapp.databinding.ActivityMainBinding
import com.google.android.material.snackbar.Snackbar
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        val policy = ThreadPolicy.Builder().permitAll().build()
        StrictMode.setThreadPolicy(policy)
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        val navController = findNavController(R.id.nav_host_fragment_content_main)
        appBarConfiguration = AppBarConfiguration(navController.graph)
        setupActionBarWithNavController(navController, appBarConfiguration)

        binding.fab.setOnClickListener { view ->
            Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                .setAction("Action", null).show()
        }

        loadTableData();

    }

    private fun loadTableData() {



        val retrofit = Retrofit.Builder()
            .baseUrl(AppConsts.apiBaseUrl) // Replace with your API base URL
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        try {
            val apiService = retrofit.create(ApiService::class.java)
            val items = apiService.getLectures().execute().body()?.items
            val tableLayout = findViewById<TableLayout>(R.id.tableLayout)

            val headerRow = createHeaderTableRow(this, "Lecturers", "Location", "StartTime", "EndTime", "Duration", "Name", "Topic", "Description")
            headerRow.setBackgroundColor(Color.parseColor("#4834d4"))
            tableLayout.addView(headerRow)

            items?.forEach { item ->
                val lectureRow = createTableRow(
                    this,
                    item.lecturerNames.joinToString(", "),
                    "${item.location.country} ${item.location.city} ${item.location.street}",
                    item.startTime.toString(),
                    item.endTime.toString(),
                    item.duration.toString(),
                    item.name ?: "",
                    item.topic ?: "",
                    item.description ?: ""
                )

                if (tableLayout.childCount % 2 == 0) {
                    lectureRow.setBackgroundColor(Color.LTGRAY)
                }
                tableLayout.addView(lectureRow)
            }

        }
        catch (e: java.lang.Exception)
        {
            print(e)
        }


    }

    fun createHeaderTableRow(context: Context, vararg columnTexts: String): TableRow {
        val tableRow = TableRow(context)
        columnTexts.forEach { text ->
            val textView = TextView(context)
            textView.text = text
            textView.setPadding(2, 2, 2, 2)
            textView.setTextColor(Color.parseColor("#ffffff"))
            tableRow.addView(textView)
        }
        return tableRow
    }
    fun createTableRow(context: Context, vararg columnTexts: String): TableRow {
        val tableRow = TableRow(context)
        columnTexts.forEach { text ->
            val textView = TextView(context)
            textView.text = text
            textView.setPadding(2, 2, 2, 2)
            tableRow.addView(textView)
        }
        return tableRow
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.menu_main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        return when (item.itemId) {
            R.id.action_settings -> true
            else -> super.onOptionsItemSelected(item)
        }
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()
    }
}