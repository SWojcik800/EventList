package com.example.eventlistmobileapp.UI

import android.content.Context
import android.util.AttributeSet
import android.view.LayoutInflater
import android.widget.FrameLayout
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

    fun setTitle(title: String) {
        binding.cardTitle.text = title
    }

    fun setDescription(description: String) {
        binding.cardDescription.text = description
    }

    fun setCardOnClickListener(listener: OnClickListener) {
        binding.cardContainer.setOnClickListener(listener)
    }
}