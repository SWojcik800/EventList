package com.example.eventlistmobileapp

import android.content.Context
import android.util.AttributeSet
import android.view.LayoutInflater
import android.widget.FrameLayout

class CardComponent @JvmOverloads constructor(
    context: Context,
    attrs: AttributeSet? = null,
    defStyleAttr: Int = 0,
) : FrameLayout(context, attrs, defStyleAttr) {

    init {
        LayoutInflater.from(context).inflate(R.layout.card_component, this, true)
    }

    fun setTitle(title: String) {
        cardTitle.text = title
    }

    fun setDescription(description: String) {
        cardDescription.text = description
    }

    fun setOnClickListener(listener: OnClickListener) {
        cardContainer.setOnClickListener(listener)
    }
}