package com.example.eventlistmobileapp.UI

import android.content.Context
import android.util.AttributeSet
import android.view.LayoutInflater
import android.widget.ArrayAdapter
import android.widget.FrameLayout
import android.widget.ListView
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

    fun setTitle(title: String): CardComponent {
        binding.cardTitle.text = title
        return this
    }

    fun setDescription(description: String): CardComponent {
        binding.cardDescription.text = description
        return this
    }

    fun setCardOnClickListener(listener: OnClickListener) {
        binding.cardContainer.setOnClickListener(listener)
    }
}