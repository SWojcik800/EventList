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
import com.example.eventlistmobileapp.databinding.CardComponentBinding

class CardComponent @JvmOverloads constructor(
    context: Context,
    attrs: AttributeSet? = null,
    defStyleAttr: Int = 0,
) : FrameLayout(context, attrs, defStyleAttr) {

    private val binding: CardComponentBinding

    init {
        binding = CardComponentBinding.inflate(LayoutInflater.from(context), this, true)
    }


    fun setPropertiesFrom(cardItems: List<CardComponentItem>): CardComponent
    {
        createList(cardItems)
        return this
    }
    fun setTitle(title: String): CardComponent {
        binding.cardTitle.text = title
        return this
    }

    fun setCardOnClickListener(listener: OnClickListener) {
        binding.cardContainer.setOnClickListener(listener)
    }

    private fun createList(items: List<CardComponentItem>)
    {
        var listView = findViewById<ListView>(R.id.cardListItems);
        val mappedItems = items.map { item -> "${item.key}: ${item.value}" }.toList()

        val arrayAdapter = ArrayAdapter(this.context,
            android.R.layout.simple_list_item_1, mappedItems)

        listView.adapter = arrayAdapter
    }
}