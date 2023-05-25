package com.example.eventlistmobileapp.UI

import android.content.Context
import android.util.AttributeSet
import android.view.LayoutInflater
import android.widget.ArrayAdapter
import android.widget.FrameLayout
import android.widget.LinearLayout
import android.widget.ListView
import com.example.eventlistmobileapp.Lectures.Lecture
import com.example.eventlistmobileapp.R
import com.example.eventlistmobileapp.databinding.CardDetailsComponentBinding

class CardDetailsComponent @JvmOverloads constructor(
    context: Context,
    attrs: AttributeSet? = null,
    defStyleAttr: Int = 0,
) : FrameLayout(context, attrs, defStyleAttr) {

    private val binding: CardDetailsComponentBinding

    init {
        binding = CardDetailsComponentBinding.inflate(LayoutInflater.from(context), this, true)
    }


    fun setPropertiesFrom(cardItems: List<CardComponentItem>): CardDetailsComponent
    {
        createList(cardItems)
        return this
    }
    fun setTitle(title: String): CardDetailsComponent {
        binding.cardTitle.text = title
        return this
    }

    private fun createList(items: List<CardComponentItem>)
    {
        var listView = findViewById<ListView>(R.id.cardDetailsListItems);
        val mappedItems = items.map { item -> "${item.key}: ${item.value}" }.toList()

        val arrayAdapter = ArrayAdapter(this.context,
            android.R.layout.simple_list_item_1, mappedItems)

        listView.adapter = arrayAdapter
    }
}